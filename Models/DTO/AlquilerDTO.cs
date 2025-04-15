
namespace proyecto_centauro.Models;

public class AlquilerDTO {
    public int Id { get; set; }
    public DateTime Fechainicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string LugarRecogida { get; set; } = string.Empty;
    public string LugarDevolucion { get; set; } = string.Empty;
    public TimeSpan HorarioRecogida {get; set; }
    public TimeSpan HorarioDevolucion {get; set; }
    public int? UsersId { get; set; }
    public int? GrupoId { get; set; }
}