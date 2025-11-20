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
using System.Data;

using ClaseBase;

namespace Vistas
{
    public partial class AltaUsuario : Window
    {
        public AltaUsuario()
        {
            InitializeComponent();
        }

        private void AltaUsuario_Loaded(object sender, RoutedEventArgs e)
        {
            cargar_roles();
        }

        private void cargar_roles()
        {

            DataTable dt = TrabajarUsuarios.getRoles();

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("No se encontraron roles.");
                return;
            }

            cmbRol.Items.Clear();
            cmbRol.DisplayMemberPath = "Rol_Descripcion";
            cmbRol.SelectedValuePath = "Rol_ID";
            cmbRol.ItemsSource = dt.DefaultView;
            cmbRol.SelectedIndex = -1;
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
                Usu_NombreUsuario = nombreUsuario,
                Usu_ApellidoNombre = apellidoNombre,
                Usu_Contraseña = contraseña,
                Rol_ID = rolId
            };
            DataRowView filaSeleccionada = (DataRowView)cmbRol.SelectedItem;
            string descripcionRol = filaSeleccionada["Rol_Descripcion"].ToString();

            MessageBoxResult result = MessageBox.Show(
                "Datos de Usuario:\n" +
                "Nombre de Usuario: " + usuario.Usu_NombreUsuario + "\n" +
                "Contraseña: " + usuario.Usu_Contraseña + "\n" +
                "Apellido y Nombre: " + usuario.Usu_ApellidoNombre + "\n" +
                "Rol: " + descripcionRol + "\n\n" +
                "¿Desea confirmar el registro?",
                "Confirmación de Registro",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question);


           if (result == MessageBoxResult.OK)
           {
               if (!TrabajarUsuarios.ExisteNombreUsuario(usuario.Usu_NombreUsuario))
               {
                   bool ok = TrabajarUsuarios.AltaUsuario(usuario);
                   MessageBox.Show(ok ? "Usuario agregado correctamente." : "Error al agregar usuario.");
               }
               else
               {
                   MessageBox.Show("El Nombre de Usuario ya existe.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
               }
           }
        }

     
   
    }
}
