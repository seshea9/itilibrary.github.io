using System.ComponentModel.DataAnnotations;

namespace ITI_Libraly_Api.MyModels
{
    public class ImportModel
    {
        public int ImpId { get; set; }
        [Required]
        public string? ImpDate { get; set; }
        [Required]
        public int? SupId { get; set; }
        public int? UseDelId { get; set; }
        public string? UseEditDate { get; set; }
        [Required]
        public List <ImportDetailsModel>? importDetails { get; set; }
    }
}
