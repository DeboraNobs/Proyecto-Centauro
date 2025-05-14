using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using proyecto_centauro.Models;
using proyecto_centauro.Models.DTO;
using proyecto_centauro.Requests;

namespace proyecto_centauro.Interfaces.InterfacesValidation
{
    public interface ISucursalRepositorioV // aqui uso DbTransaction porque este repositorio interactúa con la BBDDD
    {
        Task<List<Sucursal>> Search(SucursalModelValidation.Search search);
        Task<SucursalDTO> Insert(SucursalModelValidation.Insert insert, DbTransaction? transaction = null);
        Task<bool> Delete(SucursalModelValidation.Delete delete, DbTransaction? transaction = null);
        Task<SucursalDTO> Modify(SucursalModelValidation.Modify modify, DbTransaction? transaction = null);
        Task<SucursalDTO> ObtenerPorId(int id);
        Task<bool> ExisteSucursal(int id);
        Task <bool> ExisteSucursalPorNombre(string nombre);
    }
}