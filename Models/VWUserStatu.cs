using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class VWUserStatu
    {
        public string Nombre { get; set; }
        public string Primer_Apellido { get; set; }
        public string Segundo_Apellido { get; set; }
        public string Estado { get; set; }
        public string Identificacion { get; set; }
        public string País { get; set; }
        public string Celular { get; set; }
        public string Teléfono { get; set; }
        public string Correo_Electrónico { get; set; }
        public string Última_IP { get; set; }
        public int ID { get; set; }
    }
}
