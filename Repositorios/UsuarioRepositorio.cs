using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using proyecto_centauro.Data;
using proyecto_centauro.Interfaces;
using proyecto_centauro.Models;

namespace proyecto_centauro.Repositorios
{
    // La clase UsuarioRepositorio se encargará de interactuar con la BBDD 
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly BBDDContext _context; // se injecta DbContext mediante una variable de tipo UsuarioContext (se conecta con la BBDD)
        public UsuarioRepositorio(BBDDContext contexto) // constructor 
        { 
            _context = contexto;
        }

        public async Task<IEnumerable<Usuario>> ObtenerTodosAsync() // Task<> significa que la función devuelve una tarea asíncrona.
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<Usuario> ObtenerPorIdAsync(int id)
        {
            var usuario = await _context.Users.FindAsync(id);
            if (usuario == null) throw new KeyNotFoundException($"No se encontró el usuario con ID {id}");
            return usuario;
        }

        public async Task<Usuario?> ValidarCredencialesAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                throw new ArgumentException("El email y la contraseña son obligatorios");

            var usuario = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            
            if (usuario == null || string.IsNullOrEmpty(usuario.Password))
                return null;

            var passwordHasher = new PasswordHasher<Usuario>();
            var resultado = passwordHasher.VerifyHashedPassword(usuario, usuario.Password, password);

            return resultado == PasswordVerificationResult.Success ? usuario : null;
        }

        public async Task AgregarAsync(Usuario usuario)
        {
            _context.Users.Add(usuario); // agrega usuario a la BBDD
            await _context.SaveChangesAsync(); // como save()

        }
        public async Task ActualizarAsync(Usuario usuario)
        {
            _context.Entry(usuario).State = EntityState.Modified; // obtiene el usuario en el DbContext que va a modificarse
                                    // luego accede al estado de dicho usuario y le avisa a la BBDD que la entidad fue modificada, que debe actualizarse.
            await _context.SaveChangesAsync();
        }
        public async Task EliminarAsync(int id)
        {
            var usuario = await _context.Users.FindAsync(id);
            if (usuario != null) {
                _context.Users.Remove(usuario);
                await _context.SaveChangesAsync();
            } 
        }
        public async Task<bool> ExisteUsuarioAsync(int id)
        {
            return await _context.Users.AnyAsync(e => e.Id == id); // verifica si existe al menos un registro con dicho id  
        }

    }

}