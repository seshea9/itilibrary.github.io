using System.ComponentModel.DataAnnotations;

namespace ITI_Libraly_Api.MyModels
{
    public class SupplyerModel
    {
        public int SupId { get; set; }
        [Required]
        public string? SupName { get; set; }
        public string? SupAddress { get; set; }
        public string? SupPhone { get; set; }
    }
}
