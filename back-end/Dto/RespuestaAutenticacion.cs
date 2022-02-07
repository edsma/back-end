using System;

namespace back_end.Dto
{
    public class RespuestaAutenticacion
    {
        public string Token { get; set; }

        public DateTime  Expiracion{ get; set; }
    }
}
