using System;
using System.Data;
using System.Windows;
using ClaseBase;

namespace Proyecto.Vistas
{
    public partial class Resultado : Window
    {
        public Resultado()
        {
            InitializeComponent();
            CargarAlumnos();
        }

        private void CargarAlumnos()
        {
            cmbAlumnos.ItemsSource = TrabajarInscripciones.TraerAlumnos().DefaultView;
        }

        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            if (cmbAlumnos.SelectedValue == null)
                return;

            int idAlumno = Convert.ToInt32(cmbAlumnos.SelectedValue);

            DataTable todos = TrabajarInscripciones.TraerInscripciones(idAlumno);
            dgResultados.ItemsSource = todos.DefaultView;

            int finalizados = 0;
            int enCurso = 0;

            foreach (DataRow row in todos.Rows)
            {
                string estado = row["EstadoCurso"].ToString();
                if (estado == "Finalizado")
                    finalizados++;
                else if (estado == "EnCurso" || estado == "En_Curso")
                    enCurso++;
            }

            txtFinalizados.Text = finalizados.ToString();
            txtEnCurso.Text = enCurso.ToString();
        }
    }
}
