using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel; // 1. Namespace necesario para INotifyPropertyChanged 

namespace ClaseBase
{
    public class Usuario : Roles, INotifyPropertyChanged
    {
        private int usu_ID;
        public int Usu_ID
        {
            get { return usu_ID; }
            set
            {
                usu_ID = value;
                Notificador("Usu_ID"); //Se notifica el cambio
            }
        }

        private string usu_NombreUsuario;
        public string Usu_NombreUsuario
        {
            get { return usu_NombreUsuario; }
            set
            {
                usu_NombreUsuario = value;
                Notificador("Usu_NombreUsuario");
            }
        }

        private string usu_Contraseña;
        public string Usu_Contraseña
        {
            get { return usu_Contraseña; }
            set
            {
                usu_Contraseña = value;
                Notificador("Usu_Contraseña");
            }
        }

        private string usu_ApellidoNombre;
        public string Usu_ApellidoNombre
        {
            get { return usu_ApellidoNombre; }
            set
            {
                usu_ApellidoNombre = value;
                Notificador("Usu_ApellidoNombre"); //Se notifica el cambio
            }
        }

        private int _rol_ID;
        public new int Rol_ID
        {
            get { return _rol_ID; }
            set
            {
                _rol_ID = value;
                base.Rol_ID = value; // Opcional: mantener sincronizada la propiedad base
                Notificador("Rol_ID"); // 4. Se notifica el cambio
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Notificador(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
