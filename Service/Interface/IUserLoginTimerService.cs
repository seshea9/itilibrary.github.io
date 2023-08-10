using ITI_Libraly_Api.MyModels;

namespace ITI_Libraly_Api.Service.Interface
{
    public interface IUserLoginTimerService​​
    {
        public Task<Data> PostUserLoginTimeInfo(UserLoginTimeModel userl);
    }
}
