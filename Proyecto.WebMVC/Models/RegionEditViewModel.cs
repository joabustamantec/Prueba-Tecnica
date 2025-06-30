using System.ComponentModel.DataAnnotations;

namespace Proyecto.WebMVC.Models
{
    public class RegionEditViewModel
    {
        [Required]
        public int IdRegion { get; set; }

        [Required(ErrorMessage = "El nombre de la región es obligatorio.")]
        [StringLength(128, ErrorMessage = "No puede exceder {1} caracteres.")]
        public string NombreRegion { get; set; } = string.Empty;
    }
}
