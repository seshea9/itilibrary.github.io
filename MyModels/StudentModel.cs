using System.ComponentModel.DataAnnotations;

namespace ITI_Libraly_Api.MyModels
{
    public class StudentModel
    {
        public int StuId { get; set; }
        [Required]
        public string? StuName { get; set; }
        public string? StuSex { get; set; }
        [Required]
        public string? StuAddress { get; set; }
        [Required]
        public string? StuPhone { get; set; }
        [Required]
        public int? PosId { get; set; }
        public string? StuCode { get; set; }
        public DateTime? StuDob { get; set; }
        [Required]
        public int? NationalId { get; set; }
        public bool? IsPrintCart { get; set; }
    }
}
