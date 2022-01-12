namespace back_end.Controllers
{
    using AutoMapper;
    using back_end.Dto;
    using back_end.Entidades;
    using back_end.Filtros;
    using back_end.Repositorios;
    using back_end.Utilidades;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class GeneroController : ControllerBase
    {
        private readonly IRepositorio repositorio;
        private readonly ILogger<GeneroController> logger;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GeneroController(IRepositorio repositorio,
            ILogger<GeneroController> logger,
            ApplicationDbContext context,
            IMapper mapper)
        {
            this.repositorio = repositorio;
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }

 
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GeneroDto>> GetById(int id)
        {
            Genero genero = await context.
                Genero.
                FirstOrDefaultAsync(x => x.Id == id);
            if (genero == null)
            {
                return NotFound();
            }

            return mapper.Map<GeneroDto>(genero);
        }

        [HttpGet]
        public async Task<ActionResult<List<GeneroDto>>> Get([FromQuery] PaginacionDTO paginacionDto)
        {

            IQueryable<Genero> queryable = context.Genero.AsQueryable();
            await HttpContext.InsertarParametroPaginacionCabecera(queryable);
            var generos = await queryable.OrderBy(x => x.Nombre).Paginar(paginacionDto).ToListAsync();
            return mapper.Map<List<GeneroDto>>(generos);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] GeneroCreacionDto generoEdicionDto)
        {
            Genero genero = await context
                .Genero
                .FirstOrDefaultAsync(x => x.Id == id);
            if (genero == null)
            {
                return NotFound();
            }
            genero = mapper.Map(generoEdicionDto, genero);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GeneroCreacionDto genero )
        {
            Genero _genero = mapper.Map<Genero>(genero);
            context.Add(_genero);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete]
        public void Delete()
        {

        }

        [HttpGet("guid")]
        public ActionResult<Guid> GetGUID()
        {
            return Ok();
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            bool existe = await context.Genero.AnyAsync(x=> x.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            context.Remove(new Genero() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }


    }
}
