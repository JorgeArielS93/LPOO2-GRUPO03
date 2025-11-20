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
using ClaseBase.Servicios;

namespace Vistas
{
    public partial class AnularInscripciones : Window
    {
        private Alumno alumnoEncontrado; 

        public AnularInscripciones()
        {
            InitializeComponent();
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDNI.Text))
            {
                lblError.Text = "Por favor, ingrese un DNI.";
                return;
            }

            try
            {
                alumnoEncontrado = TrabajarAlumnos.TraerAlumnoPorDNI(txtDNI.Text.Trim());

                if (alumnoEncontrado != null)
                {
                    lblNombreAlumno.Text = "Alumno: " + alumnoEncontrado.Alu_Apellido + ", " + alumnoEncontrado.Alu_Nombre;
                    CargarInscripciones(alumnoEncontrado.Alu_ID);
                }
                else
                {
                    lblError.Text = "No se encontró ningún alumno con ese DNI.";
                    lblNombreAlumno.Text = "Esperando DNI...";
                    LimpiarCamposInscripcion();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error al buscar alumno: " + ex.Message;
                MessageBox.Show("Error: " + ex.Message, "Error de Búsqueda", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CargarInscripciones(int aluID)
        {
            try
            {
                DataTable dtInscripciones = TrabajarInscripciones.TraerInscripcionesActivasPorAlumno(aluID);

                if (dtInscripciones.Rows.Count > 0)
                {
                    cmbInscripciones.ItemsSource = dtInscripciones.DefaultView;
                    cmbInscripciones.DisplayMemberPath = "Cur_Nombre"; 
                    cmbInscripciones.SelectedValuePath = "Ins_ID";     

                    cmbInscripciones.SelectedIndex = 0;
                    cmbInscripciones.IsEnabled = true;
                    btnAnular.IsEnabled = true;
                    lblError.Text = "";
                }
                else
                {
                    lblError.Text = "El alumno no tiene inscripciones activas.";
                    LimpiarCamposInscripcion();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error al cargar inscripciones: " + ex.Message;
                MessageBox.Show("Error: " + ex.Message, "Error de Carga", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAnular_Click(object sender, RoutedEventArgs e)
        {
            if (cmbInscripciones.SelectedItem == null || alumnoEncontrado == null)
            {
                lblError.Text = "Debe seleccionar una inscripción válida.";
                return;
            }

            try
            {
                int inscripcionID = (int)cmbInscripciones.SelectedValue;

                MessageBoxResult result = MessageBox.Show(
                    "¿Está seguro de que desea ANULAR la inscripción al curso '" + ((DataRowView)cmbInscripciones.SelectedItem)["Cur_Nombre"] + "'?",
                    "Confirmar Anulación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    bool exito = TrabajarInscripciones.AnularInscripcion(inscripcionID);

                    if (exito)
                    {
                        DataRowView row = (DataRowView)cmbInscripciones.SelectedItem;
                        int curId = Convert.ToInt32(row["Cur_ID"]);

                        TrabajarInscripciones.AumentarCupo(curId);

                        MessageBox.Show("Inscripción anulada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                        CargarInscripciones(alumnoEncontrado.Alu_ID);
                    }
                    else
                    {
                        MessageBox.Show("No se pudo anular la inscripción.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

            }
            catch (Exception ex)
            {
                lblError.Text = "Error al anular: " + ex.Message; 
                MessageBox.Show("Error: " + ex.Message, "Error de Anulación", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LimpiarCamposInscripcion()
        {
            cmbInscripciones.ItemsSource = null;
            cmbInscripciones.IsEnabled = false;
            btnAnular.IsEnabled = false;
        }
    }
}
