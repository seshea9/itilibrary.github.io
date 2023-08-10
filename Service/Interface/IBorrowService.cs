using ITI_Libraly_Api.MyModels;

namespace ITI_Libraly_Api.Service.Interface
{
    public interface IBorrowService
    {
        public Task<Data> PostBorrowInfo(BorrowModel bor);
        public Task<Data> GetBorrowInfo(int BorId);
        public Task<Data> GetBorrowList(int pageSkip = 0, int pageSize = 10, bool isSearch = false, int? StuId = 0, DateTime? startDate = null, DateTime? endDate = null,string? BookDelId = null,string? BarCode = null);
        /*  public int BorId { get; set; }
        [Required]
        public int? StuId { get; set; }
        [Required]
        public string? BorDate { get; set; }
        [Required]
        public int? UseDelId { get; set; }
        public string? UseEditDate { get; set; }
        [Required]
        public List<BorrowDetailsModel>? BorrowDetailsModels { get; set; }*/
    }
}
