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
    /// Interaction logic for GestionDocente.xaml
    /// </summary>
    public partial class GestionDocente : Window
    {
        public GestionDocente()
        {
            InitializeComponent();
        }
        private void GestionDocente_Loaded(object sender, RoutedEventArgs e)
        {
            cargar_docentes();
        }

        private void cmbSeleccionDocente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSeleccionDocente.SelectedValue == null)
                return;

            int idDocente = (int)cmbSeleccionDocente.SelectedValue;

            var lista = TrabajarCursos.TraerCursosPorDocente(idDocente);

            dgCursos.ItemsSource = lista;
        }

        private void cmbModificarEstado_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            Curso curso = cmb.DataContext as Curso;

            if (curso == null)
                return;

            List<KeyValuePair<int, string>> opciones = new List<KeyValuePair<int, string>>();

            // Programado (ID = 1) → Cancelado (4)
            if (curso.Est_ID == 1)
                opciones.Add(new KeyValuePair<int, string>(4, "Cancelado"));

            // En curso (ID = 2) → Finalizado (3)
            if (curso.Est_ID == 2)
                opciones.Add(new KeyValuePair<int, string>(3, "Finalizado"));

            cmb.ItemsSource = opciones;
            cmb.DisplayMemberPath = "Value";
            cmb.SelectedValuePath = "Key";

            cmb.IsEnabled = opciones.Count > 0;
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Curso curso = btn.Tag as Curso;

            if (curso == null)
                return;

            StackPanel panel = btn.Parent as StackPanel;
            ComboBox cmb = panel.Children[0] as ComboBox;

            if (cmb.SelectedValue == null)
            {
                MessageBox.Show("Seleccione un estado primero.");
                return;
            }

            int nuevoEstado = (int)cmb.SelectedValue;

            TrabajarCursos.ModificarEstado(curso.Cur_ID, nuevoEstado);
            MessageBox.Show("Estado actualizado correctamente.");

            cmbSeleccionDocente_SelectionChanged(null, null);
        }


        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cargar_docentes()
        {
            try
            {
                List<Docente> docentes = TrabajarDocente.TraerDocentes();

                foreach (var d in docentes)
                {
                    d.Doc_Nombre = d.Doc_Apellido + ", " + d.Doc_Nombre;
                }

                cmbSeleccionDocente.ItemsSource = docentes;
                cmbSeleccionDocente.DisplayMemberPath = "Doc_Nombre";
                cmbSeleccionDocente.SelectedValuePath = "Doc_ID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los docentes: " + ex.Message,
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




    }
}