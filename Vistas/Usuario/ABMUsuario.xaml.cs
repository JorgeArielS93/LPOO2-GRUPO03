using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Vistas.Usuarios
{
    public partial class ABMUsuarios : Window
    {
        private ObservableCollection<Usuario> listaUsuarios;
        private int indexActual = 0;
        private bool esNuevo = false;

        public ABMUsuarios()
        {
            InitializeComponent();
            CargarRoles();
            CargarUsuarios();
        }

        private void CargarRoles()
        {
            var dt = TrabajarUsuarios.getRoles();

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("No se encontraron roles.");
                return;
            }

            cmbRol.DisplayMemberPath = "Rol_Descripcion";
            cmbRol.SelectedValuePath = "Rol_ID";
            cmbRol.ItemsSource = dt.DefaultView;
        }

        private void CargarUsuarios()
        {
            listaUsuarios = TrabajarUsuarios.TraerUsuarios();
            if (listaUsuarios.Count > 0)
            {
                indexActual = 0;
                MostrarUsuario();
            }
            else
            {
                LimpiarCampos();
            }
        }

        private void MostrarUsuario()
        {
            if (listaUsuarios.Count > 0 && indexActual >= 0 && indexActual < listaUsuarios.Count)
            {
                var usuario = listaUsuarios[indexActual];
                txtApellidoNombre.Text = usuario.Usu_ApellidoNombre;
                txtUserName.Text = usuario.Usu_NombreUsuario;
                txtPassword.Password = usuario.Usu_Contraseña;
                cmbRol.SelectedValue = usuario.Rol_ID;
            }
        }

        private void LimpiarCampos()
        {
            txtApellidoNombre.Clear();
            txtUserName.Clear();
            txtPassword.Clear();
            cmbRol.SelectedIndex = -1;
        }

        // --- NAVEGACIÓN ---
        private void btnPrimero_Click(object sender, RoutedEventArgs e)
        {
            indexActual = 0;
            MostrarUsuario();
        }

        private void btnAnterior_Click(object sender, RoutedEventArgs e)
        {
            if (indexActual > 0)
                indexActual--;
            MostrarUsuario();
        }

        private void btnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            if (indexActual < listaUsuarios.Count - 1)
                indexActual++;
            MostrarUsuario();
        }

        private void btnUltimo_Click(object sender, RoutedEventArgs e)
        {
            indexActual = listaUsuarios.Count - 1;
            MostrarUsuario();
        }

        // --- ACCIONES ABM ---
        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            esNuevo = true;
            LimpiarCampos();
            cmbRol.SelectedIndex = -1;
            txtApellidoNombre.Focus();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (cmbRol.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un rol.");
                return;
            }

            Usuario usuario = new Usuario
            {
                Usu_ApellidoNombre = txtApellidoNombre.Text,
                Usu_NombreUsuario = txtUserName.Text,
                Usu_Contraseña = txtPassword.Password,
                Rol_ID = (int)cmbRol.SelectedValue
            };

            bool ok;
            if (esNuevo)
            {
                ok = TrabajarUsuarios.AltaUsuario(usuario);
                MessageBox.Show(ok ? "Usuario agregado correctamente." : "Error al agregar usuario.");
            }
            else
            {
                usuario.Usu_ID = listaUsuarios[indexActual].Usu_ID;
                ok = TrabajarUsuarios.ModificarUsuario(usuario);
                MessageBox.Show(ok ? "Usuario modificado correctamente." : "Error al modificar usuario.");
            }

            esNuevo = false;
            CargarUsuarios();
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
            {
                if (listaUsuarios == null || listaUsuarios.Count == 0)
                {
                    MessageBox.Show("No hay usuarios para eliminar.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtApellidoNombre.Text) || 
                    string.IsNullOrWhiteSpace(txtUserName.Text) || 
                    cmbRol.SelectedIndex == -1)
                {
                    MessageBox.Show("Debe seleccionar o cargar un usuario antes de eliminar.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var usuario = listaUsuarios[indexActual];

                MessageBoxResult result = MessageBox.Show(
                string.Format("¿Está seguro de que desea eliminar al usuario:\n\n{0} ({1})?",
                              usuario.Usu_ApellidoNombre, usuario.Usu_NombreUsuario),
                "Confirmar eliminación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);


                if (result == MessageBoxResult.Yes)
                {
                    bool ok = TrabajarUsuarios.EliminarUsuario(usuario.Usu_ID);

                    if (ok)
                    {
                        MessageBox.Show("Usuario eliminado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        CargarUsuarios();
                    }
                    else
                    {
                        MessageBox.Show("Error al eliminar usuario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }


        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (listaUsuarios.Count > 0 && indexActual >= 0 && indexActual < listaUsuarios.Count)
            {
                esNuevo = false;

                MostrarUsuario();

                MessageBox.Show("Usuario cargado para modificar.", "Modificar Usuario", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("No hay usuario seleccionado para modificar.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}