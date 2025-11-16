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
        private Alumno alumnoEncontrado; // Guardamos el alumno aquí

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
                // 1. Buscar al alumno por DNI
                alumnoEncontrado = TrabajarAlumnos.TraerAlumnoPorDNI(txtDNI.Text.Trim());

                if (alumnoEncontrado != null)
                {
                    // 2. Mostrar nombre y cargar sus inscripciones
                    lblNombreAlumno.Text = "Alumno: " + alumnoEncontrado.Alu_Apellido + ", " + alumnoEncontrado.Alu_Nombre;
                    CargarInscripciones(alumnoEncontrado.Alu_ID);
                }
                else
                {
                    // No se encontró
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
                // 3. Traer la lista de inscripciones (Ins_ID, Cur_Nombre)
                DataTable dtInscripciones = TrabajarInscripciones.TraerInscripcionesActivasPorAlumno(aluID);

                if (dtInscripciones.Rows.Count > 0)
                {
                    // 4. Poblar el ComboBox
                    cmbInscripciones.ItemsSource = dtInscripciones.DefaultView;
                    cmbInscripciones.DisplayMemberPath = "Cur_Nombre"; // Muestra el nombre del curso
                    cmbInscripciones.SelectedValuePath = "Ins_ID";     // El valor oculto es el ID de la inscripción

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
                // 5. Obtener el ID de la inscripción seleccionada
                int inscripcionID = (int)cmbInscripciones.SelectedValue;

                // 6. Pedir confirmación
                MessageBoxResult result = MessageBox.Show(
                    "¿Está seguro de que desea ANULAR la inscripción al curso '" + ((DataRowView)cmbInscripciones.SelectedItem)["Cur_Nombre"] + "'?",
                    "Confirmar Anulación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    // 7. Llamar al método de anulación
                    bool exito = TrabajarInscripciones.AnularInscripcion(inscripcionID);

                    if (exito)
                    {
                        MessageBox.Show("Inscripción anulada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        
                        // 8. Recargar la lista de inscripciones (para que desaparezca la anulada)
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
                lblError.Text = "Error al anular: " + ex.Message; // Error aquí
                MessageBox.Show("Error: " + ex.Message, "Error de Anulación", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Resetea el ComboBox y el botón de anular.
        /// </summary>
        private void LimpiarCamposInscripcion()
        {
            cmbInscripciones.ItemsSource = null;
            cmbInscripciones.IsEnabled = false;
            btnAnular.IsEnabled = false;
        }
    }
}
