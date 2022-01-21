using System.Collections.Generic;

namespace back_end.Dto
{
    public class PeliculasPostGetDto
    {
        public List<GeneroDto>  Generos { get; set; }

        public List<CineDto> Cines { get; set; }
    }
}
