using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using proyecto_centauro.Interfaces;
using proyecto_centauro.Models;

namespace proyecto_centauro.Controllers
{
    [Route("api/sucursal")]
    [ApiController]
    public class SucursalController : Controller
    {
        private readonly ISucursalRepositorio _sucursalRepositorio;

        public SucursalController(ISucursalRepositorio sucursalRepositorio)
        {
            _sucursalRepositorio = sucursalRepositorio;
        }

        [HttpGet]
        public async Task<ActionResult<Sucursal>> GetSucursales()
        {
            var sucursales = await _sucursalRepositorio.ObtenerTodas();
            return Json(sucursales);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Sucursal>> GetSucursalById(int id)
        {
            var sucursal = await _sucursalRepositorio.ObtenerPorId(id);
            if (sucursal == null) return NotFound();
            return Json(sucursal);
        }

        [HttpPost]
        public async Task<ActionResult<Sucursal>> AgregarSucursal([FromBody] Sucursal sucursal)
        {
            if (sucursal == null) return BadRequest();
            await _sucursalRepositorio.AgregarSucursal(sucursal);
            return StatusCode(201, sucursal);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditarSucursal(int id, [FromBody] Sucursal sucursal)
        {
            if (id != sucursal.Id) return BadRequest();
            await _sucursalRepositorio.ActualizarSucursal(sucursal);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> BorrarSucursal(int id)
        {
            var existeSucursal = await _sucursalRepositorio.ExisteSucursal(id);
            if (!existeSucursal) return NotFound();

            await _sucursalRepositorio.EliminarSucursal(id);
            return NoContent(); 
        }

    }
}