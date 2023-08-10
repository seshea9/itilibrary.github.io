using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ITI_Libraly_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralDataController : ControllerBase
    {
        private IGeneralDataService generalDataService;

        public GeneralDataController(IGeneralDataService generalDataService)
        {
            this.generalDataService = generalDataService;
        }
        [HttpGet,Route("GeneralDataLocation")]
        public async Task<IActionResult> GeneralDataLocation()
        {
            Data model = new Data();
            try
            {
                model = await generalDataService.GeneralDataLocation();
            }
            catch (Exception ex)
            {
                model.data = ex.Message;
                model.success = false;
            }
            return Ok(model);
        }
        [HttpGet, Route("GenerlDataCategory")]
        public async Task<IActionResult> GenerlDataCategory(int? expert_id = 0)
        {
            Data model = new Data();
            try
            {
                model = await generalDataService.GenerlDataCategory(expert_id);
            }
            catch (Exception ex)
            {
                model.data = ex.Message;
                model.success = false;
            }
            return Ok(model);
        }
        [HttpGet, Route("GenerlDataPosition")]
        public async Task<IActionResult> GenerlDataPosition()
        {
            Data model = new Data();
            try
            {
                model = await generalDataService.GenerlDataPosition();
            }
            catch (Exception ex)
            {
                model.data = ex.Message;
                model.success = false;
            }
            return Ok(model);
        }
        [HttpGet, Route("GeneralSupplyer")]
        public async Task<IActionResult> GeneralSupplyer()
        {
            Data model = new Data();
            try
            {
                model = await generalDataService.GeneralSupplyer();
            }
            catch (Exception ex)
            {
                model.data = ex.Message;
                model.success = false;
            }
            return Ok(model);
        }
        [HttpGet, Route("GeneralDataEmployee")]
        public async Task<IActionResult> GeneralDataEmployee()
        {
            Data model = new Data();
            try
            {
                model = await generalDataService.GeneralDataEmployee();
            }
            catch (Exception ex)
            {
                model.data = ex.Message;
                model.success = false;
            }
            return Ok(model);
        }
        [HttpGet, Route("GeneralDataBook")]
        public async Task<IActionResult> GeneralDataBook(bool isSearch = false, string? BookNameKh = null, int? CateId = 0)
        {
            Data model = new Data();
            try
            {
                model = await generalDataService.GeneralDataBook(isSearch, BookNameKh, CateId);
            }
            catch (Exception ex)
            {
                model.data = ex.Message;
                model.success = false;
            }
            return Ok(model);
        }
        [HttpGet, Route("GeneralDataBookDetail")]
        public async Task<IActionResult> GeneralDataBookDetail(int pageSkip = 0, int pageSize = 10, bool isSearch = false, int BookId = 0, string? BookNameKh = null, int CateId = 0, string? LocId = null, string? BookDelId = null, string baseUri = "", int StatusId = 0)
        {
            Data model = new Data();
            try
            {
                model = await generalDataService.GeneralDataBookDetail(pageSkip,pageSize,isSearch, BookId, BookNameKh,CateId,LocId,BookDelId,baseUri,StatusId);
            }
            catch (Exception ex)
            {
                model.data = ex.Message;
                model.success = false;
            }
            return Ok(model);
        }
        [HttpGet, Route("GeneralDataStudent")]
        public async Task<IActionResult> GeneralDataStudent(bool isSearch = false, string StuName = null, int StuId = 0)
        {
            Data model = new Data();
            try
            {
                model = await generalDataService.GeneralDataStudent(isSearch, StuName,StuId);
            }
            catch (Exception ex)
            {
                model.data = ex.Message;
                model.success = false;
            }
            return Ok(model);
        }
        [HttpGet, Route("GeneralDataBorrow")]
        public async Task<IActionResult> GeneralDataBorrow(int pageSkip = 0, int pageSize = 10, bool isSearch = false, DateTime? BorrowDate = null/*, string? BookDelId = null*/, string? BorCode = null, int StuId = 0, bool IsRequried = false)
        {
            Data model = new Data();
            try
            {
                model = await generalDataService.GeneralDataBorrow(pageSkip,pageSize, isSearch, BorrowDate/*, BookDelId*/, BorCode, StuId, IsRequried);
            }
            catch (Exception ex)
            {
                model.data = ex.Message;
                model.success = false;
            }
            return Ok(model);
        }
    }
}
