using System;
using System.Globalization;
using System.Windows.Data;

namespace Vistas
{
    public class ConversorNombreEstado : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "";

            string key = value.ToString();

            switch (key)
            {
                case "1": return "Programado";
                case "2": return "En curso";
                case "3": return "Finalizado";
                case "4": return "Cancelado";
                default: return "Desconocido";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}