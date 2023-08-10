using System.ComponentModel.DataAnnotations;

namespace ITI_Libraly_Api.MyModels
{
    public class ReadDeltailsModel
    {
        public int ReadDelId { get; set; }
        //[Required]
        public int? ReadId { get; set; }
        [Required]
        public string? BookDelId { get; set; }
        //[Required]
        //public int? StatusId { get; set; }
    }
}
