namespace back_end.Dto
{
    using back_end.Validaciones;
    using System.ComponentModel.DataAnnotations;
    public class GeneroCreacionDto
    {
        [Required(ErrorMessage = "el campo {0} es requerido")]
        [StringLength(maximumLength: 10)]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
    }
}
