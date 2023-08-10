using ITI_Libraly_Api.MyModels;

namespace ITI_Libraly_Api.Service.Interface
{
    public interface IGeneralDataService
    {
        public Task<Data> GeneralDataLocation();
        public Task<Data> GenerlDataCategory(int? expert_id =0);
        public Task<Data> GenerlDataPosition();
        public Task<Data> GeneralSupplyer();
        public Task<Data> GeneralDataEmployee();
        public Task<Data> GeneralDataBook(bool isSearch = false, string? BookNameKh = null, int? CateId = 0);
        public Task<Data> GeneralDataBookDetail(int pageSkip = 0, int pageSize = 10, bool isSearch = false, int BookId =0, string ? BookNameKh = null, int CateId = 0,string? LocId =null,string? BookDelId = null, string baseUri = "", int StatusId = 0);
        public Task<Data> GeneralDataStudent( bool isSearch = false, string? StuName = null, int StuId = 0);
        public Task<Data> GeneralDataBorrow(int pageSkip = 0, int pageSize = 10, bool isSearch = false,DateTime? BorrowDate = null/*, string? BookDelId = null*/,string ? BorCode = null,int StuId =0, bool IsRequried = false);
    }
}
