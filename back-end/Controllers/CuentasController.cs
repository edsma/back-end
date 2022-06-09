using AutoMapper;
using back_end.Dto;
using back_end.Utilidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace back_end.Controllers
{
    [Route("api/cuentas")]
    [ApiController]
    public class CuentasController : ControllerBase
    {

        public CuentasController(UserManager<IdentityUser> userManager,
            IConfiguration configuration,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context,
            IMapper mapper)
        {
            UserManager = userManager;
            Configuration = configuration;
            SignInManager = signInManager;
            Context = context;
            Mapper = mapper;
        }

        public UserManager<IdentityUser> UserManager { get; }
        public IConfiguration Configuration { get; }
        public SignInManager<IdentityUser> SignInManager { get; }
        public ApplicationDbContext Context { get; }
        public IMapper Mapper { get; }

        [HttpGet("listadoUsuarios")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<ActionResult<List<UsuarioDto>>> ListadoUsuarios([FromQuery] PaginacionDTO paginacioNDto)
        {
            IQueryable<IdentityUser> queryable = Context.Users.AsQueryable();
            await HttpContext.InsertarParametroPaginacionCabecera(queryable);
            List<IdentityUser> usuarios = await queryable.OrderBy(x => x.Email).Paginar(paginacioNDto).ToListAsync();
            return Mapper.Map<List<UsuarioDto>>(usuarios);
        }

        [HttpPost("HacerAdmin")]
        public async Task<ActionResult> HacerAdmin([FromBody] string usuarioId)
        {
            IdentityUser usuario = await UserManager.FindByIdAsync(usuarioId);
            await UserManager.AddClaimAsync(usuario, new Claim("role", "admin"));
            return NoContent();
        }

        [HttpPost("RemoverAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<ActionResult> RemoverAdmin([FromBody] string usuarioId)
        {
            IdentityUser usuario = await UserManager.FindByIdAsync(usuarioId);
            await UserManager.RemoveClaimAsync(usuario, new Claim("role", "admin"));
            return NoContent();
        }

        [HttpPost("Crear")]
        public async Task<ActionResult<RespuestaAutenticacion>> Crear([FromBody] CredencialesUsuarios credenciales)
        {
            IdentityUser usuario = new IdentityUser
            {
                UserName = credenciales.Email,
                Email = credenciales.Email,
            };
            var resultado = await UserManager.CreateAsync(usuario, credenciales.Password);
            if (resultado.Succeeded)
            {
                await ConstruirToken(credenciales);
                return NoContent();
            }
            else
            {
                return BadRequest(resultado.Errors);
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<RespuestaAutenticacion>> Login([FromBody] CredencialesUsuarios credenciales)
        {
            Microsoft.AspNetCore.Identity.SignInResult resultado = await SignInManager.PasswordSignInAsync(credenciales.Email, credenciales.Password, isPersistent: false, lockoutOnFailure: false);
            if (resultado.Succeeded)
            {
                return await ConstruirToken(credenciales);
            }
            else
            {
                return BadRequest("Login incorrecto");
            }
        }

        [HttpPost("PasswordChange")]
        public async Task<ActionResult<RespuestaAutenticacion>> changePassword(CredencialesUsuarios usermodel)
        {
            var user = await UserManager.FindByEmailAsync(usermodel.Email);
            if (user == null)
            {
                return Ok();
            }
            user.PasswordHash = UserManager.PasswordHasher.HashPassword(user, usermodel.Password);
            var result = await UserManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                //throw exception......
            }
            return Ok();
        }

        private async Task<RespuestaAutenticacion> ConstruirToken(CredencialesUsuarios credenciales)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("email", credenciales.Email)
            };
            IdentityUser usuario = await UserManager.FindByEmailAsync(credenciales.Email);
            IList<Claim> claimsDb = await UserManager.GetClaimsAsync(usuario);
            claims.AddRange(claimsDb);
            SymmetricSecurityKey llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["llavejwt"]));
            SigningCredentials creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
            DateTime expiracion = DateTime.UtcNow.AddYears(1);
            JwtSecurityToken token = new JwtSecurityToken(issuer: null,
                audience: null,
                claims: claims,
                expires: expiracion,
                signingCredentials: creds);
            return new RespuestaAutenticacion()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracion = expiracion
            };
        }
    }
}
