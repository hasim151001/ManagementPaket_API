using Microsoft.AspNetCore.Mvc;
using ManagementPaket_API.Model;

namespace ManagementPaket_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly UserRepository _userRepo;

        public UserController(IConfiguration configuration)
        {
            _userRepo = new UserRepository(configuration);
        }

        [HttpGet("/GetAllUser", Name = "GetAllUser")]
        public IActionResult GetAllUser()
        {
            var user = _userRepo.getAllData();
            try
            {
                if (user != null)
                {
                    return Ok(new { Status = 200, Messages = "Berhasil Menampilkan Data User", Data = user });
                }
                else
                {
                    return StatusCode(404, new { Status = 404, Messages = "Data User Tidak Ditemukan", Data = user });
                }
            }
            catch (Exception ex)
            {
                // Tangani kesalahan umum
                return StatusCode(500, new { Status = 500, Messages = "Terjadi Kesalahan Saat Menampilkan Data User", Data = ex.Message });
            }
        }

        [HttpGet("/Login", Name = "loginUser")]
        public IActionResult loginUser(string pak_username, string pak_password)
        {
            UserModel user = _userRepo.Login(pak_username, pak_password);

            try
            {
                if (user != null)
                {
                    bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(pak_password, user.pak_password);

                    if (isPasswordCorrect)
                    {
                        // Password is correct, login successful
                        HttpContext.Session.SetString("pak_id_user", user.pak_id_user.ToString());
                        HttpContext.Session.SetString("pak_nama_user", user.pak_nama_user);
                        HttpContext.Session.SetString("pak_role", user.pak_role);

                        return Ok(new { Status = 200, Messages = "Login berhasil", Data = user });
                    }
                    else
                    {
                        // Password is incorrect
                        return Unauthorized(new { Status = 401, Messages = "Kata Sandi Salah", Data = new Object() });
                    }
                }
                else
                {
                    // Account not found
                    return NotFound(new { Status = 404, Messages = "Akun Tidak Ditemukan", Data = new Object() });
                }
            }
            catch (Exception ex)
            {
                // General error
                return StatusCode(500, new { Status = 500, Messages = "Terjadi Kesalahan Saat Login = " + ex.Message, Data = new Object() });
            }
        }


        [HttpPost("/daftar", Name = "DaftarUserBaru")]
        public IActionResult DaftarUserBaru([FromBody] UserModel user)
        {
            var result = _userRepo.daftarUser(user);
            return StatusCode(result.status, new { Status = result.status, Messages = result.messages, Data = result.data });
        }
    }
}
