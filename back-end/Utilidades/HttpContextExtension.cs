using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static back_end.Utilidades.Constants;

namespace back_end.Utilidades
{
    public static class HttpContextExtension
    {
        public async static Task InsertarParametroPaginacionCabecera<T>(this HttpContext httpContext, IQueryable<T> querable)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }
            double cantidad = await querable.CountAsync();
            httpContext.Response.Headers.Add(common.cantidadTotalRegistros,cantidad.ToString());
        }
    }
}
