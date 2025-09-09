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
    /// Interaction logic for AltaCurso.xaml
    /// </summary>
    public partial class AltaCurso : Window
    {
        public AltaCurso()
        {
            InitializeComponent();
        }

        private void AltaCurso_Loaded(object sender, RoutedEventArgs e)
        {
            List<Docente> docentes = new List<Docente>
            {
                new Docente { Doc_ID = 1, Doc_Nombre = "Juan Pérez" },
                new Docente { Doc_ID = 2, Doc_Nombre = "Fabiana" },
            };

            cmbDocente.ItemsSource = docentes;
            cmbDocente.DisplayMemberPath = "Doc_Nombre"; 
            cmbDocente.SelectedValuePath = "Doc_ID";    
        }

        private void btnAltaCurso_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblError.Text = "";
            
                if (string.IsNullOrWhiteSpace(txtNombreCurso.Text))
                {
                    lblError.Text = "Ingrese nombre del curso";
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtCupoCurso.Text))
                {
                    lblError.Text = "Ingrese cupo";
                    return;
                }
                if (FechaInicio.SelectedDate == null || FechaFin.SelectedDate == null)
                {
                    lblError.Text = "Debe seleccionar ambas fechas: inicio y fin";
                    return;
                }

                if (FechaInicio.SelectedDate > FechaFin.SelectedDate)
                {
                    lblError.Text = "La fecha de inicio no puede ser posterior a la fecha de finalización";
                    return;
                }
                if (cmbEstadoCurso.SelectedItem == null)
                {
                    lblError.Text = "Seleccione estado";
                    return;
                }
                if (cmbDocente.SelectedItem == null)
                {
                    lblError.Text = "Seleccione docente";
                    return;
                }

                Curso curso = new Curso
                {
                    Cur_Nombre = txtNombreCurso.Text,
                    Cur_Descripcion = txtDescripcionCurso.Text,
                    Cur_Cupo = int.Parse(txtCupoCurso.Text),
                    Cur_FechaInicio = FechaInicio.SelectedDate ?? DateTime.MinValue,
                    Cur_FechaFin = FechaFin.SelectedDate ?? DateTime.MinValue,
                    Est_ID = cmbEstadoCurso.SelectedIndex + 1, 
                    Doc_ID = cmbDocente.SelectedIndex + 1     
                };

                // Confirmación
                MessageBoxResult result = MessageBox.Show(
                    "Curso creado:\n" +
                    "Nombre: " + curso.Cur_Nombre + "\n" +
                    "Descripción: " + curso.Cur_Descripcion + "\n" +
                    "Cupo: " + curso.Cur_Cupo + "\n" +
                    "Fecha de inicio: " + curso.Cur_FechaInicio.ToShortDateString() + "\n" +
                    "Fecha de finalizacion: " + curso.Cur_FechaFin.ToShortDateString() + "\n" +
                    "Estado: " + ((ComboBoxItem)cmbEstadoCurso.SelectedItem).Content + "\n" +
                    "Docente: " + ((Docente)cmbDocente.SelectedItem).Doc_Nombre + "\n\n" +
                    "¿Desea confirmar el registro?",
                    "Confirmación de Registro",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Question
                );

                
                if (result == MessageBoxResult.OK)
                {
                    MessageBox.Show("Curso registrado correctamente.", "Éxito", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar curso: " + ex.Message, "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
