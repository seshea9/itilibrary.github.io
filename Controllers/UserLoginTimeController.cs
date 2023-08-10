using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ITI_Libraly_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginTimeController : ControllerBase
    {
        private readonly IUserLoginTimerService userLoginTimerService;

        public UserLoginTimeController(IUserLoginTimerService userLoginTimerService)
        {
            this.userLoginTimerService = userLoginTimerService;
        }
        [HttpPost, Route("PostUserLoginTimeInfo")]
        public async Task<IActionResult> PostUserLoginTimeInfo(UserLoginTimeModel userl)
        {
            Data model = new Data();
            try
            {
                model = await userLoginTimerService.PostUserLoginTimeInfo(userl);
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
