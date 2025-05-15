using System.ComponentModel.DataAnnotations;

namespace proyecto_centauro.Models;

public class UsuarioDTO
{
    public int? Id { get; set; }
    public string? Nombre { get; set; }
    public string? Email { get; set; }
    public string? Apellidos { get; set; }
    public string? Password { get; set; }
    public string? Movil { get; set; }
    public string? Rol { get; set; }
}