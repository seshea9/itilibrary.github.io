using ITI_Libraly_Api.Models;
using System.ComponentModel.DataAnnotations;

namespace ITI_Libraly_Api.MyModels
{
    public class ReturnModel
    {
        public int RetId { get; set; }
        [Required]
        public int? UseDelId { get; set; }
        public DateTime? UseEditDate { get; set; }
        [Required]
        public DateTime? RetDate { get; set; }
        [Required]
        public List<RetrunDetailsModel>? returndeltails { get; set; }
    }
}
