using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClaseBase.Servicios
{
    public class TrabajarEstado
    {
        public static int ObtenerEstadoID(string nombreEstado, string tipoEstado)
        {
            using (SqlConnection cn = new SqlConnection(ClaseBase.Properties.Settings.Default.BDInstituto))
            {
                cn.Open();
                // Usamos JOIN para buscar por el nombre del Estado y el nombre del Tipo de Estado
                string query = @"
                    SELECT E.Est_ID 
                    FROM Estado E
                    INNER JOIN EstadoType ET ON E.Esty_ID = ET.Esty_ID
                    WHERE E.Est_Nombre = @nombreEstado AND ET.Esty_Nombre = @tipoEstado";
                
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@nombreEstado", nombreEstado);
                cmd.Parameters.AddWithValue("@tipoEstado", tipoEstado);

                object result = cmd.ExecuteScalar();
                
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
                
                // Si no lo encuentra, lanza una excepción clara.
                throw new Exception(string.Format("No se pudo encontrar el ID para el estado '{0}' del tipo '{1}'. Verifique la tabla Estado.", nombreEstado, tipoEstado));
            }
        }
    }
}
