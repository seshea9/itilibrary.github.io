using ITI_Libraly_Api.MyModels;

namespace ITI_Libraly_Api.Service.Interface
{
    public interface IPositionService
    {
        public Task<Data> GetPositionInfo(int PosId);
        public Task<Data> GetPositionList(/*int pageSkip = 0, int pageSize = 10 ,*/ bool isSearch = false , string? PosNameKh =null, string? PosNameEn = null);
        public Task<Data> PostPositionInfo(PositionModel positionModel);
    }
}
