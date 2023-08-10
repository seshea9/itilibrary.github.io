using System.ComponentModel.DataAnnotations;

namespace ITI_Libraly_Api.MyModels
{
    public class BorrowDetailsModel
    {
        public int BorDelId { get; set; }
        public int? BorId { get; set; }
        [Required]
        public string? BookDelId { get; set; }
    }
}
