using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Dto
{
    public class PeliculaDto
    {
        public int Id { get; set; }

        public string Titulo { get; set; }

        public string Resumen { get; set; }

        public string Trailer { get; set; }

        public bool EnCines { get; set; }

        public DateTime FechaLanzamiento { get; set; }

        public string Poster { get; set; }

        public List<GeneroDto> Generos { get; set; }

        public List<PeliculaActorDto> Actor { get; set; }

        public List<CineDto> Cines { get; set; }


    }
}
