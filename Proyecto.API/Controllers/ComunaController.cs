using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Proyecto.DAL.DataAccess;
using Proyecto.DAL.Models;
using System;
using System.Collections.Generic;

namespace Proyecto.API.Controllers
{
    [ApiController]
    [Route("region/{idRegion}/comuna")]
    public class ComunaController : ControllerBase
    {
        private readonly ComunaDAL _comunaDAL;
        private readonly ILogger<ComunaController> _logger;

        public ComunaController(ComunaDAL comunaDAL, ILogger<ComunaController> logger)
        {
            _comunaDAL = comunaDAL;
            _logger = logger;
        }

        // GET /region/{idRegion}/comuna
        [HttpGet]
        public ActionResult<List<Comuna>> GetPorRegion(int idRegion)
        {
            var comunas = _comunaDAL.ListarPorRegion(idRegion);
            return Ok(comunas);
        }

        // GET /region/{idRegion}/comuna/{idComuna}
        [HttpGet("{idComuna}")]
        public ActionResult<Comuna> GetPorId(int idRegion, int idComuna)
        {
            var comuna = _comunaDAL.Obtener(idRegion, idComuna);
            if (comuna == null)
                return NotFound();
            return Ok(comuna);
        }

        // POST /region/{idRegion}/comuna
        [HttpPost]
        public IActionResult Guardar(int idRegion, [FromBody] Comuna comuna)
        {
            if (comuna == null || string.IsNullOrWhiteSpace(comuna.NombreComuna))
                return BadRequest("Datos inválidos.");

            comuna.IdRegion = idRegion;
            try
            {
                _comunaDAL.GuardarOActualizar(comuna);
                return Ok(new { mensaje = "Comuna guardada correctamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al guardar comuna en región {idRegion}");
                return StatusCode(500, "Error interno al guardar comuna.");
            }
        }


    }
}
