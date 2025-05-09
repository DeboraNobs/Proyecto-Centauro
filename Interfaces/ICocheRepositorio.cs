using proyecto_centauro.Models;
using proyecto_centauro.Models.DTO;

namespace proyecto_centauro.Interfaces
{
    public interface ICocheRepositorio
    {
        Task<IEnumerable<CocheDTO>> ObtenerTodosCoches();
        Task<IEnumerable<Coche>> ObtenerTodosCochesConRelacionGrupo();
        Task<IEnumerable<CocheDisponibilidadDTO>> ObtenerCochesFiltrados(int? sucursalId, DateTime? fechainicio, DateTime? fechaFin, TimeSpan? horarioRecogida, TimeSpan? horarioDevolucion);         
        Task<Coche> ObtenerCochePorId(int id);
      //  Task AgregarCoche(Coche coche);
        Task AgregarCoche(CocheDTO cocheDTO);
  
        Task ActualizarCoche(Coche coche);
        Task EliminarCoche(int id);
        Task<bool> ExisteCoche(int id);
    }
}