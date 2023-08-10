using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ITI_Libraly_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowController : ControllerBase
    {
        private readonly IBorrowService borrowService;

        public BorrowController(IBorrowService borrowService)
        {
            this.borrowService = borrowService;
        }
        [HttpPost,Route("PostBorrowInfo")]
        public async Task<IActionResult> PostBorrowInfo(BorrowModel bor)
        {
            Data model = new Data();
            try
            {
                model = await borrowService.PostBorrowInfo(bor);
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return Ok(model);
        }
        [HttpGet,Route("GetBorrowInfo")]
        public async Task<IActionResult> GetBorrowInfo(int BorId)
        {
            Data model = new Data();
            try
            {
                model = await borrowService.GetBorrowInfo(BorId);
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return Ok(model);
        }
        [HttpGet,Route("GetBorrowList")]
        public async Task<IActionResult> GetBorrowList(int pageSkip = 0, int pageSize = 10, bool isSearch = false, int? StuId = 0, DateTime? startDate = null, DateTime? endDate = null, string? BookDelId = null, string? BarCode = null)
        {
            Data model = new Data();
            try
            {
                model = await borrowService.GetBorrowList(pageSkip, pageSize, isSearch, StuId, startDate, endDate,BookDelId,BarCode);
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
