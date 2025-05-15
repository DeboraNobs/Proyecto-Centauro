using System.Data.Common;
using proyecto_centauro.Models;
using proyecto_centauro.Models.DTO;
using proyecto_centauro.Requests;
namespace proyecto_centauro.Interfaces
{
    public interface IUsuarioRepositorio
    {
        Task<List<Usuario>> Search(UsuarioModelValidation.Search search);
        Task<UsuarioDTO> Insert(UsuarioModelValidation.Insert insert, DbTransaction? transaction = null);
        Task<bool> Delete(UsuarioModelValidation.Delete delete, DbTransaction? transaction = null);
        Task<UsuarioDTO> Modify(UsuarioModelValidation.Modify modify, DbTransaction? transaction = null);
        Task<bool> ExisteUsuario(int id);
        Task<bool> ExisteUsuarioPorNombre(string nombre);
        Task<Usuario?> ValidarCredencialesAsync(string email, string password);
        Task<UsuarioDTO> ObtenerPorId(int id);
    }
}
