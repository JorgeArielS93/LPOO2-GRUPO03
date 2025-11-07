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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClaseBase;

namespace Vistas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Roles> roles;
        public MainWindow()
        {
            InitializeComponent();
            InicializarDatos();
        }

        private void InicializarDatos() {
            roles = new List<Roles>
            {
                new Roles { Rol_ID = 1, Rol_Descripcion = "Administrador" },
                new Roles { Rol_ID = 2, Rol_Descripcion = "Docente" },
                new Roles { Rol_ID = 3, Rol_Descripcion = "Recepcion" }
            };

        }

        private void btnIngresar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblError.Text = "";

                string nombreUsuario = login.Usuario;
                string contraseña = login.Contraseña;

                if (string.IsNullOrEmpty(nombreUsuario))
                {
                    lblError.Text = "Por favor ingrese el nombre de usuario";
                    return;
                }

                if (string.IsNullOrEmpty(contraseña))
                {
                    lblError.Text = "Por favor ingrese la contraseña";
                    return;
                }

                Usuario usuarioEncontrado = TrabajarUsuarios.AutenticarUsuario(nombreUsuario, contraseña);

                if (usuarioEncontrado != null)
                {
                    Roles rolUsuario = roles.FirstOrDefault(r => r.Rol_ID == usuarioEncontrado.Rol_ID);

                    string mensajeBienvenida = "¡Bienvenido al sistema!\n\n" +
                                               "Usuario: " + usuarioEncontrado.Usu_ApellidoNombre + "\n" +
                                               "Rol: " + (rolUsuario != null ? rolUsuario.Rol_Descripcion : "Sin rol asignado");

                    MessageBox.Show(mensajeBienvenida, "Acceso Concedido",
                                    MessageBoxButton.OK, MessageBoxImage.Information);

                    MainMenu menu = new MainMenu();
                    this.Close();
                    menu.Show();
                }
                else
                {
                    lblError.Text = "Usuario o contraseña incorrectos";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al procesar el login: " + ex.Message,
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
