using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ClaseBase
{
    public class TrabajarInscripciones
    {
        // Verificar si el alumno ya está inscrito en el curso
        public static bool AlumnoYaInscrito(int aluId, int curId)
        {
            using (SqlConnection cn = new SqlConnection(ClaseBase.Properties.Settings.Default.BDInstituto))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Inscripcion WHERE Alu_ID = @aluId AND Cur_ID = @curId", cn);
                
                cmd.Parameters.AddWithValue("@aluId", aluId);
                cmd.Parameters.AddWithValue("@curId", curId);

                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        // Verificar si el curso está en estado "Programado"
        public static bool CursoEstaProgramado(int curId)
        {
            using (SqlConnection cn = new SqlConnection(ClaseBase.Properties.Settings.Default.BDInstituto))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT Est_ID FROM Curso WHERE Cur_ID = @curId", cn);
                
                cmd.Parameters.AddWithValue("@curId", curId);

                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    int estadoId = (int)result;
                    // Asumiendo que Est_ID = 1 es "Programado"
                    return estadoId == 1;
                }
                return false;
            }
        }

        // Obtener el ID del estado "Inscripto" para inscripciones
        public static int ObtenerEstadoInscripto()
        {
            using (SqlConnection cn = new SqlConnection(ClaseBase.Properties.Settings.Default.BDInstituto))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT Est_ID FROM Estado WHERE Est_Nombre = 'Inscripto' AND Esty_ID = 2", cn);
                
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    return (int)result;
                }
                // Si no existe, devolver un valor por defecto (ajustar según la BD)
                return 5; // Ajustar según los IDs de tu base de datos
            }
        }

        // Registrar una nueva inscripción
        public static bool RegistrarInscripcion(Inscripcion inscripcion)
        {
            using (SqlConnection cn = new SqlConnection(ClaseBase.Properties.Settings.Default.BDInstituto))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Inscripcion (Ins_Fecha, Cur_ID, Alu_ID, Est_ID) " +
                    "VALUES (@fecha, @curId, @aluId, @estId)", cn);

                cmd.Parameters.AddWithValue("@fecha", inscripcion.Ins_Fecha);
                cmd.Parameters.AddWithValue("@curId", inscripcion.Cur_ID);
                cmd.Parameters.AddWithValue("@aluId", inscripcion.Alu_ID);
                cmd.Parameters.AddWithValue("@estId", inscripcion.Est_ID);

                int filas = cmd.ExecuteNonQuery();
                return filas > 0;
            }
        }

        // Traer todos los alumnos
        public static DataTable TraerAlumnos()
        {
            DataTable tabla = new DataTable();
            using (SqlConnection cn = new SqlConnection(ClaseBase.Properties.Settings.Default.BDInstituto))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Alu_ID, Alu_DNI, Alu_Apellido, Alu_Nombre, Alu_Email FROM Alumno ORDER BY Alu_Apellido", cn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tabla);
            }
            return tabla;
        }

        // Traer cursos programados
        public static DataTable TraerCursosProgramados()
        {
            DataTable tabla = new DataTable();
            using (SqlConnection cn = new SqlConnection(ClaseBase.Properties.Settings.Default.BDInstituto))
            {
                cn.Open();
                // Est_ID = 1 asumiendo que es "Programado"
                SqlCommand cmd = new SqlCommand(
                    "SELECT c.Cur_ID, c.Cur_Nombre, c.Cur_Descripcion, c.Cur_Cupo, " +
                    "c.Cur_FechaInicio, c.Cur_FechaFin, e.Est_Nombre " +
                    "FROM Curso c " +
                    "INNER JOIN Estado e ON c.Est_ID = e.Est_ID " +
                    "WHERE c.Est_ID = 1 " +
                    "ORDER BY c.Cur_Nombre", cn);
                
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tabla);
            }
            return tabla;
        }

        // Traer todas las inscripciones
        public static DataTable TraerInscripciones()
        {
            DataTable tabla = new DataTable();
            using (SqlConnection cn = new SqlConnection(ClaseBase.Properties.Settings.Default.BDInstituto))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT i.Ins_ID, i.Ins_Fecha, " +
                    "a.Alu_DNI, a.Alu_Apellido, a.Alu_Nombre, " +
                    "c.Cur_Nombre, e.Est_Nombre " +
                    "FROM Inscripcion i " +
                    "INNER JOIN Alumno a ON i.Alu_ID = a.Alu_ID " +
                    "INNER JOIN Curso c ON i.Cur_ID = c.Cur_ID " +
                    "INNER JOIN Estado e ON i.Est_ID = e.Est_ID " +
                    "ORDER BY i.Ins_Fecha DESC", cn);
                
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tabla);
            }
            return tabla;
        }
    }
}
