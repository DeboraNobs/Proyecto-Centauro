
using System.ComponentModel.DataAnnotations;

namespace proyecto_centauro.Requests 
{
public class SucursalModelValidation
{
    public class Search
    {
        public int? Id { get; set; } // puedo buscar sin utilizar el Id
        [MaxLength(30, ErrorMessage = "La longitud del nombre de la sucursal no puede tener más de 30 caracteres")]
        public string? Nombre { get; set; }
    }

    public class Modify
    {
        [Required]
        public required int Id { get; set; }
        [Required, MaxLength(30, ErrorMessage = "La longitud del nombre de la sucursal no puede tener más de 30 caracteres")]
        public required string? Nombre { get; set; }
    }

    public class Insert
    {        
        [Required, MaxLength(30, ErrorMessage = "La longitud del nombre de la sucursal no puede tener más de 30 caracteres")]
        public required string? Nombre { get; set; }
    }

    public class Delete
    {
        [Required]
        public required int Id { get; set; }
    }

}
}
