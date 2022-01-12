using System;
using System.ComponentModel.DataAnnotations;

namespace back_end.Dto
{
    public class ActorCreacionDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength:200)]
        public string Nombre { get; set; }

        public DateTime FechaNacimiento { get; set; }
    }
}
