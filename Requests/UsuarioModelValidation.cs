
using System.ComponentModel.DataAnnotations;

namespace proyecto_centauro.Requests
{
    public class UsuarioModelValidation
    {
        public class Search
        {
            public int? Id { get; set; }
            [MinLength(2)]
            public string? Nombre { get; set; }

            [EmailAddress]
            public string? Email { get; set; }
        }

        public class Modify
        {
            [Required]
            public required int Id { get; set; }
            [Required, MinLength(2)]
            public required string Nombre { get; set; }
            [Required, MinLength(2)]
            public required string Apellidos { get; set; }
            [EmailAddress]
            public string? Email { get; set; }

            [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).{8,}$",
               ErrorMessage = @"La contraseña debe tener al menos 8 caracteres, 
                            incluir al menos una mayúscula y un número.")]
            public string? Password { get; set; }
           
            public string? Movil { get; set; }

            public string? Rol { get; set; }
        }

        public class Insert
        {
            [Required, MinLength(2)]
            public required string Nombre { get; set; }
            [Required, MinLength(2)]
            public required string Apellidos { get; set; }
            [EmailAddress]
            public string? Email { get; set; }

            [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).{8,}$",
               ErrorMessage = @"La contraseña debe tener al menos 8 caracteres, 
                            incluir al menos una mayúscula y un número.")]
            public string? Password { get; set; }
           
            public string? Movil { get; set; }

            public string? Rol { get; set; }
        }
        public class Delete
        {
            [Required]
            public required int Id { get; set; }
        }

        public class Login
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

    }

}