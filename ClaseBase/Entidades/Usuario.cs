using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClaseBase
{
    public class Usuario : Roles
    {
        public int Usu_ID { get; set; }
        public string Usu_NombreUsuario { get; set; }
        public string Usu_Contraseña { get; set; }
        public string Usu_ApellidoNombre { get; set; }
        public int Rol_ID { get; set; }
    }
}
