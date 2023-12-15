using System;
using System.Data;
using System.Data.SqlClient;
namespace ManagementPaket_API.Model
{
    public class TrsPengambilan
    {
        //alamat koneksi database 
        private readonly string _connectionString;

        //koneksi sql
        private readonly SqlConnection _connection;

        //konstruktor kelas yang akan kita gunakan untuk mengsetup connection string
        public TrsPengambilan(IConfiguration configuration)
        {
            //mengambil konfigurasi conection string dari appsettings.json
            _connectionString = configuration.GetConnectionString("DefaultConnection");

            //koneksi sql menggunakan connection string
            _connection = new SqlConnection(_connectionString);
        }
        public List<TrsPengambilanModel> GetAllData()
        {
            List<TrsPengambilanModel> trspengambilanList = new List<TrsPengambilanModel>();
            try
            {
                string query = "SELECT * FROM pak_mstransaksipengambilan";
                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    TrsPengambilanModel trspengambilan = new TrsPengambilanModel
                    {
                        pak_id_transaksi = Convert.ToInt32(reader["pak_id_transaksi"]),
                        pak_id = Convert.ToInt32(reader["pak_id"]),
                        pak_id_pengambil = reader["pak_id_pengambil"].ToString(),
                        pak_nama_pengambil = reader["pak_nama_pengambil"].ToString(),
                        pak_tanggal_pengambilan = Convert.ToDateTime(reader["pak_tanggal_pengambilan"]),
                        pak_foto_pengambil = reader["pak_foto_pengambil"].ToString(),
                        pak_status_transaksi = Convert.ToInt32(reader["pak_status_transaksi"]),

                    };
                    trspengambilanList.Add(trspengambilan);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _connection?.Close();
            }

            return trspengambilanList;
        }



        public TrsPengambilanModel GetData(int pak_id_transaksi)
        {
            TrsPengambilanModel transaksiModel = new TrsPengambilanModel();
            try
            {
                string query = "SELECT * FROM pak_mstransaksipengambilan WHERE pak_id_transaksi = @p1";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@p1", pak_id_transaksi);
                    _connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            transaksiModel.pak_id_transaksi = Convert.ToInt32(reader["pak_id_transaksi"]);
                            transaksiModel.pak_id = Convert.ToInt32(reader["pak_id"]);
                            transaksiModel.pak_id_pengambil = reader["pak_id_pengambil"].ToString();
                            transaksiModel.pak_nama_pengambil = reader["pak_nama_pengambil"].ToString();
                            transaksiModel.pak_tanggal_pengambilan = Convert.ToDateTime(reader["pak_tanggal_pengambilan"]);
                            transaksiModel.pak_foto_pengambil = reader["pak_foto_pengambil"].ToString();
                            transaksiModel.pak_status_transaksi = Convert.ToInt32(reader["pak_status_transaksi"]);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _connection.Close();
            }
            return transaksiModel;
        }


        public void InsertData(int pak_id, string pak_id_pengambil, string pak_nama_pengambil, string pak_foto_pengambil)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("sp_addTrsPengambilan", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@pak_id", pak_id);
                    command.Parameters.AddWithValue("@pak_id_pengambil", pak_id_pengambil);
                    command.Parameters.AddWithValue("@pak_nama_pengambil", pak_nama_pengambil);
                    command.Parameters.AddWithValue("@pak_foto_pengambil", pak_foto_pengambil);

                    _connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _connection.Close();
            }
        }
        public List<TrsPengambilanModel> GetViewTrsPengambilan()
        {
            List<TrsPengambilanModel> trspengambilanList = new List<TrsPengambilanModel>();
            try
            {
                string query = "SELECT * FROM vw_pak_mstrspengambilan";
                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    TrsPengambilanModel trspengambilan = new TrsPengambilanModel();
                    trspengambilan.pak_id_transaksi = Convert.ToInt32(reader["pak_id_transaksi"]);
                    trspengambilan.pak_nama_pengambil = reader["pak_nama_pengambil"].ToString();
                    trspengambilan.pak_nama_pemilik = reader["pak_nama_pemilik"].ToString();
                    trspengambilan.pak_nama_pengirim = reader["pak_nama_pengirim"].ToString();
                    if (reader["pak_tanggal_pengambilan"] != DBNull.Value)
                    {
                        if (DateTime.TryParse(reader["pak_tanggal_pengambilan"].ToString(), out DateTime tempDate))
                        {
                            trspengambilan.pak_tanggal_pengambilan = tempDate;
                        }
                        else
                        {
                            // Handle error or log it
                            Console.WriteLine("Failed to parse date");
                        }
                    }
                    else
                    {
                        trspengambilan.pak_tanggal_pengambilan = null;
                    }

                    trspengambilan.pak_foto_pengambil = reader["pak_foto_pengambil"].ToString();
                    trspengambilan.pak_status_transaksi = Convert.ToInt32(reader["pak_status_transaksi"]);

                    trspengambilanList.Add(trspengambilan);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                _connection?.Close();
            }

            return trspengambilanList;
        }
    }
}
