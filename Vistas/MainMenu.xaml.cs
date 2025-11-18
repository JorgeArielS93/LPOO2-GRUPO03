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
using Vistas.Usuarios;
using Vistas.Inscripcion;

namespace Vistas
{
    /// <summary>
    /// Lógica de interacción para MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            MainWindow login = new MainWindow();
            login.Show();
            this.Close();
        }

        private void ABMUsuarios_Click(object sender, RoutedEventArgs e)
        {
            ABMUsuarios ventanaABM = new ABMUsuarios();
            ventanaABM.Show();
        }
        private void MenuUsuarios_Click(object sender, RoutedEventArgs e)
        {
            AltaUsuario alta = new AltaUsuario();
            alta.Show();
        }
        
        private void MenuAltaAlumno_Click(object sender, RoutedEventArgs e)
        {
            AltaAlumno alta = new AltaAlumno();
            alta.Show();  
        }

        private void MenuModificarAlumno_Click(object sender, RoutedEventArgs e)
        {
            ModificarAlumno modificar = new ModificarAlumno();
            modificar.Show();
        }

        private void MenuDocentes_Click(object sender, RoutedEventArgs e)
        {
            AltaDocente alta = new AltaDocente();
            alta.Show();
        }

        private void MenuCursos_Click(object sender, RoutedEventArgs e)
        {
            AltaCurso alta = new AltaCurso();
            alta.Show();
        }

        private void MenuListaCursos_Click(object sender, RoutedEventArgs e)
        {
            GrillaCursos grilla = new GrillaCursos();
            grilla.Show();
        }

        private void ListaUsuarios_Click(object sender, RoutedEventArgs e)
        {
            ListadoUsuarios listado = new ListadoUsuarios();
            listado.Show();
        }

        private void MenuInscripciones_Click(object sender, RoutedEventArgs e)
        {
            frmInscripcion inscripcion = new frmInscripcion();
            inscripcion.Show();
        }

        private void MenuAnularInscripcion_Click(object sender, RoutedEventArgs e)
        {
            AnularInscripciones anular = new AnularInscripciones();
            anular.Show();
        }

        private void MenuAcreditarInscripcion_Click(object sender, RoutedEventArgs e)
        {
            AcreditarInscripcion acreditar = new AcreditarInscripcion();
            acreditar.Show();
        }
    }
}
