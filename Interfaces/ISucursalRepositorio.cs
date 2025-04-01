using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using proyecto_centauro.Models;

namespace proyecto_centauro.Interfaces
{
    public interface ISucursalRepositorio
    {
        Task<IEnumerable<Sucursal>> ObtenerTodas();
        Task<Sucursal> ObtenerPorId(int id);
        Task AgregarSucursal(Sucursal sucursal);
        Task ActualizarSucursal(Sucursal sucursal);
        Task EliminarSucursal(int id);
        Task<bool> ExisteSucursal(int id);
    }
}