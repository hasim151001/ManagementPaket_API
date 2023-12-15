using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace ManagementPaket_API.Model
{
    public class JenisPaket
    {
        //alamat koneksi database 
        private readonly string _connectionString;

        //koneksi sql
        private readonly SqlConnection _connection;

        //konstruktor kelas yang akan kita gunakan untuk mengsetup connection string
        public JenisPaket(IConfiguration configuration)
        {
            //mengambil konfigurasi conection string dari appsettings.json
            _connectionString = configuration.GetConnectionString("DefaultConnection");

            //koneksi sql menggunakan connection string
            _connection = new SqlConnection(_connectionString);
        }
        public List<JenisPaketModel> GetAllData()
        {
            List<JenisPaketModel> jenispaketList = new List<JenisPaketModel>();
            try
            {
                string query = "SELECT * FROM pak_msjenispaket WHERE pak_status_jenis = 1 ";
                using SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JenisPaketModel jenispaket = new JenisPaketModel
                    {
                        pak_id_jenis = Convert.ToInt32(reader["pak_id_jenis"]),
                        pak_nama_jenis = reader["pak_nama_jenis"].ToString(),
                        pak_status_jenis = Convert.ToInt32(reader["pak_status_jenis"]),
                    };
                    jenispaketList.Add(jenispaket);
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

            return jenispaketList;
        }


        public JenisPaketModel GetData(int pak_id_jenis)
        {
            JenisPaketModel jenispaketModel = new JenisPaketModel();
            try
            {
                string query = "SELECT * FROM pak_msjenispaket WHERE pak_id_jenis = @p1";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@p1", pak_id_jenis);
                    _connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            jenispaketModel.pak_id_jenis = Convert.ToInt32(reader["pak_id_jenis"]);
                            jenispaketModel.pak_nama_jenis = reader["pak_nama_jenis"].ToString();
                            jenispaketModel.pak_status_jenis = Convert.ToInt32(reader["pak_status_jenis"]);

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
            return jenispaketModel;
        }


        public void InsertData(string pak_nama_jenis)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("sp_addJenisPaket", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@pak_nama_jenis", pak_nama_jenis);

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

        public void UpdateData(JenisPaketModel jenispaketModel)
        {
            try
            {
                string spName = "sp_updateJenisPaket";

                using (SqlCommand command = new SqlCommand(spName, _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@pak_id_jenis", jenispaketModel.pak_id_jenis);
                    command.Parameters.AddWithValue("@pak_nama_jenis", jenispaketModel.pak_nama_jenis);
                    command.Parameters.AddWithValue("@pak_status_jenis", jenispaketModel.pak_status_jenis);

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


        public void DeleteData(int pak_id_jenis)
        {
            try
            {
                string spName = "sp_disableJenisPaket";

                using (SqlCommand command = new SqlCommand(spName, _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@pak_id_jenis", pak_id_jenis);

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
