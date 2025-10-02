using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Vistas
{
    public class ConversorDeEstados : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Brushes.Transparent;

            string key = value.ToString();

            switch (key)
            {
                case "1": return Brushes.Green;       // Programado
                case "2": return Brushes.Orange;      // En_curso
                case "3": return Brushes.DodgerBlue;  // Finalizado
                case "4": return Brushes.Red;         // Cancelado
                default: return Brushes.Gray;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}