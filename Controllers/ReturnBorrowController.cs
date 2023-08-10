using ITI_Libraly_Api.Models;
using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ITI_Libraly_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnBorrowController : ControllerBase
    {
        private readonly IReturnBorrowService returnService;

        public ReturnBorrowController(IReturnBorrowService returnService)
        {
            this.returnService = returnService;
        }
        [HttpPost,Route("PostReturnInfo")]
        public async Task<IActionResult> PostReturnBorrowInfo(ReturnModel ret)
        {
            Data model = new Data();
            try
            {
                model = await returnService.PostReturnBorrowInfo(ret);
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return Ok(model);
        }
        [HttpGet,Route("GetReturnInfo")]
        public async Task<IActionResult> GetReturnBorrowInfo(int RetId)
        {
            Data model = new Data();
            try
            {
                model = await returnService.GetReturnBorrowInfo(RetId);
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return Ok(model);
        }
        [HttpGet,Route("GetReturnBorrowList")]
        public async Task<IActionResult> GetReturnBorrowList(int pageSkip = 0, int pageSize = 10, bool isSearch = false, int? StuId = 0, DateTime? startDate = null, DateTime? endDate = null, string? BorCode = null,string? BookDelId = null)
        {
            Data model = new Data();
            try
            {
                model = await returnService.GetReturnBorrowList(pageSkip, pageSize, isSearch, StuId, startDate, endDate, BorCode,BookDelId);
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
