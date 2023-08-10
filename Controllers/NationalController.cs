using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ITI_Libraly_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NationalController : ControllerBase
    {
        private readonly INationalService nationalService;

        public NationalController(INationalService nationalService) 
        {
            this.nationalService = nationalService;
        }
        [HttpGet, Route("GetList")]
        public async Task<IActionResult> GetList()
        {
            Data data = new Data();
            try
            {
                data = await nationalService.GetList();
            }
            catch (Exception ex)
            {
                data.success = false;
                data.message = ex.Message;
            }
            return Ok(data);
        }
        [HttpPost, Route("Ctreate")]
        public async Task<IActionResult> Ctreate(NationalModel model)
        {
            Data data = new Data();
            try
            {
                data = await nationalService.Ctreate(model);
            }
            catch (Exception ex)
            {
                data.success = false;
                data.message = ex.Message;
            }
            return Ok(data);
        }
    }
}
