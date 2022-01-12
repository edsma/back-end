using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Dto
{
    public class PaginacionDTO
    {
        public int pagina { get; set; } = 1;

        public int recordsPagina { get; set; } = 10;

        private readonly int cantidadMaximaRecordsPagina = 50;

        public int RecordsPagina
        {
            get
            {
                return recordsPagina;
            }

            set
            {
                recordsPagina = (value > cantidadMaximaRecordsPagina) ? cantidadMaximaRecordsPagina : value; 
            }
        }
    }
}
