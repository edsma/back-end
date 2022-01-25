using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Dto
{
    public class PeliculasPutGetDto
    {
        public PeliculaDto Pelicula{ get; set; }

        public List<GeneroDto> GenerosNoSeleccionados { get; set; }

        public List<GeneroDto> GenerosSeleccionados { get; set; }

        public List<CineDto> CinesSeleccionados { get; set; }

        public List<CineDto> CinesNoSeleccionados { get; set; }

        public List<PeliculaActorDto> Actores { get; set; }


    }
}
