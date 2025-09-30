using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ClaseBase
{
    public class TrabajarCursos
    {
        
        public DataTable TraerCursos()
        {
            DataTable tabla = new DataTable();
            SqlConnection cn = null;
            
            try
            {
                cn = new SqlConnection(ClaseBase.Properties.Settings.Default.BDInstituto);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM Curso";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tabla);
            }
            catch (SqlException sqlEx)
            {
               
                throw new Exception("Error de base de datos: " + sqlEx.Message);
            }
            catch (InvalidOperationException invEx)
            {
               
                throw new Exception("Error de conexión: " + invEx.Message);
            }
            catch (Exception ex)
            {
               
                throw new Exception("Error inesperado al obtener los cursos: " + ex.Message);
            }
            finally
            {
            
                if (cn != null && cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
            
            return tabla;
        }
        
    }
}