using System;
using System.Collections.Generic;
using System.Data;
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
using System.Data.SqlClient;
using ClaseBase;

namespace Vistas
{
    /// <summary>
    /// Interaction logic for GrillaCursos.xaml
    /// </summary>
    public partial class GrillaCursos : Window
    {
        public GrillaCursos()
        {
            InitializeComponent();
            ActualizarContador();
        }

        private void ActualizarContador()
        {
            try
            {
                // Obtener el ObjectDataProvider
                ObjectDataProvider provider = (ObjectDataProvider)FindResource("list_cursos");
                if (provider != null && provider.Data != null)
                {
                    DataTable tabla = (DataTable)provider.Data;
                    lblTotalCursos.Text = "Total de cursos: " + tabla.Rows.Count.ToString();
                }
                else
                {
                    lblTotalCursos.Text = "Total de cursos: 0";
                }
            }
            catch (Exception ex)
            {
                lblTotalCursos.Text = "Total de cursos: Error";
                MessageBox.Show("Error al actualizar contador: " + ex.Message, "Error", 
                               MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Refrescar el ObjectDataProvider
                ObjectDataProvider provider = (ObjectDataProvider)FindResource("list_cursos");
                if (provider != null)
                {
                    provider.Refresh();
                    ActualizarContador();
                    MessageBox.Show("Lista de cursos actualizada.", "Información", 
                                   MessageBoxButton.OK, MessageBoxImage.Information);
                }
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Dar tiempo para que se carguen los datos y luego actualizar contador
            Dispatcher.BeginInvoke(new Action(() => ActualizarContador()), 
                                 System.Windows.Threading.DispatcherPriority.Background);
        }

    }
}