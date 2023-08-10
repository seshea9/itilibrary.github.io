using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics.X86;

namespace ITI_Libraly_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
        [HttpPost, Route("PostUserInfo")]
        public async Task<IActionResult> PostUserInfo(UserModel use)
        {
            Data model = new Data();
            try
            {
                model = await userService.PostUserInfo(use);
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return Ok(model);
        }
        [HttpGet, Route("GetUserInfo")]
        public async Task<IActionResult> GetUserInfo(int UseId)
        {
            Data model = new Data();
            try
            {
                model = await userService.GetUserInfo(UseId);
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return Ok(model);
        }
        [HttpGet, Route("GetUserList")]
        public async Task<IActionResult> GetUserList(/*int pageSkip = 0, int pageSize = 10,*/ bool isSearch = false, string? UseName = null)
        {
            Data model = new Data();
            try
            {
                model = await userService.GetUserList(/*pageSkip,pageSize,*/isSearch,UseName);
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
