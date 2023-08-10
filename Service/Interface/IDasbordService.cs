using ITI_Libraly_Api.MyModels;

namespace ITI_Libraly_Api.Service.Interface
{
    public interface IDasbordService
    {
        public Task<Data> GetDasbordList();
        public Task<Data> GetDasbordNowList();
    }
}
