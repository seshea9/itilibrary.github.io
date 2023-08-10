using System.ComponentModel.DataAnnotations;

namespace ITI_Libraly_Api.MyModels
{
    public class BorrowModel
    {
        public int BorId { get; set; }
        public string? BorCode { get; set; }
        [Required]
        public int? StuId { get; set; }
        [Required]
        public DateTime? BorStartDate { get; set; }
        [Required]
        public DateTime? BorEndDate { get; set; }
        [Required]
        public int? UseDelId { get; set; }
        public string? UseEditDate { get; set; }
        [Required]
        public List<BorrowDetailsModel>? borrowDetails { get; set; }
    }
}
