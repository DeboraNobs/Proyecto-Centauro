using proyecto_centauro.Models;

namespace proyecto_centauro.Interfaces
{
    public interface ICocheRepositorio
    {
        Task<IEnumerable<Coche>> ObtenerTodosCoches();
        Task<IEnumerable<Coche>> ObtenerTodosCochesConRelacionGrupo();
        Task<IEnumerable<Coche>> ObtenerCochesFiltrados(int? sucursalId);
        Task<Coche> ObtenerCochePorId(int id);
        Task AgregarCoche(Coche coche);
        Task ActualizarCoche(Coche coche);
        Task EliminarCoche(int id);
        Task<bool> ExisteCoche(int id);
    }
}