using ITI_Libraly_Api.MyModels;

namespace ITI_Libraly_Api.Service.Interface
{
    public interface ILocationService
    {
        public Task<Data> PostLocationInfo(LocationModel loc);
        public Task<Data> GetLocationInfo(string? LocId);
        public Task<Data> GetLocationList(/*int pageSkip = 0, int pageSize = 10,*/ bool isSearch = false, string? LocLabel = null , bool? LocActive = true);
    }
}
