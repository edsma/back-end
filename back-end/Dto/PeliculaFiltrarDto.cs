using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Dto
{
    public class PeliculaFiltrarDto
    {
        public int Pagina { get; set; }

        public int RecordsPorPagina { get; set; }

        public PaginacionDTO PaginacionDto
        {
            get
            {
                return new PaginacionDTO()
                {
                    pagina = Pagina,
                    recordsPagina = RecordsPorPagina
                };
            }
        }

        public string  Titulo { get; set; }

        public int GeneroId { get; set; }

        public bool enCines { get; set; }

        public bool proximosEstrenos { get; set; }
    }
}
