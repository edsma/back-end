using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace back_end.Dto
{
    public class ActorCreacionDto
    {

        [Required]
        [StringLength(maximumLength:200)]
        public string Nombre { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public IFormFile Foto { get; set; }

        public string Biografia { get; set; }
    }
}
