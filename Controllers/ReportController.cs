using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ITI_Libraly_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService reportReadingService;

        public ReportController(IReportService reportReadingService)
        {
            this.reportReadingService = reportReadingService;
        }
        [HttpGet,Route("ReportReading")]
        public async Task<IActionResult> ReportReading(DateTime? starDate = null, DateTime? endDate = null, long id = 0)
        {
            Data model = new Data();
            try
            {
                model = await reportReadingService.ReportReading(starDate, endDate, id);    
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
