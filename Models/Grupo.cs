
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

     // representa relación M:M con Alquiler 
     [JsonIgnore] // evita que aparezca en Swagger
    public List<GrupoAlquiler> GrupoAlquileres { get; set; } = [];

    // propiedad de navegación inversa: para facilitar la navegación desde el grupo al coche
    public List<Coche>? Coches { get; set; } = [];
    }
}