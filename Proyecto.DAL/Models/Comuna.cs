using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.DAL.Models
{
    public class Comuna
    {
        public int IdComuna { get; set; }
        public int IdRegion { get; set; }
        public string NombreComuna { get; set; } = string.Empty;
        public InformacionAdicional InformacionAdicional { get; set; } = new InformacionAdicional();
    }
}
