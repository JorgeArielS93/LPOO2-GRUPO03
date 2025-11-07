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
using System.Xml;

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
            cargar_docentes(); 
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

                cmbDocente.ItemsSource = docentes;
                cmbDocente.DisplayMemberPath = "Doc_Nombre";   
                cmbDocente.SelectedValuePath = "Doc_ID";      
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los docentes: " + ex.Message,
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

                int estadoId = int.Parse(cmbEstadoCurso.SelectedValue.ToString());

                XmlElement estadoSeleccionado = (XmlElement)cmbEstadoCurso.SelectedItem;
                int estId = int.Parse(estadoSeleccionado.GetAttribute("key"));
                string estNombre = estadoSeleccionado.InnerText;

                Curso curso = new Curso
                {
                    Cur_Nombre = txtNombreCurso.Text,
                    Cur_Descripcion = txtDescripcionCurso.Text,
                    Cur_Cupo = int.Parse(txtCupoCurso.Text),
                    Cur_FechaInicio = FechaInicio.SelectedDate ?? DateTime.MinValue,
                    Cur_FechaFin = FechaFin.SelectedDate ?? DateTime.MinValue,
                    Est_ID = estId,
                    Doc_ID = (int)cmbDocente.SelectedValue
                };


                MessageBoxResult result = MessageBox.Show(
                    "Curso creado:\n" +
                    "Nombre: " + curso.Cur_Nombre + "\n" +
                    "Descripción: " + curso.Cur_Descripcion + "\n" +
                    "Cupo: " + curso.Cur_Cupo + "\n" +
                    "Fecha de inicio: " + curso.Cur_FechaInicio.ToShortDateString() + "\n" +
                    "Fecha de finalizacion: " + curso.Cur_FechaFin.ToShortDateString() + "\n" +
                    "Estado: " + estNombre + "\n" +
                    "Docente: " + ((Docente)cmbDocente.SelectedItem).Doc_Nombre + "\n\n" +
                    "¿Desea confirmar el registro?",
                    "Confirmación de Registro",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Question
                );

                
                if (result == MessageBoxResult.OK)
                {

                    TrabajarCursos.InsertarCurso(curso);
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
