using ITI_Libraly_Api.MyModels;

namespace ITI_Libraly_Api.Service.Interface
{
    public interface IStudentService
    {
        public Task<Data> PostStudentInfo(StudentModel stu);
        public Task<Data> GetStudentInfo(int StuId);
        public Task<Data> GetStudentList(/*int pageSkip = 0, int pageSize = 10,*/ bool isSearch = false, string? StuName = null, string? StuPhone = null, string? StuCode = null);
    }
}
