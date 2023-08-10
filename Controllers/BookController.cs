   using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ITI_Libraly_Api.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookedService bookedService;
        private ILogger<BookController> logger;

        public BookController(IBookedService bookedService, ILogger<BookController> logger)
        {
            this.bookedService = bookedService;
            this.logger = logger;
        }
        [HttpPost, Route("PostBookInfo")]
        public async Task<IActionResult> PostBookInfo(/*[FromForm]*/ BookModel book)
        {
            Data model = new Data();
            try
            {
                
                model = await bookedService.PostBookInfo(book);
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return Ok(model);
        }
        [HttpGet, Route("GetBookInfo")]
        public async Task<IActionResult> GetBookInfo(int BookId)
        {
            Data model = new Data();
            try
            {
                model = await bookedService.GetBookInfo(BookId);

            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return Ok(model);
        }
        [HttpGet, Route("GetBookList")]
        public async Task<IActionResult> GetBookList(int pageSkip = 0, int pageSize = 10, bool isSearch = false, string? BookNameKh = null, int? CateId = 0, string? LocId = null, string? BookDelId = null,int BookId =0)
        {
            Data model = new Data();
            try
            {
                model = await bookedService.GetBookList(pageSkip,pageSize,isSearch,BookNameKh,CateId,LocId,BookDelId,BookId);
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
