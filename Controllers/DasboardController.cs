using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ITI_Libraly_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DasboardController : ControllerBase
    {
        private readonly IDasbordService dasbordService;

        public DasboardController(IDasbordService dasbordService) 
        {
            this.dasbordService = dasbordService;
        }
        [HttpGet,Route("GetDasbordList")]
        public async Task<IActionResult> GetDasbordList()
        {
            Data model = new Data();
            try
            {
                model = await dasbordService.GetDasbordList();
            }
            catch (Exception ex)
            {

            }
            return Ok(model);
        }
        [HttpGet,Route("GetDasbordNowList")]
        public async Task<IActionResult> GetDasbordNowList()
        {
            Data model = new Data();
            try
            {
                model = await dasbordService.GetDasbordNowList();
            }
            catch (Exception ex)
            {

            }
            return Ok(model);
        }
    }
}
