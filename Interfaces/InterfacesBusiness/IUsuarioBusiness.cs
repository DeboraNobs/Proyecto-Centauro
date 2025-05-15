

using proyecto_centauro.Models;
using proyecto_centauro.Requests;

namespace proyecto_centauro.Interfaces.InterfacesBusiness
{
    public interface IUsuarioBusiness
    {
        Task<List<UsuarioDTO>> Search(UsuarioModelValidation.Search search);
        Task<UsuarioDTO> Insert(UsuarioModelValidation.Insert insert);
        Task<bool> Delete(UsuarioModelValidation.Delete delete);
        Task<UsuarioDTO> Modify(UsuarioModelValidation.Modify modify);
        Task<UsuarioDTO?> ValidarCredenciales(string email, string password);
        Task<UsuarioDTO> ObtenerPorId(int id);
    }
}