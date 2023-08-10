using ITI_Libraly_Api.MyModels;

namespace ITI_Libraly_Api.Service.Interface
{
    public interface IReturnReadService 
    {
        public Task<Data> PostReturnReadInfo(ReturnReadModel rrm);
        public Task<Data> GetReturnReadInfo(int RetReadId);
        public Task<Data> GetReturnReadList(int pageSkip = 0, int pageSize = 10, bool isSearch = false, string? startDate = null, string? endDate = null, int StuId = 0, string? BookDelId = null);
        /* public int RetReadId { get; set; }
        public string? RetReadDate { get; set; }
        public int? UseDelId { get; set; }
        public string? UseEditDate { get; set; }
        public List<ReturnReadDetailsModel>? returnReadDetailsModels { get; set; }*/

    }
}
