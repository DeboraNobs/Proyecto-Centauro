using proyecto_centauro.Models;
using proyecto_centauro.Models.DTO;

namespace proyecto_centauro.Interfaces
{
    public interface IGrupoRepositorio
    {
        // Task<IEnumerable<Grupo>> ObtenerTodos();
        Task<IEnumerable<GrupoDTO>> ObtenerTodos();
        Task<Grupo> ObtenerGrupoPorId(int id);
        Task AgregarGrupo(Grupo grupo);
        Task ActualizarGrupo(Grupo grupo);
        Task EliminarGrupo(int id);
        Task <bool> ExisteGrupo(int id);
    }
}