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
using System.Collections.ObjectModel;
using System.Globalization;
using ClaseBase;
using ClaseBase.Servicios;

namespace Vistas.Usuarios
{
    public partial class ListadoUsuarios : Window
    {
        private ObservableCollection<Usuario> listaUsuarios;

        public ListadoUsuarios()
        {
            InitializeComponent();
            ActualizarContador();
        }

        private void CargarUsuarios()
        {
            try
            {
                listaUsuarios = TrabajarUsuarios.TraerUsuarios();

                if (listaUsuarios != null && listaUsuarios.Count > 0)
                {
                    var usuariosOrdenados = listaUsuarios.OrderBy(u => u.Usu_NombreUsuario).ToList();
                    listaUsuarios = new ObservableCollection<Usuario>(usuariosOrdenados);

                    dgUsuarios.ItemsSource = listaUsuarios;
                }
                else
                {
                    dgUsuarios.ItemsSource = null;
                }

                ActualizarContador();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar usuarios: " + ex.Message, "Error",
                               MessageBoxButton.OK, MessageBoxImage.Error);
                lblTotalUsuarios.Text = "Total de usuarios: Error";
            }
        }

        private void ActualizarContador()
        {
            try
            {
                if (listaUsuarios != null && listaUsuarios.Count > 0)
                {
                    lblTotalUsuarios.Text = "Total de usuarios: " + listaUsuarios.Count.ToString();
                }
                else
                {
                    lblTotalUsuarios.Text = "Total de usuarios: 0";
                }
            }
            catch (Exception ex)
            {
                lblTotalUsuarios.Text = "Total de usuarios: Error";
                MessageBox.Show("Error al actualizar contador: " + ex.Message, "Error",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() => CargarUsuarios()),
                                 System.Windows.Threading.DispatcherPriority.Background);
        }

        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CargarUsuarios();
                MessageBox.Show("Lista de usuarios actualizada.", "Información",
                               MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar: " + ex.Message, "Error",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (listaUsuarios == null) return;

            string textoBusqueda = txtBuscar.Text.ToLower();

            if (string.IsNullOrEmpty(textoBusqueda))
            {
                dgUsuarios.ItemsSource = listaUsuarios;
            }
            else
            {
                var usuariosFiltrados = listaUsuarios.Where(u => u.Usu_NombreUsuario.ToLower().Contains(textoBusqueda)).ToList();
                dgUsuarios.ItemsSource = new ObservableCollection<Usuario>(usuariosFiltrados);
            }
        }
    }

    public class ContraseñaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return "••••••••";

            string contraseña = value.ToString();
            return new string('•', Math.Min(contraseña.Length, 8));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "Sin Rol";

            int rolId;
            if (int.TryParse(value.ToString(), out rolId))
            {
                switch (rolId)
                {
                    case 1:
                        return "Administrador";
                    case 2:
                        return "Docente";
                    case 3:
                        return "Recepción";
                    default:
                        return "Sin Rol";
                }
            }

            return "Sin Rol";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}