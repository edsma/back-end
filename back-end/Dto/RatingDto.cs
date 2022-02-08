using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Dto
{
    public class RatingDto
    {
        public int PeliculaId { get; set; }

        [Range(1,5)]
        public int Puntuacion { get; set; }
    }
}
