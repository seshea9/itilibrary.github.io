namespace ITI_Libraly_Api.MyModels
{
    public class ReturnReadModel
    {
        public int RetReadId { get; set; }
        public string? RetReadDate { get; set; }
        public int? UseDelId { get; set; }
        public string? UseEditDate { get; set; }
        public List<ReturnReadDetailsModel>? returnReadDetailsModels { get; set; }
    }
}
