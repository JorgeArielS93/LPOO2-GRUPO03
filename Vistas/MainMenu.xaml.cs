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

        private void MenuUsuarios_Click(object sender, RoutedEventArgs e)
        {
            AltaUsuario alta = new AltaUsuario();
            alta.Show();
        }
        
        private void MenuAlumnos_Click(object sender, RoutedEventArgs e)
        {
            AltaAlumno alta = new AltaAlumno();
            alta.Show();  
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
    }
}
