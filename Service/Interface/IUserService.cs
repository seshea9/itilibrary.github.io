using ITI_Libraly_Api.MyModels;

namespace ITI_Libraly_Api.Service.Interface
{
    public interface IUserService
    {
        public Task<Data> PostUserInfo(UserModel use);
        public Task<Data> GetUserInfo(int UseId);
        public Task<Data> GetUserList(/*int pageSkip = 0,int pageSize = 10,*/ bool isSearch = false, string? UseName = null);
     
    }
}
