using System.ComponentModel.DataAnnotations;

namespace proyecto_centauro.Models;

public class UsuarioDTO {
    public string? Nombre { get; set;}
    public string? Email { get; set;}
    public string? Password { get; set;}
}