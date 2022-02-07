using back_end.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
            SignInManager<IdentityUser> signInManager)
        {
            UserManager = userManager;
            Configuration = configuration;
            SignInManager = signInManager;
        }

        public UserManager<IdentityUser> UserManager { get; }
        public IConfiguration Configuration { get; }
        public SignInManager<IdentityUser> SignInManager { get; }

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
            Microsoft.AspNetCore.Identity.SignInResult resultado = await SignInManager.PasswordSignInAsync(credenciales.Email,credenciales.Password,isPersistent:false,lockoutOnFailure:false);
            if (resultado.Succeeded)
            {
                return await ConstruirToken(credenciales);
            }
            else
            {
                return BadRequest("Login incorrecto");
            }
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
