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
        // Traer todos los Cursos
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

        // Alta de un nuevo curso
        public static void InsertarCurso(Curso curso)
        {
            using (SqlConnection conn = new SqlConnection(ClaseBase.Properties.Settings.Default.BDInstituto))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Curso (Cur_Nombre, Cur_Descripcion, Cur_Cupo, Cur_FechaInicio, Cur_FechaFin, Est_ID, Doc_ID) " +
                    "VALUES (@nombre, @desc, @cupo, @fechaInicio, @fechaFin, @estado, @docente)", conn);

                cmd.Parameters.AddWithValue("@nombre", curso.Cur_Nombre);
                cmd.Parameters.AddWithValue("@desc", curso.Cur_Descripcion ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@cupo", curso.Cur_Cupo);
                cmd.Parameters.AddWithValue("@fechaInicio", curso.Cur_FechaInicio);
                cmd.Parameters.AddWithValue("@fechaFin", curso.Cur_FechaFin);
                cmd.Parameters.AddWithValue("@estado", curso.Est_ID);
                cmd.Parameters.AddWithValue("@docente", curso.Doc_ID);

                int filas = cmd.ExecuteNonQuery();
            }
        }
        /*
        public static DataTable TraerCursos()
        {
            using (SqlConnection conn = new SqlConnection(cadenaConexion))
            {
                string sql = "SELECT * FROM Curso";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }*/
    }
}