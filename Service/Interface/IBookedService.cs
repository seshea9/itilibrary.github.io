using ITI_Libraly_Api.MyModels;

namespace ITI_Libraly_Api.Service.Interface
{
    public interface IBookedService
    {
        public Task<Data> PostBookInfo(BookModel book);
        
        public Task<Data> GetBookInfo(int BookId);
        public Task<Data> GetBookList(int pageSkip = 0, int pageSize = 10, bool isSearch = false , string? BookNameKh= null, int? CateId = 0, string? LocId= null,string? BookDelId =null,int BookId =0);
    }
}
