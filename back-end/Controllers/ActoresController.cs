using AutoMapper;
using back_end.Dto;
using back_end.Entidades;
using back_end.Utilidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class ActoresController : ControllerBase
    {

        public ApplicationDbContext Context { get; }
        public IMapper Mapper { get; }
        public IAlmanecenadorArchivos AlmacenadorArchivos { get; }


        public ActoresController(ApplicationDbContext context,
            IMapper mapper,
            IAlmanecenadorArchivos almacenadorArchivos)
        {
            Context = context;
            Mapper = mapper;
            AlmacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDto>>> Get ([FromQuery] PaginacionDTO paginacionDto)
        {
            IQueryable<Actor> queryable = Context.Actor.AsQueryable();
            await HttpContext.InsertarParametroPaginacionCabecera(queryable);
            var actores = await queryable.OrderBy(x => x.Nombre).Paginar(paginacionDto).ToListAsync();
            return Mapper.Map<List<ActorDto>>(actores);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreacionDto actorCreacionDto)
        {
            Actor actor = Mapper.Map<Actor>(actorCreacionDto);
            if (actorCreacionDto.Foto != null)
            {
                actor.Foto = await AlmacenadorArchivos.GuardarArchivo(Constants.common.contenedor, actorCreacionDto.Foto);
            }
            Context.Add(actor);
            await Context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("buscarPorNombre")]
        public async Task<ActionResult<List<PeliculaActorDto>>> BuscarPorNombre([FromBody] string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return new List<PeliculaActorDto>();
            }
            var actores =  await Context.Actor
                .Where(x => x.Nombre.Contains(nombre))
                .Select(x => new PeliculaActorDto
                {
                    Id = x.Id, Nombre = x.Nombre, Foto = x.Foto
                }).Take(5).ToListAsync();
            return actores;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCreacionDto actorEdicionDto)
        {
            Actor actor = await Context
                .Actor
                .FirstOrDefaultAsync(x => x.Id == id);
            if (actor == null)
            {
                return NotFound();
            }
            actor = Mapper.Map(actorEdicionDto, actor);

            if (actorEdicionDto.Foto != null)
            {
                actor.Foto = await AlmacenadorArchivos.EditarArchivo(Constants.common.contenedor, actorEdicionDto.Foto, actor.Foto);
            }
            await Context.SaveChangesAsync();
            return NoContent();
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<ActorDto>> GetById(int id)
        {
            Actor actor = await Context.
                Actor.
                FirstOrDefaultAsync(x => x.Id == id);
            if (actor == null)
            {
                return NotFound();
            }

            return Mapper.Map<ActorDto>(actor);
        }



        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            Actor existe = await Context
                .Actor.Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            if (existe == null)
            {
                return NotFound();
            }
            Context.Remove(existe);
            await Context.SaveChangesAsync();
            await AlmacenadorArchivos.BorrarArchivo(existe.Foto, Constants.common.contenedor);
            return NoContent();
        }

    }
}
