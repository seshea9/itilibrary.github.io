using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ITI_Libraly_Api.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }
        [HttpPost, Route("PostCategoryInfo")]
        public async Task<IActionResult> PostCategoryInfo(CategoryModel cate)
        {
            Data model = new Data();
            try
            {
                model = await categoryService.PostCategoryInfo(cate);
            }
             catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return Ok(model);
        }
        [HttpGet, Route("GetCategoryInfo")]
        public async Task<IActionResult> GetCategoryInfo(int CateId)
        {
            Data model = new Data();
            try
            {
                model = await categoryService.GetCategoryInfo(CateId);
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return Ok(model);
        }
        [HttpGet, Route("GetCategoryList")]
        public async Task<IActionResult> GetCategoryList(/*int pageSkip = 0, int pageSize = 10,*/ bool isSearch = false, string? CateNameKh = null, string? CateNameEn​ = null)
        {
            Data model = new Data();
            try
            {
                model = await categoryService.GetCategoryList(/*pageSkip,pageSize,*/isSearch,CateNameKh,CateNameEn);
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
