using Proyecto.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace Proyecto.WebMVC.Models
{
    public class ComunaEditViewModel
    {
        public int IdRegion { get; set; }
        public int IdComuna { get; set; }

        [Required(ErrorMessage = "El nombre de la comuna es obligatorio.")]
        [StringLength(128, ErrorMessage = "El nombre de la comuna no puede exceder {1} caracteres.")]
        [Display(Name = "Nombre de la comuna")]
        public string NombreComuna { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "La superficie debe ser un número positivo.")]
        [Display(Name = "Superficie")]
        public decimal Superficie { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "La población debe ser un número entero no negativo.")]
        [Display(Name = "Población")]
        public int Poblacion { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "La densidad debe ser un número positivo.")]
        [Display(Name = "Densidad")]
        public decimal Densidad { get; set; }
    }
}
