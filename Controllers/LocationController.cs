using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ITI_Libraly_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService locationService;

        public LocationController(ILocationService locationService)
        {
            this.locationService = locationService;
        }
        [HttpPost, Route("PostLocationInfo")]
        public async Task<IActionResult> PostLocationInfo(LocationModel loc)
        {
            Data model = new Data();
            try
            {
                model = await locationService.PostLocationInfo(loc);
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return Ok(model);
        }
        [HttpGet, Route("GetLocationInfo")]
        public async Task<IActionResult> GetLocationInfo(string? LocId)
        {
            Data model = new Data();
            try
            {
                model = await locationService.GetLocationInfo(LocId);
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return Ok(model);
        }
        [HttpGet,Route("GetLocationList")]
        public async Task<IActionResult> GetLocationList(/*int pageSkip = 0, int pageSize = 10,*/ bool isSearch = false, string? LocLabel = null, bool? LocActive = true)
        {
            Data model = new Data();
            try
            {
                model = await locationService.GetLocationList(/*pageSkip,pageSize,*/isSearch,LocLabel,LocActive);
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return Ok(model);
        }
        /*public Task<Data> PostLocationInfo(LocationModel loc);
         public Task<Data> GetLocationInfo(string? LocId);
         public Task<Data> GetLocationList(int pageSkip = 0, int pageSize = 10, bool isSearch = false, string? LocLabel = null, bool? LocActive = true);*/
    }
}
