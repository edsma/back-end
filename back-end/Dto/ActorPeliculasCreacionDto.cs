using back_end.Utilidades;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Dto
{
    public class ActorPeliculasCreacionDto
    {
        public int Id { get; set; }

        public string Personaje { get; set; }

    }
}
