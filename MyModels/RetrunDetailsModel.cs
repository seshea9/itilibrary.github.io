using System.ComponentModel.DataAnnotations;

namespace ITI_Libraly_Api.MyModels
{
    public class RetrunDetailsModel
    {
        public int RetDelId { get; set; }
        public int? RetId { get; set; }
        [Required]
        public int? BorId { get; set; }
    }
}
