using ManagementPaket_API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManagementPaket_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JenisPaketController : ControllerBase
    {

        private readonly JenisPaket _jenispaketRepository;
        ResponseModel response = new ResponseModel();

        public JenisPaketController(IConfiguration configuration)
        {
            _jenispaketRepository = new JenisPaket(configuration);
        }
        [HttpGet("/GetAllJenisPaket", Name = "GetAllJenisPaket")]
        public IActionResult GetAllJenisPaket()
        {
            try
            {
                response.status = 200;
                response.messages = "Success";
                response.data = _jenispaketRepository.GetAllData();
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = 500;
                response.messages = $"Failed: {ex.Message}";
                return StatusCode(500, response);
            }
        }

        [HttpGet("/GetJenisPaket", Name = "GetJenisPaket")]
        public IActionResult GetJenisPaket(string pak_id_jenis)
        {
            try
            {
                response.status = 200;
                response.messages = "Success";
                response.data = _jenispaketRepository.GetData(pak_id_jenis);
            }
            catch (Exception ex)
            {
                response.status = 500;
                response.messages = "Failed, " + ex;
            }
            return Ok(response);
        }


        [HttpPost("/InsertJenisPaket", Name = "InsertJenisPaket")]
        public IActionResult InsertJenisPaket(string pak_nama_jenis)
        {
            try
            {
                response.status = 200;
                response.messages = "Success";
                _jenispaketRepository.InsertData(pak_nama_jenis);
            }
            catch (Exception ex)
            {
                response.status = 500;
                response.messages = "Failed, " + ex;
            }
            return Ok(response);
        }

        [HttpPut("/UpdateJenisPaket", Name = "UpdateJenisPaket")]
        public IActionResult UpdateJenisPaket([FromBody] JenisPaketModel jenispaketModel)
        {
            try
            {
                response.status = 200;
                response.messages = "Success";
                _jenispaketRepository.UpdateData(jenispaketModel);
            }
            catch (Exception ex)
            {
                response.status = 500;
                response.messages = "Failed, " + ex;
            }
            return Ok(response);
        }

        [HttpDelete("/DeleteJenisPaket", Name = "DeleteJenisPaket")]
        public IActionResult DeleteJenisPaket(string pak_id_jenis)
        {
            try
            {
                response.status = 200;
                response.messages = "Success";
                _jenispaketRepository.DeleteData(pak_id_jenis);
            }
            catch (Exception ex)
            {
                response.status = 500;
                response.messages = "Failed, " + ex;
            }
            return Ok(response);
        }

    }
}
