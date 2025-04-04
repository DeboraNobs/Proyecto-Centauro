using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace proyecto_centauro.Models;

public class Coche
 {
     [Key]
     public int Id { get; set;}
     [Required]
     [MinLength(2)]
     public string? Marca { get; set;}
     [Required]
     public string? Modelo { get; set;}
     [Required]
     public string? Descripcion { get; set;}
     [Required]
     public string? Patente { get; set;}
     [Required]
     public string? Tipo_coche { get; set;}
     [Required]
     public string? Tipo_cambio { get; set;}
     [Required]
     public int Num_plazas { get; set;}
     [Required]
     public int Num_maletas { get; set;}
     [Required]
     public int Num_puertas { get; set;}
     [Required]
     public bool Posee_aire_acondicionado { get; set;}

     public string? Imagen { get; set;}
     public int? GrupoId { get; set;}

     public int? SucursalId { get; set;}
     public Grupo? Grupo { get; set;} // Propiedad de navegación

     public Sucursal? Sucursal { get; set;}
 }