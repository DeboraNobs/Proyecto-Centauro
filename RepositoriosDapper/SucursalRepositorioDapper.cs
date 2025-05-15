

using System.Data.SQLite;
using Dapper;
using proyecto_centauro.Interfaces;
using proyecto_centauro.Interfaces.InterfacesDapper;
using proyecto_centauro.Models;

public class SucursalRepositorioDapper : ISucursalDapper
{
    private readonly string _connectionString;

    public SucursalRepositorioDapper(IConfiguration configuration) // constructor
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") // tomo la default conection que esta en appsettings.json
            ?? throw new InvalidOperationException("Cadena de conexión 'DefaultConnection' no encontrada.");
    }

    public async Task<IEnumerable<Sucursal>> ObtenerTodas()
    {
        using var connection = new SQLiteConnection(_connectionString);
        var sql = "SELECT * FROM Sucursales";
        var sucursales = await connection.QueryAsync<Sucursal>(sql);
        return sucursales;
    }

    public async Task<Sucursal> ObtenerPorId(int id)
    {
        using var connection = new SQLiteConnection(_connectionString);
        var sql = "SELECT * FROM Sucursales WHERE Id = @Id";
        var sucursal = connection.QuerySingleAsync<Sucursal>(sql, new { Id = id }); // tambien puedo usar QueryFirst, QuerySingle devuelve una exception si hay mas de una
        return await sucursal!;
    }

    public async Task AgregarSucursal(Sucursal sucursal)
    {
        using var connection = new SQLiteConnection(_connectionString);
        var sql = @"INSERT INTO Sucursales (Nombre)
                                VALUES (@Nombre)";
        await connection.ExecuteAsync(sql, sucursal);
    }

    public async Task ActualizarSucursal(Sucursal sucursal)
    {
        using var connection = new SQLiteConnection(_connectionString);
        var sql = @"UPDATE Sucursales
                           SET Nombre = @Nombre
                           WHERE Id = @Id";
        await connection.ExecuteAsync(sql, sucursal);
    }

    public async Task EliminarSucursal(int id)
    {
        using var connection = new SQLiteConnection(_connectionString);
        var sql = "DELETE FROM Sucursales WHERE Id = @Id";
        await connection.ExecuteAsync(sql, new { Id = id });
    }
    public async Task<bool> ExisteSucursal(int id)
    {
        using var connection = new SQLiteConnection(_connectionString);
        var sql = "SELECT COUNT(*) FROM Sucursales WHERE Id = @Id";
        var count = await connection.ExecuteScalarAsync<int>(sql, new {Id = id});
        return count > 0;
    }
}