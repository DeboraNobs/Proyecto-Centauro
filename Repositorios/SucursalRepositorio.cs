using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyecto_centauro.Data;
using proyecto_centauro.Interfaces;
using proyecto_centauro.Models;

namespace proyecto_centauro.Repositorios
{
    public class SucursalRepositorio : ISucursalRepositorio
    {
        private readonly BBDDContext _context;
        
        public SucursalRepositorio(BBDDContext contexto)
        {
            _context = contexto;
        }

        public async Task<IEnumerable<Sucursal>> ObtenerTodas() 
        {
            return await _context.Sucursales.ToListAsync();
        }

        public async Task<Sucursal> ObtenerPorId(int id)
        {
            var sucursal = await _context.Sucursales.FindAsync(id);
            if (sucursal == null) throw new KeyNotFoundException($"No se ha encontrado una sucursal con id: {id}");
            return sucursal;
        }

        public async Task AgregarSucursal(Sucursal sucursal) 
        {
            _context.Sucursales.Add(sucursal);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarSucursal(Sucursal sucursal)
        {
            _context.Entry(sucursal).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task EliminarSucursal(int id)
        {
            var sucursal = await _context.Sucursales.FindAsync(id);
            if (sucursal != null) {
                _context.Sucursales.Remove(sucursal);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExisteSucursal(int id)
        {
            return await _context.Sucursales.AnyAsync(h => h.Id == id);
        }
    }
}


