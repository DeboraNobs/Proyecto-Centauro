

using proyecto_centauro.Models;

namespace proyecto_centauro.Interfaces.InterfacesDapper
{
    public interface ISucursalDapper
    {
        Task<IEnumerable<Sucursal>> ObtenerTodas();
        Task<Sucursal> ObtenerPorId(int id);
        Task AgregarSucursal(Sucursal sucursal);
        Task ActualizarSucursal(Sucursal sucursal);
        Task EliminarSucursal(int id);
        Task<bool> ExisteSucursal(int id);
    }
}