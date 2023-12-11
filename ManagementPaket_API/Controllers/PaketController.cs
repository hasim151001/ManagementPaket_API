using ManagementPaket_API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManagementPaket_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaketController : ControllerBase
    {

        private readonly Paket _paketRepository;
        ResponseModel response = new ResponseModel();

        public PaketController(IConfiguration configuration)
        {
            _paketRepository = new Paket(configuration);
        }

        [HttpGet("/GetAllPaket", Name = "GetAllPaket")]
        public IActionResult GetAllPaket()
        {
            try
            {
                response.status = 200;
                response.messages = "Success";
                response.data = _paketRepository.GetAllData();
            }
            catch (Exception ex)
            {
                response.status = 500;
                response.messages = "Failed" + ex;
            }
            return Ok(response);
            //  return Ok(_paketRepository.getAllData());
        }

        [HttpGet("/GetPaket", Name = "GetPaket")]
        public IActionResult GetPaket(int pak_id)
        {
            try
            {
                response.status = 200;
                response.messages = "Success";
                response.data = _paketRepository.GetData(pak_id);
            }
            catch (Exception ex)
            {
                response.status = 500;
                response.messages = "Failed, " + ex;
            }
            return Ok(response);
        }

        [HttpPost("/InsertPaket", Name = "InsertPaket")]
        public IActionResult InsertPaket(string pak_nama_pemilik, int pak_id_jenis, string pak_nama_pengirim)
        {
            try
            {
                response.status = 200;
                response.messages = "Success";
                _paketRepository.InsertData(pak_nama_pemilik, pak_id_jenis, pak_nama_pengirim);
            }
            catch (Exception ex)
            {
                response.status = 500;
                response.messages = "Failed, " + ex;
            }
            return Ok(response);
        }

        [HttpPut("/UpdatePaket", Name = "UpdatePaket")]
        public IActionResult UpdatePaket([FromBody] PaketModel paketModel)
        {

            try
            {
                response.status = 200;
                response.messages = "Success";
                _paketRepository.UpdateData(paketModel);
            }
            catch (Exception ex)
            {
                response.status = 500;
                response.messages = "Failed, " + ex;
            }
            return Ok(response);
        }

        [HttpDelete("/DeletePaket", Name = "DeletePaket")]
        public IActionResult DeletePaket(int pak_id)
        {
            try
            {
                response.status = 200;
                response.messages = "Success";
                _paketRepository.DeleteData(pak_id);
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
