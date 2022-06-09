using AutoMapper;
using back_end.Dto;
using back_end.Entidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static back_end.Utilidades.Constants;

namespace back_end.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class PolizaController : ControllerBase
    {
        public ApplicationDbContext Context { get; }
        public IMapper Mapper { get; }
        public UserManager<IdentityUser> UserManager { get; }

        public PolizaController(ApplicationDbContext context, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            Context = context;
            Mapper = mapper;
            UserManager = userManager;
        }


        [HttpGet]
        public async Task<ActionResult<List<PolizasDto>>> Get()
        {
            int top = 6;
            List<Polizas> polizas = await Context.Polizas
                .OrderBy(x => x.NombreCiudadCliente)
                .Take(top)
                .ToListAsync();

            LadingPageDto resultado = new LadingPageDto();
            var prueba = Mapper.Map<List<PolizasDto>>(polizas);
            return prueba;
        }



        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PolizasDto polizaDto)
        {

            string storedProcedure = $"exec dbo.{Sp.GetPolizas} " +
                    $"'{polizaDto.NombreCliente}', " +
                    $"'{polizaDto.IdentificacionCliente}', " +
                    $"'{polizaDto.FechaNacimientoCliente.ToString("yyyy-MM-dd")}', " +
                    $"'{polizaDto.FechaPoliza.ToString("yyyy-MM-dd")}', " +
                    $"'{polizaDto.CoberturasCubiertas}', " +
                    $"'{polizaDto.ValorMaximoPoliza}', " +
                    $"'{polizaDto.NombrePoliza}', " +
                    $"'{polizaDto.NombreCiudadCliente}', " +
                    $"'{polizaDto.DireccionCliente}', " +
                    $"'{polizaDto.PlacaAutoCliente}', " +
                    $"{(polizaDto.VehiculoCuentaInspeccion == true ? 1 : 0)}, " +
                    $"'{polizaDto.NumeroPoliza}'";
            await Context.Database.ExecuteSqlRawAsync(storedProcedure);
            return NoContent();
        }


        [HttpGet("ConsultarPoliza")]
        public async Task<ActionResult<List<Polizas>>> ConsultarPoliza([FromBody] string valor)
        {
            string storedProcedure = $"exec dbo.{Sp.ConsultarPoliza} " +
                    $"'{valor}'";
            List<Polizas> resultado = await Context.Polizas.FromSqlRaw(storedProcedure).ToListAsync();
            return resultado;
        }


        [HttpPut]
        [AllowAnonymous]
        public async Task<ActionResult> Put(int id, [FromBody] PolizasDto polizaDto)
        {
            string storedProcedure = $"exec dbo.{Sp.ActualizarPoliza} " +
             $"{id}, " +
             $"'{polizaDto.NombreCliente}', " +
             $"'{polizaDto.IdentificacionCliente}', " +
             $"'{polizaDto.FechaNacimientoCliente.ToShortDateString()}', " +
             $"'{polizaDto.FechaPoliza.ToShortDateString()}', " +
             $"'{polizaDto.CoberturasCubiertas}', " +
             $"'{polizaDto.ValorMaximoPoliza}', " +
             $"'{polizaDto.NombrePoliza}', " +
             $"'{polizaDto.NombreCiudadCliente}', " +
             $"'{polizaDto.DireccionCliente}', " +
             $"'{polizaDto.PlacaAutoCliente}', " +
             $"{(polizaDto.VehiculoCuentaInspeccion == true ? 1 : 0)}, "  +
             $"'{polizaDto.NumeroPoliza}'";
            await Context.Database.ExecuteSqlRawAsync(storedProcedure);
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            string storedProcedure = $"exec dbo.{Sp.DeletePoliza} " +
                      $"{id}";
            await Context.Database.ExecuteSqlRawAsync(storedProcedure);
            return NoContent();
        }
    }
}
