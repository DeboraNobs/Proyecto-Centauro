

using proyecto_centauro.Models;

namespace proyecto_centauro.Interfaces
{
    public interface IServicioRepositorio
    {
        Task<IEnumerable<Servicio>> ObtenerTodosServicios();
        Task<Servicio> ObtenerServicioPorId(int id);
        Task AgregarServicio(Servicio servicio);
        Task ActualizarServicio(Servicio servicio);
        Task EliminarServicio(int id);
        Task<bool> ExisteServicio(int id);
    }
}