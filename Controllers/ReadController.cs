using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ITI_Libraly_Api.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ReadController : ControllerBase
    {
        private readonly IReadService readService;
        public ReadController(IReadService readService)
        {
            this.readService = readService;
        }
        [HttpPost, Route("PostReadInfo")]
        public async Task<IActionResult> PostReadInfo(ReadModel read)
        {
            Data model = new Data();
            try
            {
                model = await readService.PostReadInfo(read);
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return Ok(model);
        }
        [HttpGet, Route("GetReadInfo")]
        public async Task<IActionResult> GetReadInfo(int ReadId)
        {
            Data model = new Data();
            try
            {
                model = await readService.GetReadInfo(ReadId);
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return Ok(model);
        }
        [HttpGet,Route("GetReadList")]
        public async Task<IActionResult> GetReadList(/*int pageSkip = 0, int pageSize = 10,*/ bool isSearch = false, int? StuId = 0, string? startDate = null, string? endDate = null)
        {
            Data model = new Data();
            try
            {
                model = await readService.GetReadList(/*pageSkip, pageSize,*/ isSearch, StuId, startDate, endDate);
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return Ok(model);
        }
    }
}
