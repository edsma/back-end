namespace back_end.Controllers
{
    using AutoMapper;
    using back_end.Dto;
    using back_end.Entidades;
    using back_end.Utilidades;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        public ApplicationDbContext Context { get; }
        public IMapper Mapper { get; }
        public IAlmanecenadorArchivos AlmacenadorArchivos { get; }

        public PeliculasController(ApplicationDbContext context, 
            IMapper mapper
            , IAlmanecenadorArchivos almacenadorArchivos )
        {
            Context = context;
            Mapper = mapper;
            AlmacenadorArchivos = almacenadorArchivos;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] PeliculaCreacionDto peliculaCreacionDto)
        {
            Peliculas pelicula = Mapper.Map<Peliculas>(peliculaCreacionDto);
            if (peliculaCreacionDto.Poster != null)
            {
                pelicula.Poster = await AlmacenadorArchivos.GuardarArchivo(Constants.common.contenedorPeliculas,peliculaCreacionDto.Poster);
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
