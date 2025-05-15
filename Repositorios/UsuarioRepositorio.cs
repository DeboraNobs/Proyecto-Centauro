using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using proyecto_centauro.Data;
using proyecto_centauro.Interfaces;
using proyecto_centauro.Models;
using proyecto_centauro.Models.DTO;
using proyecto_centauro.Requests;

namespace proyecto_centauro.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;

        public UsuarioRepositorio(IConfiguration configuration, IMapper mapper) // constructor
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") // tomo la default conection que esta en appsettings.json
                ?? throw new InvalidOperationException("Cadena de conexión 'DefaultConnection' no encontrada.");
            this._mapper = mapper;
        }
        public async Task<bool> Delete(UsuarioModelValidation.Delete delete, DbTransaction? transaction = null)
        {
            using var connection = new SQLiteConnection(_connectionString);
            var sql = "DELETE FROM Users WHERE Id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, new { delete.Id });

            return rowsAffected > 0;
        }

        public async Task<bool> ExisteUsuario(int id)
        {
            try
            {
                using var connection = new SQLiteConnection(_connectionString);
                var sql = "SELECT COUNT(*) FROM Users WHERE Id = @Id";
                var count = await connection.QuerySingleAsync<int>(sql, new { Id = id });
                return count > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error " + ex + " al buscar si existe el usuario con id: " + id);
            }
        }

        public async Task<bool> ExisteUsuarioPorNombre(string nombre)
        {
            try
            {
                using var connection = new SQLiteConnection(_connectionString);
                var sql = "SELECT COUNT(*) FROM Users WHERE Nombre = @Nombre";
                var count = await connection.QuerySingleAsync<int>(sql, new { Nombre = nombre });
                return count > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error " + ex + " al buscar si existe un usuario con nombre: " + nombre);
            }
        }

        public async Task<UsuarioDTO> Insert(UsuarioModelValidation.Insert insert, DbTransaction? transaction = null)
        {
            try
            {
                using var connection = new SQLiteConnection(_connectionString);
                await connection.OpenAsync();
                var sql = @"INSERT INTO Users (Nombre, Email, Password, Apellidos, Rol, Movil) 
                                VALUES (@Nombre, @Email, @Password, @Apellidos, @Rol, @Movil);";

                var parameters = new DynamicParameters();
                parameters.Add("@Nombre", insert.Nombre!, DbType.String);
                parameters.Add("@Email", insert.Email!, DbType.String);
                parameters.Add("@Password", insert.Password!, DbType.String);
                parameters.Add("@Apellidos", insert.Apellidos!, DbType.String);
                parameters.Add("@Rol", insert.Rol!, DbType.String);
                parameters.Add("@Movil", insert.Movil!, DbType.String);

                var rowsAffected = await connection.ExecuteAsync(sql, parameters, transaction);

                if (rowsAffected == 0) throw new Exception("No se pudo insertar el usuario.");

                var lastInsertIdSql = "SELECT LAST_INSERT_ROWID();";  // Recuperamos el último ID insertado 
                var lastInsertId = await connection.QuerySingleAsync<int>(lastInsertIdSql);

                var selectSql = "SELECT Id, Nombre, Password FROM Users WHERE Id = @Id;";
                var insertedUsuario = await connection.QuerySingleOrDefaultAsync<Usuario>(selectSql, new { Id = lastInsertId }) ?? throw new Exception("No se pudo recuperar el usuario recién insertado.");

                var usuarioDTO = _mapper.Map<UsuarioDTO>(insertedUsuario);

                return usuarioDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar un usuario: " + ex.Message, ex);
            }
        }

        public async Task<UsuarioDTO> Modify(UsuarioModelValidation.Modify modify, DbTransaction? transaction = null)
        {
            try
            {
                using var connection = new SQLiteConnection(_connectionString);
                await connection.OpenAsync();
 
                var updateSql = @"UPDATE Users
                                    SET Nombre = @Nombre, Email = @Email, Password = @Password, Apellidos = @Apellidos, Rol = @Rol, Movil = @Movil 
                                    WHERE Id = @Id";

                var parameters = new DynamicParameters();
                parameters.Add("@Id", modify.Id, DbType.Int32);
                parameters.Add("@Nombre", modify.Nombre!, DbType.String);
                parameters.Add("@Email", modify.Email!, DbType.String);
                parameters.Add("@Password", modify.Password!, DbType.String);
                parameters.Add("@Apellidos", modify.Apellidos!, DbType.String);
                parameters.Add("@Rol", modify.Rol!, DbType.String);
                parameters.Add("@Movil", modify.Movil!, DbType.String);
                await connection.ExecuteAsync(updateSql, parameters, transaction);

                var selectSql = @"SELECT Id, Nombre, Email, Password, Apellidos, Rol, Movil FROM Users WHERE Id = @Id";
                var updatedUsuario = await connection.QuerySingleAsync<Usuario>(selectSql, new { Id = modify.Id });

                var dto = _mapper.Map<UsuarioDTO>(updatedUsuario);
                return dto;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar un usuario: " + ex.Message, ex);
            }
        }

        public async Task<List<Usuario>> Search(UsuarioModelValidation.Search search)
        {
            try
            {
                using var connection = new SQLiteConnection(_connectionString);

                var sql = "SELECT * FROM Users WHERE 1=1";

                var parameters = new DynamicParameters();

                if (search.Id != null)
                {
                    sql += " AND Id = @Id";
                    parameters.Add("@Id", search.Id, DbType.Int32);
                }

                if (!string.IsNullOrEmpty(search.Nombre))
                {
                    sql += " AND Nombre LIKE @Nombre";
                    parameters.Add("@Nombre", "%" + search.Nombre + "%", DbType.String);
                }

                if (!string.IsNullOrEmpty(search.Email))
                {
                    sql += " AND Email LIKE @Email";
                    parameters.Add("@Email", "%" + search.Email + "%", DbType.String);
                }

                var usuarios = await connection.QueryAsync<Usuario>(sql, parameters);

                return usuarios.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los usuarios: " + ex.Message);
            }
        }

        public async Task<Usuario?> ValidarCredencialesAsync(string email, string password)
        {
            using var connection = new SQLiteConnection(_connectionString);

            var sql = "SELECT * FROM Users WHERE Email = @Email";

            var usuario = await connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { Email = email });

            if (usuario == null || string.IsNullOrEmpty(usuario.Password))
                return null;

            var passwordHasher = new PasswordHasher<Usuario>();
            var resultado = passwordHasher.VerifyHashedPassword(usuario, usuario.Password, password);

            return resultado == PasswordVerificationResult.Success ? usuario : null;
        }

        
        public async Task<UsuarioDTO> ObtenerPorId(int id)
        {
            try
            {
                using var connection = new SQLiteConnection(_connectionString);
                var sql = "SELECT * FROM Users WHERE Id = @Id";
                var usuario = connection.QuerySingleAsync<UsuarioDTO>(sql, new { Id = id }); // tambien puedo usar QueryFirst, QuerySingle devuelve una exception si hay mas de una
                return await usuario!;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex + " obtener usuario con id: " + id);
            }
        }

    }

}