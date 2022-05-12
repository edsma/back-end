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
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class CinesController : ControllerBase
    {
        public ApplicationDbContext Context { get; }
        public IMapper Mapper { get; }
        public CinesController(ApplicationDbContext Context,
            IMapper Mapper)
        {
            this.Context = Context;
            this.Mapper = Mapper;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CineCreacionDto cineCreacion)
        {
            Cine cine = Mapper.Map<Cine>(cineCreacion);
            Context.Add(cine);
            await Context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<List<CineDto>>> Get([FromQuery] PaginacionDTO paginacionDto)
        {
            IQueryable<Cine> queryable = Context.Cines.AsQueryable();
            await HttpContext.InsertarParametroPaginacionCabecera(queryable);
            var cines = await queryable.OrderBy(x => x.Nombre).Paginar(paginacionDto).ToListAsync();
            return Mapper.Map<List<CineDto>>(cines);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CineDto>> GetById(int id)
        {
            Cine cine = await Context.
                Cines.
                FirstOrDefaultAsync(x => x.Id == id);
            if (cine == null)
            {
                return NotFound();
            }

            return Mapper.Map<CineDto>(cine);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] CineCreacionDto cineEdicionDto)
        {
            Cine cine = await Context
                .Cines
                .FirstOrDefaultAsync(x => x.Id == id);
            if (cine == null)
            {
                return NotFound();
            }
            cine = Mapper.Map(cineEdicionDto, cine);

            await Context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            Cine existe = await Context
                .Cines.Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            if (existe == null)
            {
                return NotFound();
            }
            Context.Remove(existe);
            await Context.SaveChangesAsync();
            return NoContent();
        }


    }
}
