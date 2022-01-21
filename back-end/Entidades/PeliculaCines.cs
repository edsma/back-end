namespace back_end.Entidades
{
    public class PeliculaCines
    {
        public int PeliculaId { get; set; }

        public int CineId { get; set; }

        public Peliculas Pelicula { get; set; }

        public Cine Cine { get; set; }
    }
}
