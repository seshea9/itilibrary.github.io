using ITI_Libraly_Api.MyModels;
using System.ComponentModel.DataAnnotations;

namespace ITI_Libraly_Api.Service.Interface
{
    public interface IImportService
    {
        public Task<Data> PostImportInfo(ImportModel imp);
        public Task<Data> GetImportInfo(int ImpId);
        public Task<Data> GetImportList(/*int pageSkip = 0, int pageSize =10,*/ bool isSearch =false,string? startDate= null,string? endDate = null, int? SupId = 0);
        /*public int ImpId { get; set; }
        [Required]
        public string? ImpDate { get; set; }
        [Required]
        public int? SupId { get; set; }
        public int? UseDelId { get; set; }
        public string? UseEditDate { get; set; }
        [Required]
        public List<ImportDetailsModel>? importDetails { get; set; }*/
    }
}
