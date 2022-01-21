namespace back_end.Dto
{
    using System;

    public class ActorDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public string Foto { get; set; }

        public string Biografia { get; set; }
    }
}
