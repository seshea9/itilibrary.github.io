using ITI_Libraly_Api.MyModels;

namespace ITI_Libraly_Api.Service.Interface
{
    public interface IReadService
    {
        public Task<Data> PostReadInfo(ReadModel read);
        public Task<Data> GetReadInfo(int ReadId);
        public Task<Data> GetReadList(/*int pageSkip = 0, int pageSize = 10,*/ bool isSearch = false, int? StuId = 0,string? startDate = null, string? endDate = null);
        /*  public int ReadId { get; set; }
        [Required]
        public int? StuId { get; set; }
        [Required]
        public string? ReadDate { get; set; }
        [Required]
        public int? UseDelId { get; set; }
        public string? UseEditDate { get; set; }
        [Required]
        public List<ReadDeltailsModel>? readDeltailsModels { get; set; }*/
    }
}
