using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace InmobiliariaAlaniz.Models;

	public class RepositorioContrato
	{
		protected readonly string connectionString;
		public RepositorioContrato()
		{
			connectionString = "Server=localhost;User=root;Password=;Database=inmobiliariaalaniz;SslMode=none";
		}

		public List<Contrato> ObtenerTodos() {
			List<Contrato> res = new List<Contrato>();
			using (var conn = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT c.Id, IdInquilino, IdInmueble, FechaInicio, 
                FechaFin, inm.Direccion, inq.Apellido, inq.Nombre 
				FROM Contrato c JOIN Inmueble inm ON(c.IdInmueble = inm.Id) 
				JOIN Inquilino inq ON(c.IdInquilino = inq.Id)";
				using (var comm = new MySqlCommand(sql, conn))
				{
					conn.Open();
					var reader = comm.ExecuteReader();
					while (reader.Read())
					{
						res.Add(new Contrato
                        {
                            Id = reader.GetInt32(0),
                            IdInquilino = reader.GetInt32(1),
                            Inquilino = new Inquilino {
                                Id = reader.GetInt32(1),
                                Nombre = reader.GetString(7),
                                Apellido = reader.GetString(6),
                            },
                            IdInmueble = reader.GetInt32(2),
                            Inmueble = new Inmueble {
                                Id = reader.GetInt32(2),
                                Direccion = reader.GetString(5),
                            },
                            FechaInicio = reader.GetDateTime(3),
                            FechaFin = reader.GetDateTime(4),
                        });
					}
					conn.Close();
				}
			}
			return res;
		}
		public Contrato ObtenerPorId(int id) {
			Contrato contrato = null;
			using (var conn = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT c.Id, IdInquilino, IdInmueble, FechaInicio, 
                FechaFin, inm.Direccion, inq.Apellido, inq.Nombre 
				FROM Contrato c JOIN Inmueble inm ON(c.IdInmueble = inm.Id) 
				JOIN Inquilino inq ON(c.IdInquilino = inq.Id)
				WHERE c.Id = @id";
				using (var comm = new MySqlCommand(sql, conn))
				{
                    comm.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    comm.CommandType = System.Data.CommandType.Text;
					conn.Open();
					var reader = comm.ExecuteReader();
					if (reader.Read())
					{
						contrato = new Contrato
						{
							Id = reader.GetInt32(0),
                            IdInquilino = reader.GetInt32(1),
                            Inquilino = new Inquilino {
                                Id = reader.GetInt32(1),
                                Nombre = reader.GetString(7),
                                Apellido = reader.GetString(6),
                            },
                            IdInmueble = reader.GetInt32(2),
                            Inmueble = new Inmueble {
                                Id = reader.GetInt32(2),
                                Direccion = reader.GetString(5),
                            },
                            FechaInicio = reader.GetDateTime(3),
                            FechaFin = reader.GetDateTime(4),
						};
					}
					conn.Close();
				}
			}
			return contrato;
        }
		public int Modificacion(Contrato contrato)
		{
			int res = -1;
			using (var conn = new MySqlConnection(connectionString))
			{
				string sql = @"UPDATE Contrato SET IdInquilino=@idInquilino, IdInmueble=@idInmueble, FechaInicio=@fechaInicio, 
				FechaFin=@fechaFin WHERE Id = @id";
				using (var comm = new MySqlCommand(sql, conn))
				{
					comm.CommandType = System.Data.CommandType.Text;
					comm.Parameters.AddWithValue("@idInquilino", contrato.IdInquilino);
					comm.Parameters.AddWithValue("@idInmueble", contrato.IdInmueble);
					comm.Parameters.AddWithValue("@fechaInicio", contrato.FechaInicio);
					comm.Parameters.AddWithValue("@fechaFin", contrato.FechaFin);
					comm.Parameters.AddWithValue("@id", contrato.Id);
					conn.Open();
					res = comm.ExecuteNonQuery();
					conn.Close();
				}
			}
			return res;
		}
		public int Alta(Contrato contrato)
		{
			int res = -1;
			using (var conn = new MySqlConnection(connectionString))
			{
				string sql = @"INSERT INTO Contrato (IdInquilino, IdInmueble, FechaInicio, FechaFin) 
				VALUES (@idInquilino, @idInmueble, @fechaInicio, @fechaFin);
					SELECT LAST_INSERT_ID();";
				using (var comm = new MySqlCommand(sql, conn))
				{
					comm.CommandType = System.Data.CommandType.Text;
					comm.Parameters.AddWithValue("@idInquilino", contrato.IdInquilino);
					comm.Parameters.AddWithValue("@idInmueble", contrato.IdInmueble);
					comm.Parameters.AddWithValue("@fechaInicio", contrato.FechaInicio);
					comm.Parameters.AddWithValue("@fechaFin", contrato.FechaFin);
					comm.Parameters.AddWithValue("@id", contrato.Id);
				
					conn.Open();
					res = Convert.ToInt32(comm.ExecuteScalar());
                    contrato.Id = res;
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
				string sql = $"DELETE FROM Contrato WHERE Id = @id";
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