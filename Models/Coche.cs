using System.ComponentModel.DataAnnotations;

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
     public string? Tipo_cambio { get; set;}
     [Required]
     public int Num_plazas { get; set;}
 }