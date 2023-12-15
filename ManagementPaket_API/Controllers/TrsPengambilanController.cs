using ManagementPaket_API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManagementPaket_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrsPengambilanController : ControllerBase
    {
        private readonly TrsPengambilan _trspengambilanRepository;
        ResponseModel response = new ResponseModel();

        public TrsPengambilanController(IConfiguration configuration)
        {
            _trspengambilanRepository = new TrsPengambilan(configuration);
        }

        [HttpGet("/GetAllTrsPengambilan", Name = "GetAllTrsPengambilan")]
        public IActionResult GetAllTrsPengambilan()
        {
            try
            {
                response.status = 200;
                response.messages = "Success";
                response.data = _trspengambilanRepository.GetAllData();
            }
            catch (Exception ex)
            {
                response.status = 500;
                response.messages = "Failed" + ex;
            }
            return Ok(response);
            //  return Ok(_paketRepository.getAllData());
        }

        [HttpGet("/GetTrsPengambilan", Name = "GetTrsPengambilan")]
        public IActionResult GetTrsPengambilan(int pak_id_transaksi)
        {
            try
            {
                response.status = 200;
                response.messages = "Success";
                response.data = _trspengambilanRepository.GetData(pak_id_transaksi);
            }
            catch (Exception ex)
            {
                response.status = 500;
                response.messages = "Failed, " + ex;
            }
            return Ok(response);
        }

        [HttpPost("/InsertTrsPengambilan", Name = "InsertTrsPengambilan")]
        public IActionResult InsertTrsPengambilan(int pak_id, string pak_id_pengambil, string pak_nama_pengambil, string pak_foto_pengambil)
        {
            try
            {
                response.status = 200;
                response.messages = "Success";
                _trspengambilanRepository.InsertData(pak_id, pak_id_pengambil, pak_nama_pengambil, pak_foto_pengambil);
            }
            catch (Exception ex)
            {
                response.status = 500;
                response.messages = "Failed, " + ex;
            }
            return Ok(response);
        }



        [HttpGet("/GetViewTrsPengambilan", Name = "GetViewTrsPengambilan")]
        public IActionResult GetViewTrsPengambilan()
        {
            try
            {
                response.status = 200;
                response.messages = "Success";
                response.data = _trspengambilanRepository.GetViewTrsPengambilan();
            }
            catch (Exception ex)
            {
                response.status = 500;
                response.messages = "Failed" + ex;
            }
            return Ok(response);
            //  return Ok(_paketRepository.getAllData());
        }
    }
}
