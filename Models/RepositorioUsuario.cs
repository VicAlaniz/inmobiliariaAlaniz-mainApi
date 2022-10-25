using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace InmobiliariaAlaniz.Models;

	public class RepositorioUsuario
	{
		protected readonly string connectionString;
		public RepositorioUsuario()
		{
			connectionString = "Server=localhost;User=root;Password=;Database=inmobiliariaalaniz;SslMode=none";
		}

		public List<Usuario> ObtenerTodos()
		{
			List<Usuario> res = new List<Usuario>();
			using (var conn = new MySqlConnection(connectionString))
			{
				string sql = "SELECT Id, Nombre, Apellido, Email, Clave, Avatar, Rol FROM Usuario";
				using (var comm = new MySqlCommand(sql, conn))
				{
					conn.Open();
					var reader = comm.ExecuteReader();
					while (reader.Read())
					{
						res.Add(new Usuario
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Email = reader.GetString(3),
                            Clave = reader.GetString(4),
                            Avatar = reader["Avatar"].ToString(),
                            Rol = reader.GetInt32(6),
                        });
					}
					conn.Close();
				}
			}
			return res;
		}
		public Usuario ObtenerPorId(int id)
		{
			Usuario usua = null;
			using (var conn = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT Id, Nombre, Apellido, Email, Clave, Avatar, Rol FROM Usuario" +
					$" WHERE Id=@id";
				using (var comm = new MySqlCommand(sql, conn))
				{
                    comm.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    comm.CommandType = System.Data.CommandType.Text;
					conn.Open();
					var reader = comm.ExecuteReader();
					if (reader.Read())
					{
						usua = new Usuario
						{
							Id = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
                            Email = reader.GetString(3),
                            Clave = reader.GetString(4),
                            Avatar = reader["Avatar"].ToString(),
                            Rol = reader.GetInt32(6),
							
						};
					}
					conn.Close();
				}
			}
			return usua;
        }
		public int Modificacion(Usuario usu)
		{
			int res = -1;
			using (var conn = new MySqlConnection(connectionString))
			{
				string sql = $"UPDATE Usuario SET Nombre=@nombre, Apellido=@apellido, Email=@email, Avatar=@avatar, Rol=@rol WHERE Id = @id";
				using (var comm = new MySqlCommand(sql, conn))
				{
					comm.CommandType = System.Data.CommandType.Text;
					comm.Parameters.AddWithValue("@nombre", usu.Nombre);
					comm.Parameters.AddWithValue("@apellido", usu.Apellido);
					comm.Parameters.AddWithValue("@email", usu.Email);
		
					if(String.IsNullOrEmpty(usu.Avatar))
					comm.Parameters.AddWithValue("@avatar", DBNull.Value);
                    else comm.Parameters.AddWithValue("@avatar", usu.Avatar);

                    comm.Parameters.AddWithValue("@rol", usu.Rol);
					comm.Parameters.AddWithValue("@id", usu.Id);
					conn.Open();
					res = comm.ExecuteNonQuery();
					conn.Close();
				}
			}
			return res;
		}
		     public int ModificarClave(int id, CambiarClave p)
        {
            int res = -1;
            using (var conn = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Usuario SET Clave=@clave 
                WHERE id = @id";
                using (var comm = new MySqlCommand(sql, conn))
                {
                    comm.CommandType = System.Data.CommandType.Text;
                    comm.Parameters.AddWithValue("@id", id);
                    comm.Parameters.AddWithValue("@clave", p.PassConfirmada);
                    conn.Open();
                    res = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
		public int Alta(Usuario usu)
		{
			int res = -1;
			using (var conn = new MySqlConnection(connectionString))
			{
				string sql = @"INSERT INTO Usuario (Nombre, Apellido, Email, Clave, Avatar, Rol)
					VALUES (@nombre, @apellido, @email, @clave, @avatar, @rol); 
					SELECT LAST_INSERT_ID();";
				using (var comm = new MySqlCommand(sql, conn))
				{
					comm.CommandType = System.Data.CommandType.Text;
					comm.Parameters.AddWithValue("@nombre", usu.Nombre);
					comm.Parameters.AddWithValue("@apellido", usu.Apellido);
					comm.Parameters.AddWithValue("@email", usu.Email);
					comm.Parameters.AddWithValue("@clave", usu.Clave);
                    if(String.IsNullOrEmpty(usu.Avatar))
					comm.Parameters.AddWithValue("@avatar", DBNull.Value);
                    else comm.Parameters.AddWithValue("@avatar", usu.Avatar);

                    comm.Parameters.AddWithValue("@rol", usu.Rol);
				
					conn.Open();
					res = Convert.ToInt32(comm.ExecuteScalar());
                    usu.Id = res;
                    conn.Close();
				}
			}
			return res;
		}
		public int Baja(int id)
		{
			int res = -1;
			using (var conn = new MySqlConnection(connectionString))
			{
				string sql = $"DELETE FROM Usuario WHERE Id = @id";
				using (var comm = new MySqlCommand(sql, conn))
				{
					comm.CommandType = System.Data.CommandType.Text;
					comm.Parameters.AddWithValue("@id", id);
					conn.Open();
					res = comm.ExecuteNonQuery();
					conn.Close();
				}
			}
			return res;
		}
         public Usuario ObtenerPorEmail(string email)
        {
            Usuario usu = null;
            using (var conn = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT Id, Nombre, Apellido, Email, Clave, Avatar, Rol FROM Usuario" +
                    $" WHERE Email=@email";
                using (var comm = new MySqlCommand(sql, conn))
                {
                    comm.CommandType = System.Data.CommandType.Text;
                    comm.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    if (reader.Read())
                    {
                        usu = new Usuario
                        {
                            Id = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
                            Email = reader.GetString(3),
                            Clave = reader.GetString(4),
                            Avatar = reader["Avatar"].ToString(),
                            Rol = reader.GetInt32(6),
						};
                    }
                    conn.Close();
                }
            }
            return usu;
        }
    
	}