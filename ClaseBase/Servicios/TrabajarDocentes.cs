using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ClaseBase
{
    public class TrabajarDocente
    {
        // Traer todos los docentes
        public static List<Docente> TraerDocentes()
        {
            List<Docente> lista = new List<Docente>();

            using (SqlConnection cn = new SqlConnection(ClaseBase.Properties.Settings.Default.BDInstituto))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Docente", cn);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Docente doc = new Docente
                    {
                        Doc_ID = (int)dr["Doc_ID"],
                        Doc_DNI = dr["Doc_DNI"].ToString(),
                        Doc_Apellido = dr["Doc_Apellido"].ToString(),
                        Doc_Nombre = dr["Doc_Nombre"].ToString(),
                        Doc_Email = dr["Doc_Email"].ToString()
                    };
                    lista.Add(doc);
                }
            }
            return lista;
        }

        // Traer un docente por ID
        public static Docente TraerDocente(int id)
        {
            Docente doc = null;

            using (SqlConnection cn = new SqlConnection(ClaseBase.Properties.Settings.Default.BDInstituto))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Docente WHERE Doc_ID=@id", cn);
                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    doc = new Docente
                    {
                        Doc_ID = (int)dr["Doc_ID"],
                        Doc_DNI = dr["Doc_DNI"].ToString(),
                        Doc_Apellido = dr["Doc_Apellido"].ToString(),
                        Doc_Nombre = dr["Doc_Nombre"].ToString(),
                        Doc_Email = dr["Doc_Email"].ToString()
                    };
                }
            }
            return doc;
        }

        // Alta de un nuevo docente
        public static bool AltaDocente(Docente nuevoDocente)
        {
            using (SqlConnection cn = new SqlConnection(ClaseBase.Properties.Settings.Default.BDInstituto))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Docente (Doc_DNI, Doc_Apellido, Doc_Nombre, Doc_Email) " +
                    "VALUES (@DNI, @Apellido, @Nombre, @Email)", cn);

                cmd.Parameters.AddWithValue("@DNI", nuevoDocente.Doc_DNI);
                cmd.Parameters.AddWithValue("@Apellido", nuevoDocente.Doc_Apellido);
                cmd.Parameters.AddWithValue("@Nombre", nuevoDocente.Doc_Nombre);
                cmd.Parameters.AddWithValue("@Email", nuevoDocente.Doc_Email);

                int filas = cmd.ExecuteNonQuery();
                return filas > 0;
            }
        }
    }
}
