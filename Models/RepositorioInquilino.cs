
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace InmobiliariaAlaniz.Models;

	public class RepositorioInquilino
	{
		protected readonly string connectionString;
		public RepositorioInquilino()
		{
			connectionString = "Server=localhost;User=root;Password=;Database=inmobiliariaalaniz;SslMode=none";
		}

		public List<Inquilino> ObtenerTodos()
		{
			List<Inquilino> res = new List<Inquilino>();
			using (var conn = new MySqlConnection(connectionString))
			{
				string sql = "SELECT Id, Nombre, Apellido, Dni, Telefono, Email FROM Inquilino";
				using (var comm = new MySqlCommand(sql, conn))
				{
					conn.Open();
					var reader = comm.ExecuteReader();
					while (reader.Read())
					{
						res.Add(new Inquilino
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetString(3),
                            Telefono = reader.GetString(4),
                            Email = reader.GetString(5),
                        });
					}
					conn.Close();
				}
			}
			return res;
		}
		public Inquilino ObtenerPorId(int id)
		{
			Inquilino inc = null;
			using (var conn = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT Id, Nombre, Apellido, Dni, Telefono, Email FROM Inquilino " +
					$" WHERE Id=@id";
				using (var comm = new MySqlCommand(sql, conn))
				{
                    comm.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    comm.CommandType = System.Data.CommandType.Text;
					conn.Open();
					var reader = comm.ExecuteReader();
					if (reader.Read())
					{
						inc = new Inquilino
						{
							Id = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Dni = reader.GetString(3),
							Telefono = reader.GetString(4),
							Email = reader.GetString(5),
							
						};
					}
					conn.Close();
				}
			}
			return inc;
        }
		public int Modificacion(Inquilino inc)
		{
			int res = -1;
			using (var conn = new MySqlConnection(connectionString))
			{
				string sql = $"UPDATE Inquilino SET Nombre=@nombre, Apellido=@apellido, Dni=@dni, Telefono=@telefono, Email=@email WHERE Id = @id";
				using (var comm = new MySqlCommand(sql, conn))
				{
					comm.CommandType = System.Data.CommandType.Text;
					comm.Parameters.AddWithValue("@nombre", inc.Nombre);
					comm.Parameters.AddWithValue("@apellido", inc.Apellido);
					comm.Parameters.AddWithValue("@dni", inc.Dni);
					comm.Parameters.AddWithValue("@telefono", inc.Telefono);
					comm.Parameters.AddWithValue("@email", inc.Email);
					comm.Parameters.AddWithValue("@id", inc.Id);
					conn.Open();
					res = comm.ExecuteNonQuery();
					conn.Close();
				}
			}
			return res;
		}
		public int Alta(Inquilino inc)
		{
			int res = -1;
			using (var conn = new MySqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Inquilino (Nombre, Apellido, Dni, Telefono, Email) " +
					$"VALUES (@nombre, @apellido, @dni, @telefono, @email); " +
					"SELECT LAST_INSERT_ID();";
				using (var comm = new MySqlCommand(sql, conn))
				{
					comm.CommandType = System.Data.CommandType.Text;
					comm.Parameters.AddWithValue("@nombre", inc.Nombre);
					comm.Parameters.AddWithValue("@apellido", inc.Apellido);
					comm.Parameters.AddWithValue("@dni", inc.Dni);
					comm.Parameters.AddWithValue("@telefono", inc.Telefono);
					comm.Parameters.AddWithValue("@email", inc.Email);
				
					conn.Open();
					res = Convert.ToInt32(comm.ExecuteScalar());
                    inc.Id = res;
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
				string sql = $"DELETE FROM Inquilino WHERE Id = @id";
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
	}