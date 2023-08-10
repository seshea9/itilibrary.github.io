using System.ComponentModel.DataAnnotations;

namespace ITI_Libraly_Api.MyModels
{
    public class PositionModel
    {
        public int PosId { get; set; }
        [Required]
        public string? PosNameKh { get; set; }
        public string? PosNameEn { get; set; }
    }

}
