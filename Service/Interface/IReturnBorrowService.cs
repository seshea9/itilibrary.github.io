
using ITI_Libraly_Api.MyModels;

namespace ITI_Libraly_Api.Service.Interface
{
    public interface IReturnBorrowService
    {
        public Task<Data> PostReturnBorrowInfo(ReturnModel ret);
        public Task<Data> GetReturnBorrowInfo(int RetId);
        public Task<Data> GetReturnBorrowList(int pageSkip = 0, int pageSize = 10, bool isSearch = false, int? StuId = 0, DateTime? startDate = null, DateTime? endDate = null, string? BorCode = null,string? BookDelId = null);
        /* public int RetId { get; set; }
        public DateTime? RetDate { get; set; }
        public int? StuId { get; set; }
        public int? UseDelId { get; set; }
        public DateTime? UseEditDate { get; set; }

        public virtual TblItiStudent Stu { get; set; }
        public virtual TblUseLogintime UseDel { get; set; }
        public virtual ICollection<TblItiReturndeltails> TblItiReturndeltails { get; set; }
        */
    }
}
