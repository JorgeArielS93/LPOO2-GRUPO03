using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for frmInscripcion.xaml
    /// </summary>
    public partial class frmInscripcion : Window
    {
        private DataTable dtAlumnos;
        private DataTable dtCursos;

        public frmInscripcion()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CargarDatos();
        }

        private void CargarDatos()
        {
            try
            {
                // Cargar alumnos
                dtAlumnos = TrabajarInscripciones.TraerAlumnos();
                cboAlumnos.ItemsSource = dtAlumnos.DefaultView;

                // Cargar cursos programados
                dtCursos = TrabajarInscripciones.TraerCursosProgramados();
                cboCursos.ItemsSource = dtCursos.DefaultView;

                // Cargar inscripciones
                CargarInscripciones();

                // Mensaje informativo si no hay cursos programados
                if (dtCursos.Rows.Count == 0)
                {
                    MostrarMensaje("No hay cursos programados disponibles para inscripcion.", Brushes.Orange);
                    btnInscribir.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al cargar los datos: " + ex.Message, Brushes.Red);
            }
        }

        private void CargarInscripciones()
        {
            try
            {
                DataTable dtInscripciones = TrabajarInscripciones.TraerInscripciones();
                dgInscripciones.ItemsSource = dtInscripciones.DefaultView;
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al cargar inscripciones: " + ex.Message, Brushes.Red);
            }
        }

        private void cboCursos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboCursos.SelectedItem != null)
            {
                DataRowView row = (DataRowView)cboCursos.SelectedItem;
                
                // Mostrar detalles del curso
                txtCupo.Text = row["Cur_Cupo"].ToString();
                txtFechaInicio.Text = Convert.ToDateTime(row["Cur_FechaInicio"]).ToString("dd/MM/yyyy");
                txtFechaFin.Text = Convert.ToDateTime(row["Cur_FechaFin"]).ToString("dd/MM/yyyy");
                
                borderDetallesCurso.Visibility = Visibility.Visible;
            }
            else
            {
                borderDetallesCurso.Visibility = Visibility.Collapsed;
            }
        }

        private void btnInscribir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Limpiar mensaje previo
                lblMensaje.Text = "";

                // Validar selección de alumno
                if (cboAlumnos.SelectedValue == null)
                {
                    MostrarMensaje("Por favor, seleccione un alumno.", Brushes.Orange);
                    cboAlumnos.Focus();
                    return;
                }

                // Validar selección de curso
                if (cboCursos.SelectedValue == null)
                {
                    MostrarMensaje("Por favor, seleccione un curso.", Brushes.Orange);
                    cboCursos.Focus();
                    return;
                }

                int aluId = Convert.ToInt32(cboAlumnos.SelectedValue);
                int curId = Convert.ToInt32(cboCursos.SelectedValue);

                // Validacion 1: Verificar que el curso este en estado "Programado"
                if (!TrabajarInscripciones.CursoEstaProgramado(curId))
                {
                    MostrarMensaje("El curso seleccionado no esta en estado 'Programado'. " +
                                   "Solo se pueden inscribir alumnos a cursos programados.", Brushes.Red);
                    return;
                }

                // Validacion 2: Verificar que el alumno no este ya inscrito en el curso
                if (TrabajarInscripciones.AlumnoYaInscrito(aluId, curId))
                {
                    DataRowView alumnoRow = (DataRowView)cboAlumnos.SelectedItem;
                    DataRowView cursoRow = (DataRowView)cboCursos.SelectedItem;
                    
                    string nombreCompleto = string.Format("{0} {1}", alumnoRow["Alu_Apellido"], alumnoRow["Alu_Nombre"]);
                    string nombreCurso = cursoRow["Cur_Nombre"].ToString();
                    
                    MostrarMensaje(string.Format("El alumno {0} ya esta inscrito en el curso '{1}'.", nombreCompleto, nombreCurso), 
                                   Brushes.Red);
                    return;
                }

                // Confirmar inscripcion
                MessageBoxResult resultado = MessageBox.Show(
                    "Esta seguro que desea registrar esta inscripcion?",
                    "Confirmar Inscripcion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    // Crear objeto de inscripcion
                    Inscripcion nuevaInscripcion = new Inscripcion
                    {
                        Ins_Fecha = DateTime.Now,
                        Cur_ID = curId.ToString(),
                        Alu_ID = aluId,
                        Est_ID = TrabajarInscripciones.ObtenerEstadoInscripto() // Estado "Inscripto"
                    };

                    // Registrar la inscripcion
                    bool exito = TrabajarInscripciones.RegistrarInscripcion(nuevaInscripcion);

                    if (exito)
                    {
                        DataRowView alumnoRow = (DataRowView)cboAlumnos.SelectedItem;
                        DataRowView cursoRow = (DataRowView)cboCursos.SelectedItem;
                        
                        string nombreCompleto = string.Format("{0} {1}", alumnoRow["Alu_Apellido"], alumnoRow["Alu_Nombre"]);
                        string nombreCurso = cursoRow["Cur_Nombre"].ToString();

                        string mensaje = string.Format("Inscripcion registrada exitosamente!\nAlumno: {0}\nCurso: {1}", 
                                                      nombreCompleto, nombreCurso);
                        MostrarMensaje(mensaje, Brushes.Green);

                        // Actualizar la grilla de inscripciones
                        CargarInscripciones();

                        // Limpiar formulario
                        LimpiarFormulario();
                    }
                    else
                    {
                        MostrarMensaje("Error al registrar la inscripcion. Por favor, intente nuevamente.", 
                                       Brushes.Red);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al procesar la inscripcion: " + ex.Message, Brushes.Red);
            }
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            cboAlumnos.SelectedIndex = -1;
            cboCursos.SelectedIndex = -1;
            borderDetallesCurso.Visibility = Visibility.Collapsed;
            lblMensaje.Text = "";
        }

        private void MostrarMensaje(string mensaje, Brush color)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.Foreground = color;
        }
    }
}
