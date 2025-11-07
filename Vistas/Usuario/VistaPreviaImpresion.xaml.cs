using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


using System.Windows.Controls;
using System.Windows.Documents;

using ClaseBase;

namespace Vistas.Usuarios
{
    /// <summary>
    /// Lógica de interacción para VistaPreviaImpresion.xaml
    /// </summary>
    public partial class VistaPreviaImpresion : Window
    {
        public VistaPreviaImpresion(List<Usuario> usuarios)
        {
            InitializeComponent();
            lvUsuarios.ItemsSource = usuarios;
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() == true)
            {
                pd.PrintDocument(((IDocumentPaginatorSource)DocMain).DocumentPaginator,
                                 "Listado de Usuarios");
            }
        }
    }
}
