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
                Alumno alu = new Alumno
                {
                    Alu_ID = int.Parse(txtIdAlumnoMod.Text),
                    Alu_DNI = txtDniAlumnoMod.Text,
                    Alu_Nombre = txtNombreAlumnoMod.Text,
                    Alu_Apellido = txtApellidoAlumnoMod.Text,
                    Alu_Email = txtEmailAlumnoMod.Text
                };

                TrabajarAlumnos.ModificarAlumno(alu);

                MessageBox.Show("Alumno modificado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (FormatException)
            {
                MessageBox.Show("El ID debe ser un número válido.", "Error de formato", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar el alumno: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

    

