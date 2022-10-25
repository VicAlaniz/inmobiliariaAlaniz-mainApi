using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace InmobiliariaAlaniz.Models;

	public class RepositorioInmueble
	{
		protected readonly string connectionString;
		public RepositorioInmueble()
		{
			connectionString = "Server=localhost;User=root;Password=;Database=inmobiliariaalaniz;SslMode=none";
		}

		public List<Inmueble> ObtenerTodos() {
			List<Inmueble> res = new List<Inmueble>();
			using (var conn = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT i.Id, Direccion, Uso, Tipo, CantAmbientes, 
				Coordenadas, Precio, PropietarioId, p.Nombre, p.Apellido 
				FROM Inmueble i INNER JOIN Propietario p ON i.PropietarioId = p.Id";
				using (var comm = new MySqlCommand(sql, conn))
				{
					conn.Open();
					var reader = comm.ExecuteReader();
					while (reader.Read())
					{
						res.Add(new Inmueble
                        {
                            Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Uso = reader.GetString(2),
                            Tipo = reader.GetString(3),
                            CantAmbientes = reader.GetInt32(4),
                            Coordenadas = reader.GetString(5),
                            Precio = reader.GetDecimal(6),
                            PropietarioId = reader.GetInt32(7),
                            Duenio = new Propietario {
                                Id = reader.GetInt32(7),
                                Nombre = reader.GetString(8),
                                Apellido = reader.GetString(9),
							}
                        });
					}
					conn.Close();
				}
			}
			return res;
		}
		public Inmueble ObtenerPorId(int id) {
			Inmueble inm = null;
			using (var conn = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT i.Id, Direccion, Uso, Tipo, CantAmbientes, 
				Coordenadas, Precio, PropietarioId, p.Nombre, p.Apellido 
				FROM Inmueble i INNER JOIN Propietario p ON i.PropietarioId = p.Id
				 WHERE i.Id = @id";
				using (var comm = new MySqlCommand(sql, conn))
				{
                    comm.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    comm.CommandType = System.Data.CommandType.Text;
					conn.Open();
					var reader = comm.ExecuteReader();
					if (reader.Read())
					{
						inm = new Inmueble
						{
							Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Uso = reader.GetString(2),
                            Tipo = reader.GetString(3),
                            CantAmbientes = reader.GetInt32(4),
                            Coordenadas = reader.GetString(5),
                            Precio = reader.GetDecimal(6),
                            PropietarioId = reader.GetInt32(7),
							Duenio = new Propietario {
                                Id = reader.GetInt32(7),
                                Nombre = reader.GetString(8),
                                Apellido = reader.GetString(9),
							}
						};
					}
					conn.Close();
				}
			}
			return inm;
        }
		public int Modificacion(Inmueble inm)
		{
			int res = -1;
			using (var conn = new MySqlConnection(connectionString))
			{
				string sql = @"UPDATE Inmueble SET Direccion=@direccion, Uso=@uso, Tipo=@tipo, 
				CantAmbientes=@cantAmbientes, Coordenadas=@coordenadas, Precio=@precio, PropietarioId=@propietarioId 
				WHERE Id = @id";
				using (var comm = new MySqlCommand(sql, conn))
				{
					comm.CommandType = System.Data.CommandType.Text;
					comm.Parameters.AddWithValue("@direccion", inm.Direccion);
					comm.Parameters.AddWithValue("@uso", inm.Uso);
					comm.Parameters.AddWithValue("@tipo", inm.Tipo);
					comm.Parameters.AddWithValue("@cantAmbientes", inm.CantAmbientes);
					comm.Parameters.AddWithValue("@coordenadas", inm.Coordenadas);
                    comm.Parameters.AddWithValue("@precio", inm.Precio);
                    comm.Parameters.AddWithValue("@propietarioId", inm.PropietarioId);
					comm.Parameters.AddWithValue("@id", inm.Id);
					conn.Open();
					res = comm.ExecuteNonQuery();
					conn.Close();
				}
			}
			return res;
		}
		public int Alta(Inmueble inm)
		{
			int res = -1;
			using (var conn = new MySqlConnection(connectionString))
			{
				string sql = @"INSERT INTO Inmueble (Direccion, Uso, Tipo, CantAmbientes, Coordenadas, Precio, PropietarioId) 
				VALUES (@direccion, @uso, @tipo, @cantAmbientes, @coordenadas, @precio, @propietarioId);
					SELECT LAST_INSERT_ID();";
				using (var comm = new MySqlCommand(sql, conn))
				{
					comm.CommandType = System.Data.CommandType.Text;
					comm.Parameters.AddWithValue("@direccion", inm.Direccion);
					comm.Parameters.AddWithValue("@uso", inm.Uso);
					comm.Parameters.AddWithValue("@tipo", inm.Tipo);
					comm.Parameters.AddWithValue("@cantAmbientes", inm.CantAmbientes);
					comm.Parameters.AddWithValue("@coordenadas", inm.Coordenadas);
                    comm.Parameters.AddWithValue("@precio", inm.Precio);
                    comm.Parameters.AddWithValue("@propietarioId", inm.PropietarioId);
				
					conn.Open();
					res = Convert.ToInt32(comm.ExecuteScalar());
                    inm.Id = res;
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
				string sql = $"DELETE FROM Inmueble WHERE Id = @id";
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