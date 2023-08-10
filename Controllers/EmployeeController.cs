using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ITI_Libraly_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }
        [HttpPost, Route("PostEmployeeInfo")]
      public async Task <IActionResult>  PostEmployeeInfo(EmployeeModel emp)
        {
            Data model = new Data();
            try
            {
                model = await employeeService.PostEmployeeInfo(emp);
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return Ok(model);
        }
        [HttpGet, Route("GetEmployeeInfo")]
      public async Task<IActionResult> GetEmployeeInfo(int EmpId)
        {
            Data model = new Data();
            try
            {
                model = await employeeService.GetEmployeeInfo(EmpId);
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return Ok(model);
        }
        [HttpGet, Route("GetEmployeeList")]
       public async Task<IActionResult> GetEmployeeList(int pageSkip = 0, int pageSize = 10, bool isSearch = false, string? EmpNoteId = null, string? EmpNameKh = null, string? EmpNameEn = null)
        {
            Data model = new Data();
            try
            {
                model = await employeeService.GetEmployeeList(pageSkip,pageSize,isSearch,EmpNoteId,EmpNameKh,EmpNameEn);
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

