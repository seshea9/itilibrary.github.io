namespace ITI_Libraly_Api.MyModels
{
    public class Data
    {
        public Object data { get; set; } = null;
        public string message { get; set; }
        public bool success { get; set; } = false;
        public int total_record { get; set; } = 0;
        public int total_record_details { get; set; } = 0;
        public List<DetailRecord>? total_record_detail { get; set; }
        public int total_page { get; set; } = 0;
        public int page_size { get; set; } = 10;
        public int page { get; set; } = 1;
        public int total_qty { get; set; } = 0;

        public int setTotalRecord
        {
            set
            {
                this.total_record = value;
                this.total_page = (int)Math.Ceiling((double)this.total_record / this.page_size);
            }
        }
    }
    public class DetailRecord
    {
        public int id { get; set; } = 0;
        public int total_details_record { get; set; } = 0;
    }
}
