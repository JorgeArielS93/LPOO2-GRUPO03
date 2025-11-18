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

namespace Vistas.Inscripcion
{

    public partial class AcreditarInscripcion : Window
    {
        private int alumnoID = -1;

        public AcreditarInscripcion()
        {
            InitializeComponent();
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            string dni = txtDNI.Text.Trim();

            if (dni == "")
            {
                MessageBox.Show("Ingrese un DNI.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Ahora retorna un Alumno (NO DataTable)
            Alumno alu = TrabajarAlumnos.TraerAlumnoPorDNI(dni);

            if (alu == null)
            {
                MessageBox.Show("Alumno no encontrado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                dgCursos.ItemsSource = null;
                alumnoID = -1;
                return;
            }

            alumnoID = alu.Alu_ID; // Guardamos su ID

            // Obtener cursos en los cuales está inscrito y el curso está ENCURSO
            DataTable dtCursos = TrabajarInscripciones.TraerInscripcionesEnCursoPorAlumno(alumnoID);

            if (dtCursos.Rows.Count == 0)
            {
                MessageBox.Show("El alumno no posee cursos en estado 'EnCurso'.",
                                "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);

                dgCursos.ItemsSource = null;
                return;
            }

            dgCursos.ItemsSource = dtCursos.DefaultView;
        }

        private void BtnAcreditar_Click(object sender, RoutedEventArgs e)
        {
            if (dgCursos.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un curso.");
                return;
            }

            DataRowView drv = (DataRowView)dgCursos.SelectedItem;
            int insID = (int)drv["Ins_ID"];

            bool ok = TrabajarInscripciones.AcreditarInscripcion(insID);

            if (ok)
                MessageBox.Show("Acreditación realizada correctamente.");
            else
                MessageBox.Show("Error al acreditar.");

            dgCursos.ItemsSource = null;
        }
    }
}
