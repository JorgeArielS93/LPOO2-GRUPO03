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
    /// Interaction logic for AltaDocente.xaml
    /// </summary>
    public partial class AltaDocente : Window
    {

        public AltaDocente()
        {
            InitializeComponent();
        }

        private void btnAltaDocente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblError.Text = "";

                string dniDocente = txtDniDocente.Text.Trim();
                string nombreDocente = txtNombreDocente.Text.Trim();
                string apellidoDocente = txtApellidoDocente.Text.Trim();
                string emailDocente = txtEmailDocente.Text.Trim();

                if (string.IsNullOrEmpty(dniDocente))
                {
                    lblError.Text = "Por favor ingrese el DNI del docente";
                    txtDniDocente.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(nombreDocente))
                {
                    lblError.Text = "Por favor ingrese el nombre del docente";
                    txtNombreDocente.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(apellidoDocente))
                {
                    lblError.Text = "Por favor ingrese el apellido del docente";
                    txtApellidoDocente.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(emailDocente))
                {
                    lblError.Text = "Por favor ingrese el email del docente";
                    txtEmailDocente.Focus();
                    return;
                }


                Docente docente = new Docente
                {
                    Doc_DNI = txtDniDocente.Text,
                    Doc_Apellido = txtApellidoDocente.Text,
                    Doc_Nombre = txtNombreDocente.Text,
                    Doc_Email = txtEmailDocente.Text
                };


                MessageBoxResult result = MessageBox.Show(
                                   "Docente creado:\n" +
                                   "DNI: " + docente.Doc_DNI + "\n" +
                                   "Apellido: " + docente.Doc_Apellido + "\n" +
                                   "Nombre: " + docente.Doc_Nombre + "\n" +
                                   "Email: " + docente.Doc_Email + "\n\n" +
                                   "¿Desea confirmar el registro?",
                                   "Confirmación de Registro",
                                   MessageBoxButton.OKCancel,
                                   MessageBoxImage.Question
               );

                if (result == MessageBoxResult.OK)
                {
                    bool registrado = TrabajarDocente.AltaDocente(docente);

                    if (registrado)
                    {
                        MessageBox.Show("Docente registrado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtDniDocente.Text = "";
                        txtApellidoDocente.Text = "";
                        txtNombreDocente.Text = "";
                        txtEmailDocente.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("No se pudo registrar el docente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar docente: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
