using System.ComponentModel.DataAnnotations;

namespace ITI_Libraly_Api.MyModels
{
    public class ImportDetailsModel
    {
        public int ImpDelId { get; set; }
        //[Required]
        public int? ImpId { get; set; }
        [Required]
        public int? BookId { get; set; }
        [Required]
        public int? ImpQty { get; set; }
        public double? ImpPrice { get; set; }
    }
}
