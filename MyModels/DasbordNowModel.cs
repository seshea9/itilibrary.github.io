using ITI_Libraly_Api.Models;

namespace ITI_Libraly_Api.MyModels
{
    public class DasbordNowModel
    {
        public int ReadQty { get; set; }
        public int BorrowQty { get; set; }
        public int ReturnQty { get; set; }
        public ReturnIn? ReturnIn { get; set; }
    }
    public class ReturnIn
    {
        public int ReturnInQty { get; set; }
        public List<Detail>? Details { get; set; }
    }
    public class Detail : BorrowModel
    {
        public Student? Student { get; set; }
    }
    public class Student
    {
        public int StuId { get; set; }
        public string? StuName { get; set; }
        public string? StuSex { get; set; }
        public string? StuAddress { get; set; }
        public string? StuPhone { get; set; }
        public int? PosId { get; set; }
        public string? StuCode { get; set; }
        public bool? IsPrintCart { get; set; }
        public DateTime? StuDob { get; set; }
        public int? NattionalId { get; set; }
    }
}
