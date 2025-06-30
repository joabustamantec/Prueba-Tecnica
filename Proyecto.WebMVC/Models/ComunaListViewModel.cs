using Proyecto.DAL.Models;
using System.Collections.Generic;

namespace Proyecto.WebMVC.Models
{
    public class ComunaListViewModel
    {
        public int IdRegion { get; set; }
        public string NombreRegion { get; set; } = "";
        public List<Comuna> Comunas { get; set; } = new();
        public ComunaEditViewModel NuevaComuna { get; set; } = new ComunaEditViewModel();
    }
}
