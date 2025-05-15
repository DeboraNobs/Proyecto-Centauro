
using System.Data;
using System.Data.SQLite;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using proyecto_centauro.Data;
using proyecto_centauro.Interfaces;
using proyecto_centauro.Interfaces.InterfacesDapper;
using proyecto_centauro.Models;

// Los repositorios de Dapper no usan BBDDContext. Aquí Dapper trabaja directamente con la base de datos por su cuenta.
public class UsuarioRepositorioDapper : IUsuarioDapper
{
    private readonly string _connectionString;

    public UsuarioRepositorioDapper(IConfiguration configuration) // constructor
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") // tomo la default conection que esta en appsettings.json
            ?? throw new InvalidOperationException("Cadena de conexión 'DefaultConnection' no encontrada.");
    }

    public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
    {
        using var connection = new SQLiteConnection(_connectionString);
        var sql = "SELECT * FROM Users";
        var usuarios = await connection.QueryAsync<Usuario>(sql);
        return usuarios;
    }

    public async Task<Usuario> ObtenerPorIdAsync(int id)
    {
        using var connection = new SQLiteConnection(_connectionString);
        var sql = "SELECT * FROM Users WHERE Id = @Id";
        var usuario = connection.QuerySingleAsync<Usuario>(sql, new { Id = id });
        return await usuario!;
    }

    public async Task<Usuario?> ValidarCredencialesAsync(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            throw new ArgumentException("El email y la contraseña son obligatorios");

        using var connection = new SQLiteConnection(_connectionString);
        var sql = "SELECT * FROM Users WHERE Email = @Email";
        var usuario = await connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { Email = email });

        if (usuario == null || string.IsNullOrEmpty(usuario.Password))
            return null;

        var passwordHasher = new PasswordHasher<Usuario>();
        var resultado = passwordHasher.VerifyHashedPassword(usuario, usuario.Password, password);

        return resultado == PasswordVerificationResult.Success ? usuario : null;
    }

    public async Task AgregarAsync(Usuario usuario) // se hashea la contraseña en el constructor
    {
        using var connection = new SQLiteConnection(_connectionString);
        // el arroba sirve para que no haya que escapar un espacio cuando se hace un query con saltos de línea 
        var sql = @"INSERT INTO Users (Nombre, Email, Apellidos, Movil, Password, Rol)
                    VALUES (@Nombre, @Email, @Apellidos, @Movil, @Password, @Rol)";
        await connection.ExecuteAsync(sql, usuario);
    }

    public async Task ActualizarAsync(Usuario usuario)
    {
        using var connection = new SQLiteConnection(_connectionString);
        var sql = @"UPDATE Users 
                        SET Nombre = @Nombre, 
                            Email = @Email, 
                            Apellidos = @Apellidos, 
                            Movil = @Movil, 
                            Password = @Password, 
                            Rol = @Rol 
                        WHERE Id = @Id";
        await connection.ExecuteAsync(sql, usuario);
    }

    public async Task EliminarAsync(int id)
    {
        using var connection = new SQLiteConnection(_connectionString);
        var sql = "DELETE FROM Users WHERE Id = @Id";
        await connection.ExecuteAsync(sql, new { Id = id });
    }

    public async Task<bool> ExisteUsuarioAsync(int id)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            var sql = "SELECT COUNT(*) FROM Users WHERE Id = @Id";
            var count = await connection.ExecuteScalarAsync<int>(sql, new { Id = id });
            return count > 0;
        }
    }

}