using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Utilidades
{
    public class Constants
    {
        public struct common
        {
            public const string cantidadTotalRegistros = "cantidadTotalRegistros";
            public const string contenedor = "Actores";
            public const string contenedorPeliculas = "Peliculas";
        }

        public struct messages
        {
            public const string valorNoAdecuado = "El valor dado no es del tipo adecuado";
        }

        public struct Sp
        {
            public const string GetPolizas = "GetPolizas";
            public const string ActualizarPoliza = "ActualizarPoliza";
            public const string ConsultarPoliza = "ConsultarPoliza";
            public const string DeletePoliza = "DeletePoliza";

            
        }
    }
}
