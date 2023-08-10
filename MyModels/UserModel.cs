using System.ComponentModel.DataAnnotations;

namespace ITI_Libraly_Api.MyModels
{
    public class UserModel
    {
        public int UseId { get; set; }
        [Required]
        public string? UseName { get; set; }
        [Required]
        public string? UseType { get; set; }
        public string? UseCreateDate { get; set; }
        public string? UsePasswords { get; set; }
        [Required]
        public int? EmpId { get; set; }
    }
}
