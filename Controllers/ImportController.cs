using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ITI_Libraly_Api.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ImportController :  ControllerBase
    {
        private readonly IImportService importService;

        public ImportController(IImportService importService)
        {
            this.importService = importService;
        }
        [HttpPost, Route("PostImportInfo")]
        public async Task<IActionResult> PostImportInfo(ImportModel imp)
        {
            Data model = new Data();
            try
            {
                model = await importService.PostImportInfo(imp);
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return Ok(model);
        }
        [HttpGet, Route("GetImportInfo")]
        public async Task<IActionResult> GetImportInfo(int ImpId)
        {
            Data model = new Data();
            try
            {
                model = await importService.GetImportInfo(ImpId);
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return Ok(model);
        }
        [HttpGet, Route("GetImportList")]
        public async Task<IActionResult> GetImportList(/*int pageSkip = 0, int pageSize = 10,*/ bool isSearch = false, string? startDate = null, string? endDate = null, int? SupId = 0)
        {
            Data model = new Data();
            try
            {
                model = await importService.GetImportList(/*pageSkip,pageSize,*/isSearch,startDate,endDate,SupId);
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
