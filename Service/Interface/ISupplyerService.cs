using ITI_Libraly_Api.MyModels;

namespace ITI_Libraly_Api.Service.Interface
{
    public interface ISupplyerService
    {
        public Task<Data> PostSupplyerInfo(SupplyerModel sup);
        public Task<Data> GetSupplyerInfo(int SupId);
        public Task<Data> GetSupplyerList(/*int pageSkip = 0, int pageSize=10,*/ bool isSearch = false, string? SupName= null, string? SupPhone = null);
        /* public int SupId { get; set; }
        public string SupName { get; set; }
        public string SupAddress { get; set; }
        public string SupPhone { get; set; }*/
    }
}
