using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Dto
{
    public class LadingPageDto
    {
        public List<PeliculaDto> enCines { get; set; }

        public List<PeliculaDto> proximosEstrenos { get; set; }
    }
}
