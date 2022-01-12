using AutoMapper;
using back_end.Dto;
using back_end.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActoresController : ControllerBase
    {

        public ApplicationDbContext Context { get; }
        public IMapper Mapper { get; }
        public ActoresController(ApplicationDbContext context,IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ActorCreacionDto actorCreacionDto)
        {
            Actor actor = Mapper.Map<Actor>(actorCreacionDto);
            Context.Add(actor);
            await Context.SaveChangesAsync();
            return NoContent();
        }

    }
}
