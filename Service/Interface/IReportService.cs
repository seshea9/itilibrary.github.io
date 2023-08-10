using ITI_Libraly_Api.MyModels;

namespace ITI_Libraly_Api.Service.Interface
{
    public interface IReportService
    {
        public Task<Data> ReportReading(DateTime? starDate = null, DateTime? endDate = null,long id =0);
    }
}
