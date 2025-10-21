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
using ClaseBase.Servicios;

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
                txtPassword.Text = usuario.Usu_Contraseña;
                txtID.Text = usuario.Usu_ID.ToString();
                cmbRol.SelectedValue = usuario.Rol_ID;
            }
        }

        private void LimpiarCampos()
        {
            txtID.Clear();
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
                Usu_Contraseña = txtPassword.Text,
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
            if (listaUsuarios.Count > 0)
            {
                var usuario = listaUsuarios[indexActual];
                bool ok = TrabajarUsuarios.EliminarUsuario(usuario.Usu_ID);
                MessageBox.Show(ok ? "Usuario eliminado." : "Error al eliminar usuario.");
                CargarUsuarios();
            }
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            esNuevo = false;
            // Los campos ya están listos para editar
        }
    }
}