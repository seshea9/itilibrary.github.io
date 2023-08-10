using ITI_Libraly_Api.MyModels;

namespace ITI_Libraly_Api.Service.Interface
{
    public interface INationalService
    {
        public Task<Data> GetList();
        public Task<Data> Ctreate(NationalModel model);
    }
}
