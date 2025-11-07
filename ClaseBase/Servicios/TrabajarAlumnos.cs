using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace ClaseBase
{
    public class TrabajarAlumnos
    {
        // Traer un Alumno por ID
        public static Alumno TraerAlumno(int id)
        {
            Alumno alu = null;

            using (SqlConnection cn = new SqlConnection(ClaseBase.Properties.Settings.Default.BDInstituto))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Alumno WHERE Alu_ID=@id", cn);
                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    alu = new Alumno
                    {
                        Alu_ID = (int)dr["Alu_ID"],
                        Alu_DNI = dr["Alu_DNI"].ToString(),
                        Alu_Apellido = dr["Alu_Apellido"].ToString(),
                        Alu_Nombre = dr["Alu_Nombre"].ToString(),
                        Alu_Email = dr["Alu_Email"].ToString()
                    };
                }
            }
            return alu;
        }

        // Modifica un ALumno
        public static void ModificarAlumno(Alumno alu)
        {
            using (SqlConnection cn = new SqlConnection(ClaseBase.Properties.Settings.Default.BDInstituto))
            {
                try
                {
                    cn.Open();

                    string sql = @"UPDATE Alumno 
                           SET Alu_DNI = @dni, 
                               Alu_Apellido = @apellido, 
                               Alu_Nombre = @nombre, 
                               Alu_Email = @email
                           WHERE Alu_ID = @id";

                    SqlCommand cmd = new SqlCommand(sql, cn);

                    cmd.Parameters.AddWithValue("@dni", alu.Alu_DNI);
                    cmd.Parameters.AddWithValue("@apellido", alu.Alu_Apellido);
                    cmd.Parameters.AddWithValue("@nombre", alu.Alu_Nombre);
                    cmd.Parameters.AddWithValue("@email", alu.Alu_Email);
                    cmd.Parameters.AddWithValue("@id", alu.Alu_ID);

                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas == 0)
                    {
                        throw new Exception("No se encontró un alumno con el ID especificado.");
                    }
                }
                catch (SqlException sqlEx)
                {
                    throw new Exception("Error en la base de datos: " + sqlEx.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al modificar el alumno: " + ex.Message);
                }
            }
        }

        // Alta de un nuevo Alumno
        public static bool AltaAlumno(Alumno nuevoAlumno)
        {
            using (SqlConnection cn = new SqlConnection(ClaseBase.Properties.Settings.Default.BDInstituto))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Alumno (Alu_DNI, Alu_Apellido, Alu_Nombre, Alu_Email) " +
                    "VALUES (@DNI, @Apellido, @Nombre, @Email)", cn);

                cmd.Parameters.AddWithValue("@DNI", nuevoAlumno.Alu_DNI);
                cmd.Parameters.AddWithValue("@Apellido", nuevoAlumno.Alu_Apellido);
                cmd.Parameters.AddWithValue("@Nombre", nuevoAlumno.Alu_Nombre);
                cmd.Parameters.AddWithValue("@Email", nuevoAlumno.Alu_Email);

                int filas = cmd.ExecuteNonQuery();
                return filas > 0; 
            }
        }

    }
}
