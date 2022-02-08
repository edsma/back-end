namespace back_end.Controllers
{
    using back_end.Dto;
    using back_end.Entidades;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    [Route("Api/rating")]
    public class RatingsController : Controller
    {
        public RatingsController(UserManager<IdentityUser> userManager,
            ApplicationDbContext context)
        {
            UserManager = userManager;
            Context = context;
        }

        public UserManager<IdentityUser> UserManager { get; }
        public ApplicationDbContext Context { get; }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post([FromBody] RatingDto ratingDto)
        {
            string email = HttpContext.User.Claims.FirstOrDefault(x=> x.Type == "email").Value;
            IdentityUser usuario = await UserManager.FindByEmailAsync(email);
            string usuarioId = usuario.Id;
            Rating ratingActual = await Context.Ratings
                .FirstOrDefaultAsync(x=> x.PeliculaId == ratingDto.PeliculaId
                && x.UsuarioId == usuarioId);
            if(ratingActual == null)
            {
                var rating = new Rating
                {
                    Puntuacion = ratingDto.Puntuacion,
                    PeliculaId = ratingDto.PeliculaId,
                    UsuarioId = usuarioId
                };
                Context.Add(rating);
                await Context.SaveChangesAsync();
            }
            else
            {
                ratingActual.Puntuacion = ratingDto.Puntuacion;
            }
            await Context.SaveChangesAsync();
            return NoContent();
        }
    }
}
