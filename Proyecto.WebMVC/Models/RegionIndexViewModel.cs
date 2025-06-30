using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Proyecto.DAL.Models;

namespace Proyecto.WebMVC.Models
{
    public class RegionIndexViewModel
    {
        public IEnumerable<Region> Regiones { get; set; } = new List<Region>();

        [Required(ErrorMessage = "El nombre de la región es obligatorio.")]
        [StringLength(128, ErrorMessage = "No puede exceder {1} caracteres.")]
        [Display(Name = "Nombre de la región")]
        public string NuevaRegionNombre { get; set; } = string.Empty;
    }
}
