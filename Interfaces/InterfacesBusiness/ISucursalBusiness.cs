
using System.Data.Common;
using proyecto_centauro.Models;
using proyecto_centauro.Models.DTO;
using proyecto_centauro.Requests;

namespace proyecto_centauro.Interfaces.InterfacesBusiness
{
    public interface ISucursalBusiness 
    {
        Task<List<SucursalDTO>> Search(SucursalModelValidation.Search search);
        Task<SucursalDTO> Insert(SucursalModelValidation.Insert insert);
        Task<bool> Delete(SucursalModelValidation.Delete delete);
        Task<SucursalDTO> Modify(SucursalModelValidation.Modify modify);
    }
}