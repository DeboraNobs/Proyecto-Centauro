using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proyecto_centauro.Models
{
    [Table("Users")]
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(2)]
        public string? Nombre { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Apellidos { get; set; }
        [Required]
        [Phone]
        public string? Movil { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).{8,}$",
           ErrorMessage = "La contraseña debe tener al menos 8 caracteres, incluir al menos una mayúscula y un número.")]
        public string? Password { get; set; }
        [Required]
        public string? Rol { get; set; }

        // propiedad de navegación inversa: para facilitar la navegación desde el usuario al alquiler
        public List<Alquiler> Alquileres { get; set; } = []; // crea una nueva lista de alquileres vacía 
        // en mi caso CREO no es necesario utilizarla porque no tengo más de una relación entre los mismos tipos. 
    }
}
