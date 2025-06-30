using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Proyecto.DAL.DataAccess;
using Proyecto.DAL.Models;
using System;
using System.Collections.Generic;

namespace Proyecto.API.Controllers
{
    [ApiController]
    [Route("region")]
    public class RegionController : ControllerBase
    {
        private readonly RegionDAL _regionDAL;
        private readonly ILogger<RegionController> _logger;

        public RegionController(RegionDAL regionDAL, ILogger<RegionController> logger)
        {
            _regionDAL = regionDAL;
            _logger = logger;
        }

        // GET /region
        [HttpGet]
        public ActionResult<List<Region>> GetAll()
        {
            var regs = _regionDAL.ListarRegiones();
            return Ok(regs);
        }

        // GET /region/{id}
        [HttpGet("{id}")]
        public ActionResult<Region> GetById(int id)
        {
            var region = _regionDAL.ObtenerRegion(id);
            if (region == null)
                return NotFound();
            return Ok(region);
        }

        // POST /region
        [HttpPost]
        public IActionResult Save([FromBody] Region region)
        {
            if (region == null || string.IsNullOrWhiteSpace(region.NombreRegion))
                return BadRequest("Datos inválidos.");

            try
            {
                _regionDAL.GuardarOActualizar(region);
                return Ok(new { mensaje = "Región guardada correctamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al guardar región {region.IdRegion}");
                return StatusCode(500, "Error interno al guardar región.");
            }
        }

       
    }
}
