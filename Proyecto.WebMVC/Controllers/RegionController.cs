using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Proyecto.WebMVC.Models;
using Proyecto.WebMVC.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto.WebMVC.Controllers
{
    public class RegionController : Controller
    {
        private readonly RegionService _regionService;
        private readonly ILogger<RegionController> _logger;

        public RegionController(RegionService regionService, ILogger<RegionController> logger)
        {
            _regionService = regionService;
            _logger = logger;
        }

        // GET: /Region/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var regs = await _regionService.ObtenerRegiones();
            var vm = new RegionIndexViewModel
            {
                Regiones = regs,
                NuevaRegionNombre = string.Empty
            };
            return View(vm);
        }

        // POST: /Region/Crear
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(RegionIndexViewModel vm)
        {
            // 1) Validación básica
            if (!ModelState.IsValid)
            {
                vm.Regiones = await _regionService.ObtenerRegiones();
                return View("Index", vm);
            }

            var nuevoNombre = vm.NuevaRegionNombre.Trim();

            // 2) Chequeo de duplicados (case-insensitive)
            var existentes = await _regionService.ObtenerRegiones();
            if (existentes.Any(r =>
                    string.Equals(r.NombreRegion.Trim(),
                                  nuevoNombre,
                                  StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError(
                    nameof(vm.NuevaRegionNombre),
                    "Ya existe una región con ese nombre.");
                vm.Regiones = existentes;
                return View("Index", vm);
            }

            // 3) Persistir
            var nueva = new Proyecto.DAL.Models.Region
            {
                NombreRegion = nuevoNombre
            };
            var ok = await _regionService.GuardarRegion(nueva);

            TempData[ok ? "Exito" : "Error"] = ok
                ? "Región creada correctamente."
                : "No se pudo crear la región.";

            // 4) PRG
            return RedirectToAction(nameof(Index));
        }

        // Proyecto.WebMVC/Controllers/RegionController.cs
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RegionEditViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                // recargar lista si hay error de validación
                var regs = await _regionService.ObtenerRegiones();
                var idxVm = new RegionIndexViewModel
                {
                    Regiones = regs,
                    NuevaRegionNombre = string.Empty
                };
                return View("Index", idxVm);
            }

            // chequeo de duplicados (excluyendo la propia)
            var existentes = await _regionService.ObtenerRegiones();
            if (existentes.Any(r =>
                    r.IdRegion != vm.IdRegion &&
                    string.Equals(r.NombreRegion.Trim(),
                                  vm.NombreRegion.Trim(),
                                  StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError(
                    nameof(vm.NombreRegion),
                    "Ya existe otra región con ese nombre.");
                var regs = await _regionService.ObtenerRegiones();
                var idxVm = new RegionIndexViewModel
                {
                    Regiones = regs,
                    NuevaRegionNombre = string.Empty
                };
                return View("Index", idxVm);
            }

            // persistir cambios
            var entidad = new Proyecto.DAL.Models.Region
            {
                IdRegion = vm.IdRegion,
                NombreRegion = vm.NombreRegion.Trim()
            };
            var ok = await _regionService.GuardarRegion(entidad);
            TempData[ok ? "Exito" : "Error"] = ok
                ? "Región actualizada correctamente."
                : "No se pudo actualizar la región.";

            return RedirectToAction(nameof(Index));
        }


    }
}
