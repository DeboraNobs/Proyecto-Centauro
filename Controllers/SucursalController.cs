using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using proyecto_centauro.Interfaces;
using proyecto_centauro.Interfaces.InterfacesBusiness;
using proyecto_centauro.Models;
using proyecto_centauro.Models.DTO;
using proyecto_centauro.Requests;

namespace proyecto_centauro.Controllers
{
    [Route("api/sucursal")]
    [ApiController]
    public class SucursalController(ISucursalBusiness business) : Controller
    {

        [HttpGet] 
        public async Task<ActionResult<List<SucursalDTO>>> GetSucursales([FromQuery] SucursalModelValidation.Search request)
        {
            return await business.Search(request);
        }


/*
        [HttpGet("{id}")]
        public async Task<ActionResult<Sucursal>> GetSucursalById(int id)
        {
            var sucursal = await _sucursalRepositorio.ObtenerPorId(id);
            if (sucursal == null) return NotFound();
            return Json(sucursal);
        }
*/

        [HttpPost]
        public async Task<ActionResult<SucursalDTO>> AgregarSucursal([FromForm] SucursalModelValidation.Insert request)
        {
            return await business.Insert(request);
        }

        [HttpPut("{id}")] 
        public async Task<ActionResult<SucursalDTO>> EditarSucursal(int id, [FromForm] SucursalModelValidation.Modify request)
        {
            request.Id = id;
            return await business.Modify(request);
        }

        [HttpDelete("{id}")] 
        public async Task<ActionResult<bool>> BorrarSucursal(int id, [FromForm] SucursalModelValidation.Delete request)
        {
            request.Id = id;
           return await business.Delete(request);
        }

    }
}