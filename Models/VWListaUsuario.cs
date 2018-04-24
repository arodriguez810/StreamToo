using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public partial class VWListaUsuario
    {
        public string Nombre { get; set; }
        public string Primer_Apellido { get; set; }
        public string Segundo_Apellido { get; set; }
        public string Fecha_de_Nacimiento { get; set; }
        public string Identificacion { get; set; }
        public string Calle_y_Número { get; set; }
        public string sector { get; set; }
        public string Ciudad { get; set; }
        public string Provincia { get; set; }
        public string País { get; set; }
        public string Celular { get; set; }
        public string Teléfono { get; set; }
        public string Correo_Electrónico { get; set; }
        public string username { get; set; }
        public string Estado { get; set; }
        public string Fecha_de_Registro { get; set; }
        public string Última_Actualización { get; set; }
        public string Última_Visita { get; set; }
        public string Última_IP { get; set; }
    }
}
