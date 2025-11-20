using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ClaseBase.Servicios;

namespace ClaseBase
{
    public class TrabajarInscripciones
    {

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

                    return estadoId == 1;
                }
                return false;
            }
        }

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

                return 5;
            }
        }

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

        public static DataTable TraerCursosProgramados()
        {
            DataTable tabla = new DataTable();
            using (SqlConnection cn = new SqlConnection(ClaseBase.Properties.Settings.Default.BDInstituto))
            {
                cn.Open();

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

        public static DataTable TraerInscripcionesActivasPorAlumno(int aluID)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(ClaseBase.Properties.Settings.Default.BDInstituto))
            {
                cn.Open();
                string query = @"
            SELECT 
                I.Ins_ID,
                I.Cur_ID,
                C.Cur_Nombre
            FROM Inscripcion I
            INNER JOIN Curso C ON I.Cur_ID = C.Cur_ID
            INNER JOIN Estado E ON I.Est_ID = E.Est_ID
            WHERE I.Alu_ID = @aluID 
              AND E.Est_Nombre <> 'Cancelado'";

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@aluID", aluID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public static bool AnularInscripcion(int insID)
        {
            int estadoCanceladoID = TrabajarEstado.ObtenerEstadoID("Cancelado", "Inscripcion");

            using (SqlConnection cn = new SqlConnection(ClaseBase.Properties.Settings.Default.BDInstituto))
            {
                cn.Open();
                string query = "UPDATE Inscripcion SET Est_ID = @estID WHERE Ins_ID = @insID";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@estID", estadoCanceladoID);
                cmd.Parameters.AddWithValue("@insID", insID);

                int filasAfectadas = cmd.ExecuteNonQuery();
                return filasAfectadas > 0;
            }
        }
        public static DataTable TraerInscripcionesEnCursoPorAlumno(int aluID)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(ClaseBase.Properties.Settings.Default.BDInstituto))
            {
                cn.Open();

                string query = @"
            SELECT 
                I.Ins_ID,
                C.Cur_Nombre
            FROM Inscripcion I
            INNER JOIN Curso C ON I.Cur_ID = C.Cur_ID
            INNER JOIN Estado EIns ON I.Est_ID = EIns.Est_ID
            INNER JOIN Estado ECur ON C.Est_ID = ECur.Est_ID
            WHERE I.Alu_ID = @aluID
              AND ECur.Est_Nombre = 'En_Curso'";

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@aluID", aluID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }
        public static bool AcreditarInscripcion(int insID)
        {
            int estadoConfirmadoID = TrabajarEstado.ObtenerEstadoID("Confirmado", "Inscripcion");

            using (SqlConnection cn = new SqlConnection(ClaseBase.Properties.Settings.Default.BDInstituto))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Inscripcion SET Est_ID = @estID WHERE Ins_ID = @insID",
                    cn);

                cmd.Parameters.AddWithValue("@estID", estadoConfirmadoID);
                cmd.Parameters.AddWithValue("@insID", insID);

                int filas = cmd.ExecuteNonQuery();
                return filas > 0;
            }
        }

        public static DataTable TraerInscripcionesFinalizadas(int aluID)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.BDInstituto))
            {
                cn.Open();
                string query = @"
            SELECT 
                c.Cur_Nombre,
                d.Doc_Nombre + ' ' + d.Doc_Apellido AS Docente,
                i.Ins_Fecha AS FechaFinalizacion,
                eCur.Est_Nombre AS EstadoCurso
            FROM Inscripcion i
            INNER JOIN Curso c ON i.Cur_ID = c.Cur_ID
            INNER JOIN Docente d ON c.Doc_ID = d.Doc_ID
            INNER JOIN Estado eIns ON i.Est_ID = eIns.Est_ID
            INNER JOIN Estado eCur ON c.Est_ID = eCur.Est_ID
            WHERE i.Alu_ID = @aluID
              AND eCur.Est_Nombre = 'Finalizado'"; // Ajusta según tu lógica

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@aluID", aluID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public static DataTable TraerInscripcionesEnCurso(int aluID)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.BDInstituto))
            {
                cn.Open();
                string query = @"
        SELECT 
            C.Cur_Nombre AS Cur_Nombre,
            D.Doc_Nombre + ' ' + D.Doc_Apellido AS Docente,
            C.Cur_FechaFin AS FechaFinalizacion,
            ECur.Est_Nombre AS EstadoCurso
        FROM Inscripcion I
        INNER JOIN Curso C ON I.Cur_ID = C.Cur_ID
        INNER JOIN Docente D ON C.Doc_ID = D.Doc_ID
        INNER JOIN Estado ECur ON C.Est_ID = ECur.Est_ID
        WHERE I.Alu_ID = @aluID
          AND ECur.Est_Nombre = 'En_Curso'
        ORDER BY C.Cur_FechaFin";

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@aluID", aluID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public static DataTable TraerInscripcionesOtrosEstados(int aluID)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.BDInstituto))
            {
                cn.Open();
                string query = @"
            SELECT 
                c.Cur_Nombre,
                d.Doc_Nombre + ' ' + d.Doc_Apellido AS Docente,
                i.Ins_Fecha AS FechaFinalizacion,
                eCur.Est_Nombre AS EstadoCurso
            FROM Inscripcion i
            INNER JOIN Curso c ON i.Cur_ID = c.Cur_ID
            INNER JOIN Docente d ON c.Doc_ID = d.Doc_ID
            INNER JOIN Estado eIns ON i.Est_ID = eIns.Est_ID
            INNER JOIN Estado eCur ON c.Est_ID = eCur.Est_ID
            WHERE i.Alu_ID = @aluID
              AND eCur.Est_Nombre NOT IN ('Finalizado', 'EnCurso')";

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@aluID", aluID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public static DataTable TraerInscripciones(int aluID)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.BDInstituto))
            {
                cn.Open();
                string query = @"
        SELECT 
            c.Cur_Nombre,
            d.Doc_Nombre + ' ' + d.Doc_Apellido AS Docente,
            i.Ins_Fecha AS FechaFinalizacion,
            eCur.Est_Nombre AS EstadoCurso
        FROM Inscripcion i
        INNER JOIN Curso c ON i.Cur_ID = c.Cur_ID
        INNER JOIN Docente d ON c.Doc_ID = d.Doc_ID
        INNER JOIN Estado eCur ON c.Est_ID = eCur.Est_ID
        WHERE i.Alu_ID = @aluID";

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@aluID", aluID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public static bool CursoTieneCupo(int curId)
        {
            using (SqlConnection cn = new SqlConnection(ClaseBase.Properties.Settings.Default.BDInstituto))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT Cur_Cupo FROM Curso WHERE Cur_ID = @curId", cn);

                cmd.Parameters.AddWithValue("@curId", curId);

                object result = cmd.ExecuteScalar();

                if (result != null)
                    return Convert.ToInt32(result) > 0;

                return false;
            }
        }

        public static void DescontarCupo(int curId)
        {
            using (SqlConnection cn = new SqlConnection(ClaseBase.Properties.Settings.Default.BDInstituto))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Curso SET Cur_Cupo = Cur_Cupo - 1 WHERE Cur_ID = @curId", cn);

                cmd.Parameters.AddWithValue("@curId", curId);
                cmd.ExecuteNonQuery();
            }
        }

        public static void AumentarCupo(int curId)
        {
            SqlConnection cn = new SqlConnection(ClaseBase.Properties.Settings.Default.BDInstituto);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "UPDATE Curso SET Cur_Cupo = Cur_Cupo + 1 WHERE Cur_ID = @id";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@id", curId);
            cmd.Connection = cn;

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
        }



    }

}
