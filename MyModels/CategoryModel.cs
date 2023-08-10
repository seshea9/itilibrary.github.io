using System.ComponentModel.DataAnnotations;

namespace ITI_Libraly_Api.MyModels
{
    public class CategoryModel
    {
        public int CateId { get; set; }
        [Required]
        public string? CateNameKh { get; set; }
        public string? CateNameEn { get; set; }
    }
}
