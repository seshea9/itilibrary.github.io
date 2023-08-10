using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ITI_Libraly_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService studentService;

        public StudentController(IStudentService studentService)
        {
            this.studentService = studentService;
        }
        [HttpPost, Route("PostStudentInfo")]
        public async Task<IActionResult> PostStudentInfo(StudentModel stu)
        {
            Data model = new Data();
            try
            {
                model = await studentService.PostStudentInfo(stu);
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return Ok(model);
        }
        [HttpGet,Route("GetStudentInfo")]
        public async Task<IActionResult> GetStudentInfo(int StuId)
        {
            Data model = new Data();
            try
            {
                model = await studentService.GetStudentInfo(StuId);
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return Ok(model);
        }
        [HttpGet,Route("GetStudentList")]
        public async Task<IActionResult> GetStudentList(/*int pageSkip = 0, int pageSize = 10,*/ bool isSearch = false, string? StuName = null, string? StuPhone = null, string? StuCode = null)
        {
            Data model = new Data();
            try
            {
                model = await studentService.GetStudentList(/*pageSkip,pageSize,*/isSearch,StuName,StuPhone,StuCode);
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
