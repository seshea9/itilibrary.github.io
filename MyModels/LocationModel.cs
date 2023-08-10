using System.ComponentModel.DataAnnotations;

namespace ITI_Libraly_Api.MyModels
{
    public class LocationModel
    {
        public string? LocId { get; set; }
        [Required]
        public string? LocLabel { get; set; }
        public bool? LocActive { get; set; }
    }
}
