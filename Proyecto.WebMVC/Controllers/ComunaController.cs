using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Proyecto.DAL.Models;
using Proyecto.WebMVC.Models;
using Proyecto.WebMVC.Services;
using System.Threading.Tasks;

namespace Proyecto.WebMVC.Controllers
{
    public class ComunaController : Controller
    {
        private readonly RegionService _regionService;
        private readonly ComunaService _comunaService;
        private readonly ILogger<ComunaController> _logger;

        public ComunaController(
            RegionService regionService,
            ComunaService comunaService,
            ILogger<ComunaController> logger)
        {
            _regionService = regionService;
            _comunaService = comunaService;
            _logger = logger;
        }

        // GET: /Comuna/Index?regionId=5
        [HttpGet]
        public async Task<IActionResult> Index(int regionId)
        {
            var region = await _regionService.ObtenerRegionPorId(regionId);
            if (region == null) return NotFound();

            var comunas = await _comunaService.ObtenerComunas(regionId);
            var vm = new ComunaListViewModel
            {
                IdRegion = region.IdRegion,
                NombreRegion = region.NombreRegion,
                Comunas = comunas,
                NuevaComuna = new ComunaEditViewModel { IdRegion = region.IdRegion }
            };
            return View(vm);  // Views/Comuna/Index.cshtml
        }

        // POST: /Comuna/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ComunaListViewModel vm)
        {
            // 1) Validación básica de ModelState
            if (!ModelState.IsValid)
            {
                await CargarListasParaRetry(vm);
                return View("Index", vm);
            }

            // 2) Compruebo si ya existe una comuna con ese nombre en la región
            var existentes = await _comunaService.ObtenerComunas(vm.IdRegion);
            if (existentes.Any(c =>
                    string.Equals(c.NombreComuna?.Trim(),
                                  vm.NuevaComuna.NombreComuna?.Trim(),
                                  StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError(
                    key: "NuevaComuna.NombreComuna",
                    errorMessage: "Ya existe una comuna con ese nombre en esta región.");
                await CargarListasParaRetry(vm);
                return View("Index", vm);
            }

            // 3) Construyo la entidad y guardo
            var nueva = new Comuna
            {
                IdRegion = vm.IdRegion,
                NombreComuna = vm.NuevaComuna.NombreComuna.Trim(),
                InformacionAdicional = new InformacionAdicional
                {
                    Superficie = vm.NuevaComuna.Superficie,
                    Poblacion = vm.NuevaComuna.Poblacion,
                    Densidad = vm.NuevaComuna.Densidad
                }
            };
            var ok = await _comunaService.GuardarComuna(nueva);
            TempData[ok ? "Exito" : "Error"] = ok
                ? "Comuna creada correctamente."
                : "Ocurrió un error al crear la comuna.";

            // 4) PRG
            return RedirectToAction(nameof(Index), new { regionId = vm.IdRegion });
        }

        // Método privado para recargar región y lista de comunas en caso de retry
        private async Task CargarListasParaRetry(ComunaListViewModel vm)
        {
            var region = await _regionService.ObtenerRegionPorId(vm.IdRegion);
            vm.NombreRegion = region?.NombreRegion ?? "";
            vm.Comunas = await _comunaService.ObtenerComunas(vm.IdRegion);
        }

        // GET: /Comuna/Edit?regionId=5&comunaId=10
        [HttpGet]
        public async Task<IActionResult> Edit(int regionId, int comunaId)
        {
            var comuna = await _comunaService.ObtenerComuna(regionId, comunaId);
            if (comuna == null) return NotFound();

            var info = comuna.InformacionAdicional;
            var vm = new ComunaEditViewModel
            {
                IdRegion = comuna.IdRegion,
                IdComuna = comuna.IdComuna,
                NombreComuna = comuna.NombreComuna,
                Superficie = info.Superficie,
                Poblacion = info.Poblacion,
                Densidad = info.Densidad
            };
            return View(vm);  // Views/Comuna/Edit.cshtml
        }

        // POST: /Comuna/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ComunaEditViewModel vm)
        {
            // 1) Validación básica de modelo
            if (!ModelState.IsValid)
                return View(vm);

            // 2) COMPROBAR DUPLICADO de nombre en la región (excluyendo la actual)
            var todas = await _comunaService.ObtenerComunas(vm.IdRegion);
            if (todas.Any(c =>
                    c.IdComuna != vm.IdComuna &&
                    string.Equals(c.NombreComuna?.Trim(),
                                  vm.NombreComuna?.Trim(),
                                  StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError(
                    key: nameof(vm.NombreComuna),
                    errorMessage: "Ya existe otra comuna con ese nombre en esta región.");
                return View(vm);
            }

            // 3) Construir entidad y guardar
            var updated = new Comuna
            {
                IdRegion = vm.IdRegion,
                IdComuna = vm.IdComuna,
                NombreComuna = vm.NombreComuna.Trim(),
                InformacionAdicional = new InformacionAdicional
                {
                    Superficie = vm.Superficie,
                    Poblacion = vm.Poblacion,
                    Densidad = vm.Densidad
                }
            };
            var ok = await _comunaService.GuardarComuna(updated);

            // 4) Mensaje de resultado
            TempData[ok ? "Exito" : "Error"] = ok
                ? "Comuna actualizada correctamente."
                : "Ocurrió un error al actualizar la comuna.";

            // 5) PRG
            return RedirectToAction(nameof(Index), new { regionId = vm.IdRegion });
        }

    }
}
