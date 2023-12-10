using System;
using System.Data;
using System.Data.SqlClient;
namespace ManagementPaket_API.Model
{
    public class Paket
    {
        //alamat koneksi database 
        private readonly string _connectionString;

        //koneksi sql
        private readonly SqlConnection _connection;

        //konstruktor kelas yang akan kita gunakan untuk mengsetup connection string
        public Paket(IConfiguration configuration)
        {
            //mengambil konfigurasi conection string dari appsettings.json
            _connectionString = configuration.GetConnectionString("DefaultConnection");

            //koneksi sql menggunakan connection string
            _connection = new SqlConnection(_connectionString);
        }
        public List<PaketModel> GetAllData()
        {
            List<PaketModel> paketList = new List<PaketModel>();
            try
            {
                string query = "SELECT * FROM vw_pak_mspaket";
                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    PaketModel paket = new PaketModel
                    {
                        pak_id = reader["pak_id"].ToString(),
                        pak_nama_pemilik = reader["pak_nama_pemilik"].ToString(),
                        pak_id_jenis = reader["pak_id_jenis"].ToString(),
                        pak_tanggal_sampai = Convert.ToDateTime(reader["pak_tanggal_sampai"]),
                        pak_nama_pengirim = reader["pak_nama_pengirim"].ToString(),
                        pak_status = reader["pak_status"].ToString(),
                    };
                    paketList.Add(paket);
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

            return paketList;
        }



        public PaketModel GetData(string pak_id)
        {
            PaketModel paketModel = new PaketModel();
            try
            {
                string query = "SELECT * FROM vw_pak_mspaket WHERE pak_id = @p1";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@p1", pak_id);
                    _connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            paketModel.pak_id = reader["pak_id"].ToString();
                            paketModel.pak_nama_pemilik = reader["pak_nama_pemilik"].ToString();
                            paketModel.pak_id_jenis = reader["pak_id_jenis"].ToString();
                            paketModel.pak_tanggal_sampai = Convert.ToDateTime(reader["pak_tanggal_sampai"]);
                            paketModel.pak_nama_pengirim = reader["pak_nama_pengirim"].ToString();
                            paketModel.pak_status = reader["pak_status"].ToString();
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
            return paketModel;
        }


        public void InsertData(string pak_nama_pemilik, string pak_id_jenis, string pak_nama_pengirim)
        {
            try
            {
                // Call the stored procedure to insert new paket
                using (SqlCommand command = new SqlCommand("sp_addPaket", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@pak_nama_pemilik", pak_nama_pemilik);
                    command.Parameters.AddWithValue("@pak_id_jenis", pak_id_jenis);
                    //  command.Parameters.AddWithValue("@pak_tanggal_sampai", pak_tanggal_sampai);
                    command.Parameters.AddWithValue("@pak_nama_pengirim", pak_nama_pengirim);

                    _connection.Open();
                    command.ExecuteNonQuery();
                    _connection.Close();

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
        public void UpdateData(PaketModel paketModel)
        {
            try
            {
                string spName = "sp_updatePaket";

                using (SqlCommand command = new SqlCommand(spName, _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@pak_id", paketModel.pak_id);
                    command.Parameters.AddWithValue("@pak_nama_pemilik", paketModel.pak_nama_pemilik);
                    command.Parameters.AddWithValue("@pak_id_jenis", paketModel.pak_id_jenis);
                    command.Parameters.AddWithValue("@pak_tanggal_sampai", paketModel.pak_tanggal_sampai);
                    command.Parameters.AddWithValue("@pak_nama_pengirim", paketModel.pak_nama_pengirim);

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


        public void DeleteData(string pak_id)
        {
            try
            {
                string spName = "sp_deletePaket";

                using (SqlCommand command = new SqlCommand(spName, _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@pak_id", pak_id);

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


    }
}
