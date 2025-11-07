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
    /// Lógica de interacción para ModificarAlumno.xaml
    /// </summary>
    public partial class ModificarAlumno : Window
    {
        public ModificarAlumno()
        {
            InitializeComponent();
        }

        private void txtIdAlumnoMod_TextChanged(object sender, TextChangedEventArgs e)
        {
            int id;
            if (int.TryParse(txtIdAlumnoMod.Text, out id))
            {
                var odp = this.Resources["DATA_ALUMNO"] as ObjectDataProvider;
                if (odp != null)
                {
                    odp.MethodParameters.Clear();
                    odp.MethodParameters.Add(id);
                    odp.Refresh();
                }
            }
        }

        private void btnModificarAlumno_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblError.Text = "";

                string idAlumnoText = txtIdAlumnoMod.Text.Trim();
                string dniAlumno = txtDniAlumnoMod.Text.Trim();
                string nombreAlumno = txtNombreAlumnoMod.Text.Trim();
                string apellidoAlumno = txtApellidoAlumnoMod.Text.Trim();
                string emailAlumno = txtEmailAlumnoMod.Text.Trim();

                if (string.IsNullOrEmpty(idAlumnoText))
                {
                    lblError.Text = "Por favor ingrese el ID del alumno";
                    txtIdAlumnoMod.Focus();
                    return;
                }
                int idAlumno;
                if (!int.TryParse(idAlumnoText, out idAlumno))
                {
                    lblError.Text = "El ID debe ser un número válido";
                    txtIdAlumnoMod.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(dniAlumno))
                {
                    lblError.Text = "Por favor ingrese el DNI del alumno";
                    txtDniAlumnoMod.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(nombreAlumno))
                {
                    lblError.Text = "Por favor ingrese el nombre del alumno";
                    txtNombreAlumnoMod.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(apellidoAlumno))
                {
                    lblError.Text = "Por favor ingrese el apellido del alumno";
                    txtApellidoAlumnoMod.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(emailAlumno))
                {
                    lblError.Text = "Por favor ingrese el email del alumno";
                    txtEmailAlumnoMod.Focus();
                    return;
                }

                Alumno alumno = new Alumno
                {
                    Alu_ID = idAlumno,
                    Alu_DNI = dniAlumno,
                    Alu_Apellido = apellidoAlumno,
                    Alu_Nombre = nombreAlumno,
                    Alu_Email = emailAlumno
                };
        
                TrabajarAlumnos.ModificarAlumno(alumno);
                MessageBox.Show("Alumno modificado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                txtIdAlumnoMod.Text = "";
                txtDniAlumnoMod.Text = "";
                txtApellidoAlumnoMod.Text = "";
                txtNombreAlumnoMod.Text = "";
                txtEmailAlumnoMod.Text = "";
        
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar alumno: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}

    

