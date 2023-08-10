using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ITI_Libraly_Api.MyModels
{
    public class BookModel
    {
        public int BookId { get; set; }
        [Required]
        public string? BookNameKh { get; set; }
        public string? BookNameEn { get; set; }
        public string? BookYear { get; set; }
        public int? BookQty { get; set; }
		public string? FilePath { get; set; }
		[Required]
        public int? CateId { get; set; }
        public string? FileName { get; set; }
		//[JsonIgnore(Condition = JsonIgnoreCondition.Always)]
		
		[Required]
        public string? LocId { get; set; }
        public string? BookDescription { get; set; }
        [Required]
        public string? BookAuthor { get; set; }

    }
}
