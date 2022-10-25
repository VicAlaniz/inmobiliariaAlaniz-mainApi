
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace InmobiliariaAlaniz.Models;

	public class RepositorioPropietario
	{
		protected readonly string connectionString;
		public RepositorioPropietario()
		{
			connectionString = "Server=localhost;User=root;Password=;Database=inmobiliariaalaniz;SslMode=none";
		}

		public List<Propietario> ObtenerTodos()
		{
			List<Propietario> res = new List<Propietario>();
			using (var conn = new MySqlConnection(connectionString))
			{
				string sql = "SELECT Id, Nombre, Apellido, Dni, Telefono, Email FROM Propietario";
				using (var comm = new MySqlCommand(sql, conn))
				{
					conn.Open();
					var reader = comm.ExecuteReader();
					while (reader.Read())
					{
						res.Add(new Propietario
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
		public Propietario ObtenerPorId(int id)
		{
			Propietario prop = null;
			using (var conn = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT Id, Nombre, Apellido, Dni, Telefono, Email FROM Propietario " +
					$" WHERE Id=@id";
				using (var comm = new MySqlCommand(sql, conn))
				{
                    comm.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    comm.CommandType = CommandType.Text;
					conn.Open();
					var reader = comm.ExecuteReader();
					if (reader.Read())
					{
						prop = new Propietario
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
			return prop;
        }
		public int Modificacion(Propietario prop)
		{
			int res = -1;
			using (var conn = new MySqlConnection(connectionString))
			{
				string sql = $"UPDATE Propietario SET Nombre=@nombre, Apellido=@apellido, Dni=@dni, Telefono=@telefono, Email=@email WHERE Id = @id";
				using (var comm = new MySqlCommand(sql, conn))
				{
					comm.CommandType = CommandType.Text;
					comm.Parameters.AddWithValue("@nombre", prop.Nombre);
					comm.Parameters.AddWithValue("@apellido", prop.Apellido);
					comm.Parameters.AddWithValue("@dni", prop.Dni);
					comm.Parameters.AddWithValue("@telefono", prop.Telefono);
					comm.Parameters.AddWithValue("@email", prop.Email);
					comm.Parameters.AddWithValue("@id", prop.Id);
					conn.Open();
					res = comm.ExecuteNonQuery();
					conn.Close();
				}
			}
			return res;
		}
		public int Alta(Propietario prop)
		{
			int res = -1;
			using (var conn = new MySqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Propietario (Nombre, Apellido, Dni, Telefono, Email) " +
					$"VALUES (@nombre, @apellido, @dni, @telefono, @email);" +
					"SELECT LAST_INSERT_ID();";
				using (var comm = new MySqlCommand(sql, conn))
				{
					comm.CommandType = CommandType.Text;
					comm.Parameters.AddWithValue("@nombre", prop.Nombre);
					comm.Parameters.AddWithValue("@apellido", prop.Apellido);
					comm.Parameters.AddWithValue("@dni", prop.Dni);
					comm.Parameters.AddWithValue("@telefono", prop.Telefono);
					comm.Parameters.AddWithValue("@email", prop.Email);
				
					conn.Open();
					res = Convert.ToInt32(comm.ExecuteScalar());
                    prop.Id = res;
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
				string sql = $"DELETE FROM Propietario WHERE Id = @id";
				using (var comm = new MySqlCommand(sql, conn))
				{
					comm.CommandType = CommandType.Text;
					comm.Parameters.AddWithValue("@id", id);
					conn.Open();
					res = comm.ExecuteNonQuery();
					conn.Close();
				}
			}
			return res;
		}
	}