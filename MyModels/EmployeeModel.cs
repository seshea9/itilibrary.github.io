using System.ComponentModel.DataAnnotations;

namespace ITI_Libraly_Api.MyModels
{
    public class EmployeeModel
    {
        public int EmpId { get; set; }
        public string? EmpNoteId { get; set; }
        [Required]
        public string? EmpNameKh { get; set; }
        [Required]
        public string? EmpNameEn { get; set; }
        [Required]
        public string? EmpSex { get; set; }
        [Required]
        public string? EmpDob { get; set; }
        public string? EmpAddress { get; set; }
        [Required]
        public string? EmpDateIn { get; set; }
        public string? EmpDateOut { get; set; }
        [Required]
        public string? EmpPhone { get; set; }
        public bool? EmpAtive { get; set; }
        [Required]
        public int? PosId { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        [Required]
        public string? EmpWorkId { get; set; }
        [Required]
        public int? NationalId { get; set; }

    }
}
