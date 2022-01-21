namespace back_end.Entidades
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Peliculas
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(maximumLength:300)]
        public string Titulo { get; set; }

        public string Resumen { get; set; }

        public string Trailer { get; set; }

        public bool EnCines { get; set; }

        public DateTime FechaLanzamiento { get; set; }

        public string Poster { get; set; }

        public List<PeliculasActores> PeliculasActores { get; set; }

        public List<PeliculaGenero> PeliculaGenero { get; set; }

        public List<PeliculaCines> PeliculaCine { get; set; }


    }
}
