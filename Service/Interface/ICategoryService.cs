using ITI_Libraly_Api.MyModels;

namespace ITI_Libraly_Api.Service.Interface
{
    public interface ICategoryService
    {
        public Task<Data> PostCategoryInfo(CategoryModel cate);
        public Task<Data> GetCategoryInfo(int CateId);
        public Task<Data> GetCategoryList(/*int pageSkip = 0, int pageSize = 10,*/ bool isSearch = false, string? CateNameKh = null, string? CateNameEn​ = null);

    }
}
