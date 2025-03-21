
using proyecto_centauro.Models;

namespace proyecto_centauro.Interfaces
{
    public interface IAlquilerRepositorio
    {
        Task<IEnumerable<Alquiler>> ObtenerTodos();
        Task<Alquiler> ObtenerAlquilerPorId(int id);
        Task AgregarAlquiler(Alquiler alquiler);
        Task ActualizarAlquiler(Alquiler alquiler);
        Task EliminarAlquiler(int id);
        Task <bool> ExisteAlquiler(int id);
    }
}