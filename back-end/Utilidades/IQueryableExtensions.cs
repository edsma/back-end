using back_end.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Utilidades
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, PaginacionDTO paginacionDto)
        {
            return queryable.Skip((paginacionDto.pagina - 1) * paginacionDto.RecordsPagina)
                .Take(paginacionDto.RecordsPagina);
        }
    }
}
