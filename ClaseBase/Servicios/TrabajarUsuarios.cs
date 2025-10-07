using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ClaseBase.Servicios
{
    public class TrabajarUsuarios
    {
        // Traer todos los Roles
        public static DataTable getRoles()
        {
            SqlConnection cn = new SqlConnection(ClaseBase.Properties.Settings.Default.BDInstituto);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM Roles";

            cmd.CommandType = CommandType.Text;
            cmd.Connection = cn;
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            cn.Open();
            da.Fill(dt);

            cn.Close();
            return dt;
        }

        // Alta de un nuevo usuario
        public static bool AltaUsuario(Usuario usuario)
        {
            using (SqlConnection cn = new SqlConnection(ClaseBase.Properties.Settings.Default.BDInstituto))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Usuario (Usu_NombreUsuario, Usu_Contraseña, Usu_ApellidoNombre, Rol_ID) " +
                    "VALUES (@nombreUsuario, @contrasenia, @apellidoNombre, @rol)", cn);

                cmd.Parameters.AddWithValue("@nombreUsuario", usuario.Usu_NombreUsuario);
                cmd.Parameters.AddWithValue("@contrasenia", usuario.Usu_Contraseña);
                cmd.Parameters.AddWithValue("@apellidoNombre", usuario.Usu_ApellidoNombre);
                cmd.Parameters.AddWithValue("@rol", usuario.Rol_ID);

                int filas = cmd.ExecuteNonQuery();
                return filas > 0;
            }
        }

        // Autenticar un usuario
        public static Usuario AutenticarUsuario(string nombreUsuario, string password)
        {
            SqlConnection cn = new SqlConnection(Properties.Settings.Default.BDInstituto);
            SqlCommand cmd = new SqlCommand(
                @"SELECT Usu_Id, Usu_NombreUsuario, Usu_ApellidoNombre, Rol_ID 
        FROM Usuario 
        WHERE Usu_NombreUsuario = @usuario AND Usu_Contraseña = @password", cn);

            cmd.Parameters.AddWithValue("@usuario", nombreUsuario);
            cmd.Parameters.AddWithValue("@password", password);

            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                Usuario usuario = new Usuario
                {
                    Usu_ID = Convert.ToInt32(dr["Usu_Id"]),
                    Usu_NombreUsuario = dr["Usu_NombreUsuario"].ToString(),
                    Usu_ApellidoNombre = dr["Usu_ApellidoNombre"].ToString(),
                    Rol_ID = Convert.ToInt32(dr["Rol_ID"])
                };
                cn.Close();
                return usuario;
            }
            cn.Close();
            return null;

        }
    }
}
