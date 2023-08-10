using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ITI_Libraly_Api.Controllers
{
    [Route("/api[controller]")]
    [ApiController]
    public class ReturnReadController : ControllerBase
    {
        private readonly IReturnReadService returnReadService;

        public ReturnReadController(IReturnReadService returnReadService)
        {
            this.returnReadService = returnReadService;
        }
        [HttpPost, Route("PostReturnReadInfo")]
        public async Task<IActionResult> PostReturnReadInfo(ReturnReadModel rrm)
        {
            Data model = new Data();
            try
            {
                model = await returnReadService.PostReturnReadInfo(rrm);
            }
            catch(Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return Ok(model);
        }
        [HttpGet,Route("GetReturnReadInfo")]
        public async Task<IActionResult> GetReturnReadInfo(int RetReadId)
        {
            Data model = new Data();
            try
            {
                model = await returnReadService.GetReturnReadInfo(RetReadId);
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return Ok(model);
        }
        [HttpGet,Route("GetReturnReadList")]
        public async Task<IActionResult> GetReturnReadList(int pageSkip = 0, int pageSize = 10, bool isSearch = false, string? startDate = null, string? endDate = null, int StuId = 0, string? BookDelId = null)
        {
            Data model = new Data();
            try
            {
                model = await returnReadService.GetReturnReadList(pageSkip, pageSize, isSearch, startDate, endDate, StuId, BookDelId);
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
