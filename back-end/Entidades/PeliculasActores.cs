using System.ComponentModel.DataAnnotations;

namespace back_end.Entidades
{
    public class PeliculasActores
    {
        public int PeliculaId { get; set; }

        public int ActorId { get; set; }

        public Peliculas pelicula{ get; set; }

        public Actor actor { get; set; }

        [StringLength(maximumLength:10)]
        public string Personaje { get; set; }

        public int Orden { get; set; }
    }
}
