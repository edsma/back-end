namespace back_end.Entidades
{
    public class PeliculaGenero
    {
        public int PeliculaId { get; set; }

        public int GeneroId { get; set; }

        public Peliculas pelicula { get; set; }

        public Genero  genero { get; set; }
    }
}
