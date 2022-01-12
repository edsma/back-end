namespace back_end.Repositorios
{
    using back_end.Entidades;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class RepositorioMemoria: IRepositorio
    {
        private List<Genero> _generos;

        public RepositorioMemoria()
        {
            _generos = new List<Genero>()
            {
                new Genero(){ Id = 1, Nombre = "Drama" },
                new Genero(){Id = 2,  Nombre = "Acción" }
            };
        }

        public List<Genero> ObtenerTodosLosGeneros()
        {
            return _generos;
        }

        public async Task<Genero> ObtenerGeneroId(int id)
        {
            await Task.Delay(TimeSpan.FromSeconds(3));
            return  _generos.
                FirstOrDefault(x=> x.Id == id);
        }

        public Guid ObtenerGuid()
        {
            return Guid.NewGuid();
        }

        public void CrearGenero(Genero genero)
        {
            genero.Id = _generos.Count() + 1;
            _generos.Add(genero);
        }
    }
}
