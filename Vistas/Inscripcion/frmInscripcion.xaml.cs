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
                dtAlumnos = TrabajarInscripciones.TraerAlumnos();
                cboAlumnos.ItemsSource = dtAlumnos.DefaultView;

                dtCursos = TrabajarInscripciones.TraerCursosProgramados();
                cboCursos.ItemsSource = dtCursos.DefaultView;

                CargarInscripciones();

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
                lblMensaje.Text = "";

                if (cboAlumnos.SelectedValue == null)
                {
                    MostrarMensaje("Por favor, seleccione un alumno.", Brushes.Orange);
                    cboAlumnos.Focus();
                    return;
                }

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

                // Validacion 3: Verificar que el curso tenga cupo disponible
                int cupoActual = Convert.ToInt32(((DataRowView)cboCursos.SelectedItem)["Cur_Cupo"]);
                if (cupoActual <= 0)
                {
                    MostrarMensaje("No hay cupos disponibles para este curso.", Brushes.Red);
                    return;
                }


                MessageBoxResult resultado = MessageBox.Show(
                    "Esta seguro que desea registrar esta inscripcion?",
                    "Confirmar Inscripcion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    ClaseBase.Inscripcion nuevaInscripcion = new ClaseBase.Inscripcion
                    {
                        Ins_Fecha = DateTime.Now,
                        Cur_ID = curId.ToString(),
                        Alu_ID = aluId,
                        Est_ID = TrabajarInscripciones.ObtenerEstadoInscripto() 
                    };

                    bool exito = TrabajarInscripciones.RegistrarInscripcion(nuevaInscripcion);

                    if (exito)
                    {
                        DataRowView alumnoRow = (DataRowView)cboAlumnos.SelectedItem;
                        DataRowView cursoRow = (DataRowView)cboCursos.SelectedItem;

                        if (alumnoRow == null || cursoRow == null)
                        {
                            MostrarMensaje("Inscripción registrada, pero no se pudo obtener detalle del alumno/curso.", Brushes.Green);
                        }
                        else
                        {
                            string aluApellido = alumnoRow["Alu_Apellido"] == DBNull.Value ? "" : alumnoRow["Alu_Apellido"].ToString();
                            string aluNombre = alumnoRow["Alu_Nombre"] == DBNull.Value ? "" : alumnoRow["Alu_Nombre"].ToString();
                            string curNombre = cursoRow["Cur_Nombre"] == DBNull.Value ? "" : cursoRow["Cur_Nombre"].ToString();

                            string nombreCompleto = string.Format("{0} {1}", aluApellido, aluNombre);
                            string nombreCurso = curNombre;
                            TrabajarInscripciones.DescontarCupo(curId);
                            dtCursos = TrabajarInscripciones.TraerCursosProgramados();
                            cboCursos.ItemsSource = dtCursos.DefaultView;

                            string mensaje = string.Format("Inscripcion registrada exitosamente!\r\nAlumno: {0}\r\nCurso: {1}",
                                                           nombreCompleto, nombreCurso);
                            MostrarMensaje(mensaje, Brushes.Green);
                        }

                        CargarInscripciones();
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
