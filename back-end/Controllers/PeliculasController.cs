namespace back_end.Controllers
{
    using AutoMapper;
    using back_end.Dto;
    using back_end.Entidades;
    using back_end.Utilidades;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class PeliculasController : ControllerBase
    {
        public ApplicationDbContext Context { get; }
        public IMapper Mapper { get; }
        public IAlmanecenadorArchivos AlmacenadorArchivos { get; }
        public UserManager<IdentityUser> UserManager { get; }

        public PeliculasController(ApplicationDbContext context,
            IMapper mapper
            , IAlmanecenadorArchivos almacenadorArchivos,
            UserManager<IdentityUser> userManager)
        {
            Context = context;
            Mapper = mapper;
            AlmacenadorArchivos = almacenadorArchivos;
            UserManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<LadingPageDto>> Get()
        {
            int top = 6;
            DateTime hoy = DateTime.Today;
            List<Peliculas> proximosEstrenos = await Context.Peliculas
                .Where(x => x.FechaLanzamiento > hoy)
                .OrderBy(x => x.FechaLanzamiento)
                .Take(top)
                .ToListAsync();
            List<Peliculas> enCines = await Context.Peliculas
                .Where(x => x.EnCines)
                .OrderBy(x => x.FechaLanzamiento)
                .Take(top)
                .ToListAsync();

            LadingPageDto resultado = new LadingPageDto();
            resultado.proximosEstrenos = Mapper.Map<List<PeliculaDto>>(proximosEstrenos);
            resultado.enCines = Mapper.Map<List<PeliculaDto>>(enCines);
            return resultado;
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<PeliculaDto>> Get(int id)
        {
            Peliculas pelicula = await Context
               .Peliculas
               .Include(x => x.PeliculaGenero).ThenInclude(x => x.genero)
               .Include(x => x.PeliculasActores).ThenInclude(x => x.actor)
               .Include(x => x.PeliculaCine).ThenInclude(x => x.Cine)
               .FirstOrDefaultAsync(x => x.Id == id);
            if (pelicula == null)
            {
                return NotFound();
            }
            double promedioVoto = 0.0;
            int usuarioVoto = 0;
            if(await Context.Ratings.AnyAsync(x=> x.PeliculaId == id))
            {
                promedioVoto = await Context.Ratings.Where(x => x.PeliculaId == id)
                    .AverageAsync(x => x.Puntuacion);
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    string email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email").Value;
                    IdentityUser usuario = await UserManager.FindByEmailAsync(email);
                    string usuarioId = usuario.Id;
                    var votoUsuario = await Context.Ratings.FirstOrDefaultAsync(x=> x.UsuarioId == usuarioId
                    && x.PeliculaId == id);
                    if(votoUsuario != null)
                    {
                        usuarioVoto = votoUsuario.Puntuacion;
                    }
                }
            }
            PeliculaDto dto = Mapper.Map<PeliculaDto>(pelicula);
            dto.VotoUsuario = usuarioVoto;
            dto.PromedioVoto = promedioVoto;
            dto.Actor = dto.Actor.OrderBy(x => x.Orden).ToList();
            return dto;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] PeliculaCreacionDto peliculaCreacionDto)
        {
            Peliculas pelicula = Mapper.Map<Peliculas>(peliculaCreacionDto);
            if (peliculaCreacionDto.Poster != null)
            {
                pelicula.Poster = await AlmacenadorArchivos.GuardarArchivo(Constants.common.contenedorPeliculas, peliculaCreacionDto.Poster);
            }
            EscribirOrdenActores(pelicula);
            Context.Add(pelicula);
            await Context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("PostGet")]
        public async Task<ActionResult<PeliculasPostGetDto>> PostGet()
        {
            List<Cine> cine = await Context.Cines.ToListAsync();
            List<Genero> genero = await Context.Genero.ToListAsync();
            List<CineDto> cineDto = Mapper.Map<List<CineDto>>(cine);
            List<GeneroDto> generoDto = Mapper.Map<List<GeneroDto>>(genero);
            return new PeliculasPostGetDto() { Generos = generoDto, Cines = cineDto };

        }

        [HttpGet("PutGet/{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<PeliculasPutGetDto>> PutGet(int id)
        {
            ActionResult<PeliculaDto> peliculaActionResult = await Get(id);
            if (peliculaActionResult.Result is NotFoundResult)
            {
                return NotFound();
            }
            PeliculaDto pelicula = peliculaActionResult.Value;
            List<int> generosSeleccionadosIds = pelicula.Generos.Select(x => x.Id).ToList();
            List<Genero> generosNoSeleccionados = await Context.Genero
                .Where(x => !generosSeleccionadosIds.Contains(x.Id))
                .ToListAsync();
            List<int> cinesSeleccionadosIds = pelicula
                .Cines
                .Select(x => x.Id)
                .ToList();
            List<Cine> cinesNoSeleccionados = await Context.Cines
                .Where(x => !cinesSeleccionadosIds.Contains(x.Id))
                .ToListAsync();
            List<GeneroDto> generosNoSeleccionadosDto = Mapper.Map<List<GeneroDto>>(generosNoSeleccionados);
            List<CineDto> cinesNoSeleccionadosDto = Mapper.Map<List<CineDto>>(cinesNoSeleccionados);
            PeliculasPutGetDto respuesta = new PeliculasPutGetDto();
            respuesta.Pelicula = pelicula;
            respuesta.GenerosSeleccionados = pelicula.Generos;
            respuesta.GenerosNoSeleccionados = generosNoSeleccionadosDto;
            respuesta.CinesSeleccionados = pelicula.Cines;
            respuesta.CinesNoSeleccionados = cinesNoSeleccionadosDto;
            respuesta.Actores = pelicula.Actor;
            return respuesta;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] PeliculaCreacionDto peliculaCreacionDto)
        {
            Peliculas pelicula = await Context.Peliculas
                .Include(x => x.PeliculasActores)
                .Include(x => x.PeliculaGenero)
                .Include(x => x.PeliculaCine)
                .FirstOrDefaultAsync(x => x.Id == id);
            if(pelicula == null)
            {
                return NotFound();
            }

            pelicula = Mapper.Map(peliculaCreacionDto, pelicula);
            if (peliculaCreacionDto.Poster != null)
            {
                pelicula.Poster = await AlmacenadorArchivos.EditarArchivo(Constants.common.contenedorPeliculas,
                    peliculaCreacionDto.Poster, pelicula.Poster);
            }

            EscribirOrdenActores(pelicula);
            await Context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("Filtrar")]
        [AllowAnonymous]
        public async Task<ActionResult<List<PeliculaDto>>> Filtrar([FromQuery] PeliculaFiltrarDto peliculasFiltrarDto)
        {
            IQueryable<Peliculas> peliculaQueryable = Context.Peliculas.AsQueryable();
            if (!string.IsNullOrEmpty(peliculasFiltrarDto.Titulo))
            {
                peliculaQueryable = peliculaQueryable.Where(x => x.Titulo.Contains(peliculasFiltrarDto.Titulo));
            }

            if (peliculasFiltrarDto.enCines)
            {
                peliculaQueryable = peliculaQueryable.Where(x => x.EnCines);
            }

            if (peliculasFiltrarDto.proximosEstrenos)
            {
                DateTime hoy = DateTime.Now;
                peliculaQueryable = peliculaQueryable.Where(x => x.FechaLanzamiento > hoy);
            }

            if (peliculasFiltrarDto.GeneroId != 0)
            {
                peliculaQueryable = peliculaQueryable
                    .Where(x => x.PeliculaGenero.Select(x => x.GeneroId)
                    .Contains(peliculasFiltrarDto.GeneroId));
            }

            await HttpContext.InsertarParametroPaginacionCabecera(peliculaQueryable);
            List<Peliculas> peliculas = await peliculaQueryable.Paginar(peliculasFiltrarDto.PaginacionDto).ToListAsync();
            return Mapper.Map<List<PeliculaDto>>(peliculas);
        }

        [HttpDelete("id:int")]
        public async Task<ActionResult> Delete(int id)
        {
            Peliculas pelicula = await Context.Peliculas.FirstOrDefaultAsync(x => x.Id == id);
            if (pelicula == null)
            {
                return NotFound();
            }
            Context.Remove(pelicula);
            await Context.SaveChangesAsync();
            await AlmacenadorArchivos.BorrarArchivo(pelicula.Poster, Constants.common.contenedorPeliculas);
            return NoContent();
        }


        [HttpGet("PostGet")]
        private void EscribirOrdenActores(Peliculas pelicula)
        {
            if (pelicula.PeliculasActores != null)
            {
                for (int i = 0; i < pelicula.PeliculasActores.Count; i++)
                {
                    pelicula.PeliculasActores[i].Orden = i;
                }
            }
        }
        
    }
}
