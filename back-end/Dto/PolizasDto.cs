using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Dto
{
    public class PolizasDto
    {
        public int Id { get; set; }

        public string NombreCliente { get; set; }

        public string IdentificacionCliente { get; set; }

        public DateTime FechaNacimientoCliente { get; set; }

        public DateTime FechaPoliza { get; set; }

        public string CoberturasCubiertas { get; set; }

        public decimal ValorMaximoPoliza { get; set; }

        public string NombrePoliza { get; set; }

        public string NombreCiudadCliente { get; set; }

        public string DireccionCliente { get; set; }

        public string PlacaAutoCliente { get; set; }

        public bool VehiculoCuentaInspeccion { get; set; }

        public string NumeroPoliza { get; set; }
    }
}
