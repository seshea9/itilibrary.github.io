using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ITI_Libraly_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly IPositionService postionService;

        public PositionController(IPositionService positionService)
        {
            this.postionService = positionService;
        }
        [HttpPost, Route("PostPositionInfo")]
     public async Task<IActionResult> PostPositionInfo(PositionModel positionModel)
        {
            Data model = new Data();
            try
            {
                model = await postionService.PostPositionInfo(positionModel);
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return Ok(model);
        }
        [HttpGet, Route("GetPositionInfo")]
        public async Task<IActionResult> GetPositionInfo(int PosId)
        {
            Data model = new Data();
            try
            {
                model = await postionService.GetPositionInfo(PosId);
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return Ok(model);
        }
        [HttpGet, Route("GetPositionList")]
        public async Task<IActionResult> GetPositionList(/*int pageSkip = 0, int pageSize = 10,*/ bool isSearch = false, string? PosNameKh = null, string? PosNameEn = null)
        {
            Data model = new Data();
            try
            {
                model = await postionService.GetPositionList(/*pageSkip,pageSize,*/isSearch,PosNameKh,PosNameEn);
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
