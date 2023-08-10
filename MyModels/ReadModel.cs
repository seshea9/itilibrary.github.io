using System.ComponentModel.DataAnnotations;

namespace ITI_Libraly_Api.MyModels
{
    public class ReadModel
    {
        public int ReadId { get; set; }
        [Required]
        public int? StuId { get; set; }
        [Required]
        public string? ReadDate { get; set; }
        [Required]
        public int? UseDelId { get; set; }
        public string? UseEditDate { get; set; }
        [Required]
        public string? StartTime { get; set; }
        [Required]
        public string? EndTime { get; set; }
        [Required]
        public short? TimeTypeId { get; set; }
        public int? StatusId { get; set; }
        [Required]
        public List<ReadDeltailsModel>? readDeltailsModels { get; set; }
    }
}
