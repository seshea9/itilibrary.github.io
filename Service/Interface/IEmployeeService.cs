using ITI_Libraly_Api.MyModels;

namespace ITI_Libraly_Api.Service.Interface
{
    public interface IEmployeeService
    {
        public Task<Data> PostEmployeeInfo(EmployeeModel emp);
        public Task<Data> GetEmployeeInfo(int EmpId);
        public Task<Data> GetEmployeeList(int pageSkip = 0, int pageSize = 10, bool isSearch = false, string? EmpNoteId = null, string? EmpNameKh = null, string? EmpNameEn =null);
    }
}
