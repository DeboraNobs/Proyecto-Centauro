using Microsoft.AspNetCore.Mvc;
using proyecto_centauro.Interfaces;
using proyecto_centauro.Models;
using proyecto_centauro.Models.DTO;

namespace proyecto_centauro.Controllers
{
    [Route("api/coche")]
    [ApiController]
    public class CocheController : Controller
    {
        private readonly ICocheRepositorio _cocheRepositorio;

        public CocheController(ICocheRepositorio cocheRepositorio)
        {
            _cocheRepositorio = cocheRepositorio;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Coche>>> GetCoches()
        {
            var coches = await _cocheRepositorio.ObtenerTodosCoches();
            return Ok(coches);
        }

        [HttpGet("con-grupo")]
        public async Task<ActionResult<IEnumerable<Coche>>> GetCochesConGrupo()
        {
            var coches = await _cocheRepositorio.ObtenerTodosCochesConRelacionGrupo();
            return Ok(coches);
        }

/*
        [HttpGet("con-filtrado-sucursal")]
        public async Task<ActionResult<IEnumerable<Coche>>> ObtenerCochesFiltradosBySucursalId([FromQuery] int? sucursalId)
        {
            try
            {
                var coches = await _cocheRepositorio.ObtenerCochesFiltradosBySucursalId(sucursalId); 
                if (coches == null || !coches.Any())
                {
                    return NotFound(new { mensaje = "No se encontraron coches para esta sucursal." });
                }

                return Ok(coches);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Hubo un error al obtener los coches.", detalle = ex.Message });
            }
        }
*/

        [HttpGet("con-filtrado")]
        public async Task<ActionResult<IEnumerable<CocheDisponibilidadDTO>>> GetCochesFiltrados(
            [FromQuery] int? sucursalId, 
            [FromQuery] DateTime? fechainicio, 
            [FromQuery] DateTime? fechaFin)
        {
            try
            {
                var coches = await _cocheRepositorio.ObtenerCochesFiltrados(sucursalId, fechainicio, fechaFin); 
                if (coches == null || !coches.Any())
                {
                    return NotFound(new { mensaje = "No se encontraron coches para esta sucursal." });
                }

                return Ok(coches);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Hubo un error al obtener los coches.", detalle = ex.Message });
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Coche>> GetCocheById(int id)
        {
            var coche = await _cocheRepositorio.ObtenerCochePorId(id);
            if (coche == null) return NotFound();
            return Ok(coche);
        }

        [HttpPost]
        public async Task<ActionResult> CrearCoche([FromForm] Coche coche, IFormFile? imagen)
        {
            if (coche == null)
                return BadRequest("Coche no debe ser nulo");

            if (imagen != null && imagen.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imagen.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imagen.CopyToAsync(stream);
                }
                coche.Imagen = uniqueFileName;
            }

            await _cocheRepositorio.AgregarCoche(coche);

            var cocheDTO = new Coche
            {
                Id = coche.Id,
                Marca = coche.Marca,
                Modelo = coche.Modelo,
                Descripcion = coche.Descripcion,
                Patente = coche.Patente,
                Tipo_coche = coche.Tipo_coche,
                Tipo_cambio = coche.Tipo_cambio,
                Num_plazas = coche.Num_plazas,
                Num_maletas = coche.Num_maletas,
                Num_puertas = coche.Num_puertas,
                Posee_aire_acondicionado = coche.Posee_aire_acondicionado,
                Imagen = coche.Imagen,
                GrupoId = coche.GrupoId,
                SucursalId = coche.SucursalId,
                Grupo = coche.Grupo,
                Sucursal = coche.Sucursal
            };
            return StatusCode(201, cocheDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditarCoche(int id, [FromForm] Coche coche, IFormFile? imagen)
        {
            if (id != coche.Id)
                return BadRequest("El ID no coincide");

            try
            {
                if (imagen != null && imagen.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes");

                    if (!string.IsNullOrEmpty(coche.Imagen)) // eliminar imagen anterior si existe
                    {
                        var imagenAnteriorPath = Path.Combine(uploadsFolder, coche.Imagen);
                        if (System.IO.File.Exists(imagenAnteriorPath))
                        {
                            System.IO.File.Delete(imagenAnteriorPath);
                        }
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imagen.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imagen.CopyToAsync(stream);
                    }

                    coche.Imagen = uniqueFileName;
                }

                await _cocheRepositorio.ActualizarCoche(coche);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno al actualizar el coche: {ex.Message}");
            }
        }
        /*
        public async Task<ActionResult> EditarCoche(int id, [FromBody] Coche coche)
        {
            if (id != coche.Id) return BadRequest();
            await _cocheRepositorio.ActualizarCoche(coche);
            return NoContent();
        }
        */

        [HttpDelete("{id}")]
        public async Task<ActionResult> BorrarCoche(int id)
        {
            var existeCoche = await _cocheRepositorio.ExisteCoche(id);
            if (!existeCoche) return NotFound();

            await _cocheRepositorio.EliminarCoche(id);
            return NoContent();
        }

    }
}