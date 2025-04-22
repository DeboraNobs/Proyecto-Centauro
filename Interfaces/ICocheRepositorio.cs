using proyecto_centauro.Models;
using proyecto_centauro.Models.DTO;

namespace proyecto_centauro.Interfaces
{
    public interface ICocheRepositorio
    {
        Task<IEnumerable<Coche>> ObtenerTodosCoches();
        Task<IEnumerable<Coche>> ObtenerTodosCochesConRelacionGrupo(); // AQUI ANTES ERA CocheDTO
        Task<IEnumerable<Coche>> ObtenerCochesFiltrados(int? sucursalId);
        Task<Coche> ObtenerCochePorId(int id);
        Task AgregarCoche(Coche coche);
        Task ActualizarCoche(Coche coche);
        Task EliminarCoche(int id);
        Task<bool> ExisteCoche(int id);
    }
}