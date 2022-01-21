using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Utilidades
{
    public class AlmacenadorArchivosLocal : IAlmanecenadorArchivos
    {
        public IWebHostEnvironment Env { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }

        public AlmacenadorArchivosLocal(IWebHostEnvironment env,IHttpContextAccessor httpContextAccessor)
        {
            Env = env;
            HttpContextAccessor = httpContextAccessor;
        }

        public Task BorrarArchivo(string ruta, string contenedor)
        {
            if (string.IsNullOrEmpty(ruta))
            {
                string nombreArchivo = Path.GetFileName(ruta);
                string directorioArchivo = Path.Combine(Env.WebRootPath, contenedor, nombreArchivo);
                if (File.Exists(directorioArchivo))
                {
                    File.Delete(directorioArchivo);
                }
            }
            return Task.CompletedTask;
        }

        public async Task<string> EditarArchivo(string contenedor, IFormFile archivo, string ruta)
        {
            await BorrarArchivo(ruta, contenedor);
            return await GuardarArchivo(contenedor, archivo);
        }

        public async Task<string> GuardarArchivo(string contenedor, IFormFile archivo)
        {
            string extension = Path.GetExtension(archivo.FileName);
            string nombreArchivo = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(Env.WebRootPath, contenedor);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string ruta = Path.Combine(folder, nombreArchivo);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await archivo.CopyToAsync(memoryStream);
                var contenido = memoryStream.ToArray();
                await File.WriteAllBytesAsync(ruta,contenido);
            }

            string urlActual = $"{HttpContextAccessor.HttpContext.Request.Scheme}://{HttpContextAccessor.HttpContext.Request.Host}";
            string rutaDb = Path.Combine(urlActual, contenedor, nombreArchivo).Replace("\\", "/");
            return rutaDb.Trim();
        }
    }
}
