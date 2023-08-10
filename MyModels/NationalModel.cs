using System.ComponentModel.DataAnnotations;

namespace ITI_Libraly_Api.MyModels
{
    public class NationalModel
    {
        public int Id { get; set; }
        [Required]
        public string? NameEn { get; set; }
        [Required]
        public string? NameKh { get; set; }
        public string? Description { get; set; }
        public string? Sort { get; set; }
        public bool? Active { get; set; }
    }
}
