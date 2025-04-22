
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace proyecto_centauro.Models
{
    [Table("Grupos")]
    public class Grupo
    {
     [Key]
     public int Id { get; set;}
     [Required]
     [MinLength(2)]
     public string? Nombre { get; set;}
     [Required]
     [MaxLength(200)]
     public string? Descripcion { get; set;}

     [Required]
     public int Precio { get; set;}

     // propiedad de navegación inversa: para facilitar la navegación desde el grupo al coche, y desde el grupo al alquiler
     public List<Coche>? Coches { get; set; } = [];
     public List<Alquiler>? Alquileres { get; set; } = [];

    }
}