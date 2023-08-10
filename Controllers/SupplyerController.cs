using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ITI_Libraly_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplyerController : ControllerBase
    {
        private readonly ISupplyerService supplyerService;
        public SupplyerController(ISupplyerService supplyerService) 
        {
            this.supplyerService = supplyerService;
        }
        [HttpPost, Route("PostSupplyerInfo")]
        public async Task<IActionResult> PostSupplyerInfo(SupplyerModel sup)
        {
            Data model = new Data();
            try
            {
                model = await supplyerService.PostSupplyerInfo(sup);
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return Ok(model);
        }
        [HttpGet, Route("GetSupplyerInfo")]
        public async Task<IActionResult> GetSupplyerInfo(int SupId)
        {
            Data model = new Data();
            try
            {
                model = await supplyerService.GetSupplyerInfo(SupId);
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return Ok(model);
        }
        [HttpGet, Route("GetSupplyerList")]
        public async Task<IActionResult> GetSupplyerList(/*int pageSkip = 0, int pageSize = 10,*/ bool isSearch = false, string? SupName = null, string? SupPhone = null)
        {
            Data model = new Data();
            try
            {
                model = await supplyerService.GetSupplyerList(/*pageSkip,pageSize,*/isSearch,SupName,SupPhone);
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
