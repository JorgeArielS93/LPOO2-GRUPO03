using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using ClaseBase;

namespace Vistas
{
    /// <summary>
    /// Lógica de interacción para AltaUsuario.xaml
    /// </summary>
    public partial class AltaUsuario : Window
    {
        public AltaUsuario()
        {
            InitializeComponent();
        }
         private void btnAltaUsuario_Click(object sender, RoutedEventArgs e)
        {
            string nombreUsuario = txtNombreUsuario.Text;
            string contraseña = txtContraseña.Password;
            string apellidoNombre = txtApellidoNombre.Text;
            int rolId = cmbRol.SelectedIndex + 1; 

            if (string.IsNullOrWhiteSpace(nombreUsuario))
            {
                lblError.Text = "Ingrese nombre de usuario";
                return;
            }
            if (string.IsNullOrWhiteSpace(contraseña))
            {
                lblError.Text = "Ingrese contraseña";
                return;
            }
            if (string.IsNullOrWhiteSpace(apellidoNombre))
            {
                lblError.Text = "Ingrese apellido y nombre";
                return;
            }
            if (cmbRol.SelectedItem == null)
            {
                lblError.Text = "Seleccione un Rol.";
                return;
            }

            Usuario usuario = new Usuario
            {
                Usu_ID = 1,
                Usu_NombreUsuario = nombreUsuario,
                Usu_ApellidoNombre = apellidoNombre,
                Usu_Contraseña = contraseña,
                Rol_ID = rolId
            };
           MessageBoxResult result = MessageBox.Show(
                    "Datos de Usuario:\n" +
                    "Nombre de Usuario: " + usuario.Usu_NombreUsuario + "\n" +
                    "Contraseña: " + usuario.Usu_Contraseña + "\n" +
                    "Apellido y Nombre: " + usuario.Usu_ApellidoNombre + "\n" +
                    "Rol: " + ((ComboBoxItem) cmbRol.SelectedItem).Content + "\n" +
                    "¿Desea confirmar el registro?",
                    "Confirmación de Registro",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Question
                );
        }
    }
}
