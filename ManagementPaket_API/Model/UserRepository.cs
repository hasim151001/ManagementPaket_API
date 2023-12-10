using System.Data.SqlClient;
using BCrypt.Net;

namespace ManagementPaket_API.Model
{
    public class UserRepository
    {
        private readonly string _connectingString;
        private readonly SqlConnection _connection;
        ResponseModel response = new ResponseModel();

        public UserRepository(IConfiguration configuration)
        {
            _connectingString = configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(_connectingString);
        }

        public List<UserModel> getAllData()
        {
            List<UserModel> usrList = new List<UserModel>();
            try
            {
                string query = "select * from pak_msuser";
                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    UserModel userModel = new UserModel
                    {
                        pak_id_user = Convert.ToInt32(reader["pak_id_user"]),
                        pak_nama_user = reader["pak_nama_user"].ToString(),
                        pak_username = reader["pak_username"].ToString(),
                        pak_password = reader["pak_password"].ToString(),
                        pak_foto = (byte[])reader["pak_foto"], // Mengonversi ke byte[] karena kolom pak_foto bertipe data image
                        pak_role = reader["pak_role"].ToString(),
                    };
                    usrList.Add(userModel);
                }
                reader.Close();
                _connection.Close();
                return usrList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
                return null;
            }

        }

        public UserModel Login(string username, string password)
        {
            try
            {
                using (var connection = new SqlConnection(_connectingString))
                {
                    connection.Open();
                    string query = "SELECT * FROM pak_msuser WHERE pak_username = @p1";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@p1", username);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string hashedPasswordFromDb = reader["pak_password"].ToString();
                                if (!string.IsNullOrEmpty(hashedPasswordFromDb))
                                {
                                    bool verify = BCrypt.Net.BCrypt.Verify(password, hashedPasswordFromDb);
                                    if (verify)
                                    {
                                        // Password is correct, check user role
                                        string userRole = reader["pak_role"].ToString();
                                        return new UserModel
                                        {
                                            pak_id_user = Convert.ToInt32(reader["pak_id_user"]),
                                            pak_nama_user = reader["pak_nama_user"].ToString(),
                                            pak_username = reader["pak_username"].ToString(),
                                            pak_password = hashedPasswordFromDb,
                                            pak_foto = (byte[])reader["pak_foto"],
                                            pak_role = userRole
                                        };
                                    }
                                }
                            }
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }



        public ResponseModel daftarUser(UserModel userModel)
        {
            try
            {
                // Hash the password using bcrypt
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userModel.pak_password, 12);

                string query = "INSERT INTO pak_msuser (pak_id_user, pak_nama_user, pak_username, pak_password, pak_foto, pak_role) VALUES (@p1, @p2, @p3, @p4, @p5, @p6)";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", userModel.pak_id_user); // Assuming pak_id_user is an auto-incremented column
                command.Parameters.AddWithValue("@p2", userModel.pak_nama_user);
                command.Parameters.AddWithValue("@p3", userModel.pak_username);
                command.Parameters.AddWithValue("@p4", hashedPassword);
                command.Parameters.AddWithValue("@p5", userModel.pak_foto); // Adjust based on how you handle image data
                command.Parameters.AddWithValue("@p6", userModel.pak_role);
                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();

                response.status = 200;
                response.messages = "User baru berhasil didaftarkan";
                response.data = userModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                response.status = 500;
                response.messages = "Terjadi kesalahan saat mendaftarkan user baru = " + ex.Message;
                response.data = null;
            }
            return response;
        }

    }
}
