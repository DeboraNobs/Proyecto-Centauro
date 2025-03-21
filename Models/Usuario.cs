using System.ComponentModel.DataAnnotations;

namespace proyecto_centauro.Models;
public class Usuario
 {
     [Key]
     public int Id { get; set;}
     [Required]
     [MinLength(2)]
     public string? Nombre { get; set;}
     [Required]
     [EmailAddress]
     public string? Email { get; set;}
     [Required]
     public string? Apellidos { get; set;} 
     [Required]
     public string? Password { get; set;}
     [Required]
     public string? Rol { get; set;} 
 }