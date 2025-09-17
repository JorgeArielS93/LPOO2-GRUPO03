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
        private List<Usuario> usuarios;
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
            
            usuarios = new List<Usuario>
            {
                new Usuario { Usu_ID = 1, Usu_NombreUsuario = "admin", Usu_Contraseña = "123456", Usu_ApellidoNombre = "Rodriguez, Juan Carlos", Rol_ID = 1 },
                new Usuario { Usu_ID = 2, Usu_NombreUsuario = "docente", Usu_Contraseña = "doc123", Usu_ApellidoNombre = "Garcia, Maria Elena", Rol_ID = 2 },
                new Usuario { Usu_ID = 3, Usu_NombreUsuario = "recepcion", Usu_Contraseña = "rec123", Usu_ApellidoNombre = "Lopez, Ana Beatriz", Rol_ID = 3 },
            };

        }

        private void btnIngresar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblError.Text = "";


                String nombreUsuario = login.Usuario;
                String contraseña = login.Contraseña;
                
               
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
                
           
                Usuario usuarioEncontrado = usuarios.FirstOrDefault(u => 
                    u.Usu_NombreUsuario.Equals(nombreUsuario, StringComparison.OrdinalIgnoreCase) && 
                    u.Usu_Contraseña == contraseña);
                
                if (usuarioEncontrado != null)
                {
                
                    Roles rolUsuario = roles.FirstOrDefault(r => r.Rol_ID == usuarioEncontrado.Rol_ID);
        
                    
                    string mensajeBienvenida = "¡Bienvenido al sistema!\n\n" +
                                                "Usuario: " + usuarioEncontrado.Usu_ApellidoNombre + "\n" +
                                                "Rol: " + (rolUsuario != null ? rolUsuario.Rol_Descripcion : "Sin rol asignado");;
                    
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
                MessageBox.Show("Error al procesar el login: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
