using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyecto_centauro.Data;
using proyecto_centauro.Interfaces;
using proyecto_centauro.Models;
using proyecto_centauro.Models.DTO;
using proyecto_centauro.Requests;

namespace proyecto_centauro.Repositorios
{
    public class SucursalRepositorio : ISucursalRepositorio
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;

        public SucursalRepositorio(IConfiguration configuration, IMapper mapper) // constructor
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") // tomo la default conection que esta en appsettings.json
                ?? throw new InvalidOperationException("Cadena de conexión 'DefaultConnection' no encontrada.");
            this._mapper = mapper;
        }
        public async Task<bool> Delete(SucursalModelValidation.Delete delete, DbTransaction? transaction = null)
        {
            using var connection = new SQLiteConnection(_connectionString);
            var sql = "DELETE FROM Sucursales WHERE Id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, new { delete.Id });

            return rowsAffected > 0;
        }


        public async Task<bool> ExisteSucursal(int id)
        {
            try
            {
                using var connection = new SQLiteConnection(_connectionString);
                var sql = "SELECT COUNT(*) FROM Sucursales WHERE Id = @Id";
                var count = await connection.QuerySingleAsync<int>(sql, new { Id = id });
                return count > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error " + ex + " al buscar si existe la sucursal con id: " + id);
            }
        }


        public async Task<bool> ExisteSucursalPorNombre(string nombre)
        {
            try
            {
                using var connection = new SQLiteConnection(_connectionString);
                var sql = "SELECT COUNT(*) FROM Sucursales WHERE Nombre = @Nombre";
                var count = await connection.QuerySingleAsync<int>(sql, new { Nombre = nombre });
                return count > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error " + ex + " al buscar si existe la sucursal con nombre: " + nombre);
            }
        }


        public async Task<SucursalDTO> Insert(SucursalModelValidation.Insert insert, DbTransaction? transaction = null)
        {
            try
            {
                using var connection = new SQLiteConnection(_connectionString);
                await connection.OpenAsync();
                var sql = @"INSERT INTO Sucursales (Nombre) 
                                VALUES (@Nombre);";

                var parameters = new DynamicParameters();
                parameters.Add("@Nombre", insert.Nombre!, DbType.String);

                var rowsAffected = await connection.ExecuteAsync(sql, parameters, transaction);

                if (rowsAffected == 0)
                {
                    throw new Exception("No se pudo insertar la sucursal.");
                }

                var lastInsertIdSql = "SELECT LAST_INSERT_ROWID();";  // Recuperamos el último ID insertado 
                var lastInsertId = await connection.QuerySingleAsync<int>(lastInsertIdSql);

                var selectSql = "SELECT Id, Nombre FROM Sucursales WHERE Id = @Id;";
                var insertedSucursal = await connection.QuerySingleOrDefaultAsync<Sucursal>(selectSql, new { Id = lastInsertId }) ?? throw new Exception("No se pudo recuperar la sucursal recién insertada.");

                var sucursalDTO = _mapper.Map<SucursalDTO>(insertedSucursal);

                return sucursalDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar una sucursal: " + ex.Message, ex);
            }
        }

        public async Task<SucursalDTO> Modify(SucursalModelValidation.Modify modify, DbTransaction? transaction = null)
        {
            try
            {
                using var connection = new SQLiteConnection(_connectionString);
                await connection.OpenAsync();

                var updateSql = @"UPDATE Sucursales
                                    SET Nombre = @Nombre
                                    WHERE Id = @Id";

                var parameters = new DynamicParameters();
                parameters.Add("@Id", modify.Id, DbType.Int32);
                parameters.Add("@Nombre", modify.Nombre!, DbType.String);

                await connection.ExecuteAsync(updateSql, parameters, transaction);

                var selectSql = @"SELECT Id, Nombre FROM Sucursales WHERE Id = @Id";
                var updatedSucursal = await connection.QuerySingleAsync<Sucursal>(selectSql, new { Id = modify.Id });

                var dto = _mapper.Map<SucursalDTO>(updatedSucursal);
                return dto;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar una sucursal: " + ex.Message, ex);
            }
        }



        public async Task<SucursalDTO> ObtenerPorId(int id)
        {
            try
            {
                using var connection = new SQLiteConnection(_connectionString);
                var sql = "SELECT * FROM Sucursales WHERE Id = @Id";
                var sucursal = connection.QuerySingleAsync<SucursalDTO>(sql, new { Id = id }); // tambien puedo usar QueryFirst, QuerySingle devuelve una exception si hay mas de una
                return await sucursal!;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex + " obtener sucursal con id: " + id);
            }
        }

        public async Task<List<Sucursal>> Search(SucursalModelValidation.Search search) // método que busca todas las sucursales que contengan los parametros introducidos
        {
            try
            {
                using var connection = new SQLiteConnection(_connectionString);

                var sql = "SELECT * FROM Sucursales WHERE 1=1";

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

                var sucursales = await connection.QueryAsync<Sucursal>(sql, parameters);

                return sucursales.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las sucursales: " + ex.Message);
            }


        }
    }
}


