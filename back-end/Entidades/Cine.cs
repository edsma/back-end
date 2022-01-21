using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Entidades
{
    public class Cine
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength:75)]
        public string Nombre { get; set; }

        public int MyProperty { get; set; }

        public Point Ubicacion { get; set; }

        public List<PeliculaCines> PeliculasCine { get; set; }
    }
}
