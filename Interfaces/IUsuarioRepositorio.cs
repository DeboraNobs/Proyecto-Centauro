using proyecto_centauro.Models;
namespace proyecto_centauro.Interfaces
{
      public interface IUsuarioRepositorio
    {
        Task<IEnumerable<Usuario>> ObtenerTodosAsync(); // task<> significa que la función devuelve una tarea asíncrona.
        Task<Usuario> ObtenerPorIdAsync(int id);
        Task AgregarAsync(Usuario usuario); // no llevan ningún <> porque son void, no devuelven nada
        Task ActualizarAsync(Usuario usuario);
        Task EliminarAsync(int id);
        Task<bool> ExisteUsuarioAsync(int id);
    }
}
