using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace InmobiliariaAlaniz.Models;

	public class RepositorioPago
	{
		protected readonly string connectionString;
		public RepositorioPago()
		{
			connectionString = "Server=localhost;User=root;Password=;Database=inmobiliariaalaniz;SslMode=none";
		}

		public List<Pago> ObtenerTodos() {
			List<Pago> res = new List<Pago>();
			using (var conn = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT p.id, FechaPago, Importe, IdContrato, inm.Direccion, inq.Nombre, inq.Apellido
                FROM Pago p JOIN Contrato c ON(p.IdContrato = c.id)
                JOIN Inquilino inq ON(c.IdInquilino = inq.id)
                JOIN Inmueble inm ON(c.IdInmueble= inm.id)";
				using (var comm = new MySqlCommand(sql, conn))
				{
					conn.Open();
					var reader = comm.ExecuteReader();
					while (reader.Read())
					{
						res.Add(new Pago
                        {
                            Id = reader.GetInt32(0),
                            FechaPago = reader.GetDateTime(1),
                            Importe = reader.GetDecimal(2),
                            IdContrato = reader.GetInt32(3),
                            Contrato = new Contrato {
                                Id = reader.GetInt32(3),
								Inmueble = new Inmueble
                                {
                                    Direccion = reader.GetString(4),
                                },
                                Inquilino = new Inquilino
                                {
                                    Nombre = reader.GetString(5),
                                    Apellido = reader.GetString(6),
                                }
								},
                           
                        });
                        
					}
					conn.Close();
				}
			}
			return res;
		}
		public Pago ObtenerPorId(int id) {
			Pago pago = null;
			using (var conn = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT p.id, FechaPago, Importe, IdContrato, inm.Direccion, inq.Nombre, inq.Apellido
                FROM Pago p JOIN Contrato c ON(p.IdContrato = c.id)
                JOIN Inquilino inq ON(c.IdInquilino = inq.id)
                JOIN Inmueble inm ON(c.IdInmueble = inm.id)
                WHERE p.id=@id";
				using (var comm = new MySqlCommand(sql, conn))
				{
                    comm.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    comm.CommandType = System.Data.CommandType.Text;
					conn.Open();
					var reader = comm.ExecuteReader();
					if (reader.Read())
					{
						pago = new Pago
						{
							Id = reader.GetInt32(0),
                            FechaPago = reader.GetDateTime(1),
                            Importe = reader.GetDecimal(2),
                            IdContrato = reader.GetInt32(3),
                            Contrato = new Contrato {
                                Id = reader.GetInt32(3),
								Inmueble = new Inmueble
                                {
                                    Direccion = reader.GetString(4),
                                },
                                Inquilino = new Inquilino
                                {
                                    Nombre = reader.GetString(5),
                                    Apellido = reader.GetString(6),
                                }},
						};
					}
					conn.Close();
				}
			}
			
			return pago;
        }

		 public IList<Pago> PagosPorContrato(int id)
        {
            IList<Pago> lista = new List<Pago>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT Id, FechaPago, 
                Importe, IdContrato, FROM Pago WHERE IdContrato = @id";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    var reader = comm.ExecuteReader();

                    while (reader.Read()) {
                        Contrato contrato = new Contrato
                        {
                            Id = reader.GetInt32(1),
                        };

                        Pago pago = new Pago
                        {
                           Id = reader.GetInt32(0),
                            FechaPago = reader.GetDateTime(1),
                            Importe = reader.GetDecimal(2),
                            IdContrato = reader.GetInt32(3),
                            Contrato = new Contrato {
                                Id = reader.GetInt32(3),},
						};
                        lista.Add(pago);
                    }

                    conn.Close();
                }
			

            }  return lista;
        
    }

		public int Modificacion(Pago pago)
		{
			int res = -1;
			using (var conn = new MySqlConnection(connectionString))
			{
				string sql = @"UPDATE Pago SET FechaPago=@fechaPago, Importe=@importe, IdContrato=@idContrato 
                WHERE Id = @id";
				using (var comm = new MySqlCommand(sql, conn))
				{
					comm.CommandType = System.Data.CommandType.Text;
					comm.Parameters.AddWithValue("@fechaPago", pago.FechaPago);
					comm.Parameters.AddWithValue("@importe", pago.Importe);
					comm.Parameters.AddWithValue("@idContrato", pago.IdContrato);
					comm.Parameters.AddWithValue("@id", pago.Id);
					conn.Open();
					res = comm.ExecuteNonQuery();
					conn.Close();
				}
			}
			return res;
		}
		public int Alta(Pago pago)
		{
			int res = -1;
			using (var conn = new MySqlConnection(connectionString))
			{
				string sql = @"INSERT INTO Pago (FechaPago, Importe, IdContrato) 
				VALUES (@fechaPago, @importe, @idContrato);
					SELECT LAST_INSERT_ID();";
				using (var comm = new MySqlCommand(sql, conn))
				{
					comm.CommandType = System.Data.CommandType.Text;
					comm.Parameters.AddWithValue("@fechaPago", pago.FechaPago);
					comm.Parameters.AddWithValue("@importe", pago.Importe);
					comm.Parameters.AddWithValue("@idContrato", pago.IdContrato);
					
					comm.Parameters.AddWithValue("@id", pago.Id);
				
					conn.Open();
					res = Convert.ToInt32(comm.ExecuteScalar());
                    pago.Id = res;
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