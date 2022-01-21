namespace back_end.Dto
{
    using back_end.Utilidades;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class PeliculaCreacionDto
    {
        [Required]
        [StringLength(maximumLength: 300)]
        public string Titulo { get; set; }

        public string Resumen { get; set; }

        public string Trailer { get; set; }

        public bool EnCines { get; set; }

        public DateTime FechaLanzamiento { get; set; }

        public IFormFile Poster { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> GenerosIds { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> CinesIds { get; set; }


        [ModelBinder(BinderType = typeof(TypeBinder<List<ActorPeliculasCreacionDto>>))]
        public List<ActorPeliculasCreacionDto> actorCreacionPeliculasDto { get; set; }

    }
}
