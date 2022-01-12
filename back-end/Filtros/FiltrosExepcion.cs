using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Filtros
{
    public class FiltrosExepcion: ExceptionFilterAttribute
    {
        private readonly ILogger<FiltrosExepcion> logger;
        public FiltrosExepcion(ILogger<FiltrosExepcion> Logger)
        {
            this.logger = Logger;
        }

        public override void OnException(ExceptionContext context)
        {
            logger.LogError(context.Exception, context.Exception.Message);
            base.OnException(context);
        }
    }
}
