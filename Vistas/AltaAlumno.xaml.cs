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
    /// Interaction logic for AltaAlumno.xaml
    /// </summary>
    public partial class AltaAlumno : Window
    {
        public AltaAlumno()
        {
            InitializeComponent();
        }

        private void btnAltaAlumno_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblError.Text = "";

                string dniAlumno = txtDniAlumno.Text.Trim();
                string nombreAlumno = txtNombreAlumno.Text.Trim();
                string apellidoAlumno = txtApellidoAlumno.Text.Trim();
                string emailAlumno = txtEmailAlumno.Text.Trim();

                if (string.IsNullOrEmpty(dniAlumno))
                {
                    lblError.Text = "Por favor ingrese el DNI del alumno";
                    txtDniAlumno.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(nombreAlumno))
                {
                    lblError.Text = "Por favor ingrese el nombre del alumno";
                    txtNombreAlumno.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(apellidoAlumno))
                {
                    lblError.Text = "Por favor ingrese el apellido del alumno";
                    txtApellidoAlumno.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(emailAlumno))
                {
                    lblError.Text = "Por favor ingrese el email del alumno";
                    txtEmailAlumno.Focus();
                    return;
                }

                Alumno alumno = new Alumno
                {
                    Alu_DNI = dniAlumno,
                    Alu_Apellido = apellidoAlumno,
                    Alu_Nombre = nombreAlumno,
                    Alu_Email = emailAlumno
                };

                MessageBoxResult result = MessageBox.Show(
                    "Alumno a crear:\n" +
                    "DNI: " + alumno.Alu_DNI + "\n" +
                    "Apellido: " + alumno.Alu_Apellido + "\n" +
                    "Nombre: " + alumno.Alu_Nombre + "\n" +
                    "Email: " + alumno.Alu_Email + "\n\n" +
                    "¿Desea confirmar el registro?",
                    "Confirmación de Registro",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.OK)
                {
                    bool registrado = TrabajarAlumnos.AltaAlumno(alumno);

                    if (registrado)
                    {
                        MessageBox.Show("Alumno registrado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtDniAlumno.Text = "";
                        txtApellidoAlumno.Text = "";
                        txtNombreAlumno.Text = "";
                        txtEmailAlumno.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("No se pudo registrar el alumno.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar alumno: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


   }
}
