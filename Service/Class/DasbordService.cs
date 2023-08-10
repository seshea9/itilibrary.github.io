using ITI_Libraly_Api.Contexts;
using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;

namespace ITI_Libraly_Api.Service.Class
{
    public class DasbordService : IDasbordService
    {
        ITI_LibraySystemContext db;
        public DasbordService(ITI_LibraySystemContext db)
        {
            this.db = db;
        }
        public async Task<Data> GetDasbordList()
        {
            Data data = new Data();
            try
            {

                List<DasbordListModel> result = new List<DasbordListModel>();
                DasbordListModel employee = new DasbordListModel()
                {
                    Description = "បុគ្គលិក/គ្រូ",
                    TotalQty = db.TblItiEmployee.Count().ToString()+ "​ នាក់",
                    Male = db.TblItiEmployee.Where(w => w.EmpSex.Equals("ប្រុស")).Count().ToString() +" នាក់",
                    Female = db.TblItiEmployee.Where(w => w.EmpSex.Equals("ស្រី")).Count().ToString() + "​​ នាក់",
                };
                DasbordListModel student = new DasbordListModel()
                {
                    Description = "សិស្ស/អតិថិជន",
                    TotalQty = db.TblItiStudent.Count().ToString() + "នាក់",
                    Male = db.TblItiStudent.Where(w => w.StuSex.Equals("ប្រុស")).Count().ToString() + " នាក់",
                    Female = db.TblItiStudent.Where(w => w.StuSex.Equals("ស្រី")).Count().ToString() + " នាក់",
                };
                DasbordListModel book = new DasbordListModel()
                {
                    Description = "សៀវភៅ",
                    SubjectQty = db.TblItiBook.Count().ToString() + " ក្បាល",
                    TotalQty = db.TblItiBookdetails.Count().ToString() + " ក្បាល",
                    Single = (from b in db.TblItiBook
                              join bd in db.TblItiBookdetails on b.BookId equals bd.BookId
                              where bd.StatusId ==1 select b).Count().ToString() + " ក្បាល",
                    Reading = (from b in db.TblItiBook
                              join bd in db.TblItiBookdetails on b.BookId equals bd.BookId
                              where bd.StatusId == 2
                              select b).Count().ToString() + " ក្បាល",
                    Borrow = (from b in db.TblItiBook
                               join bd in db.TblItiBookdetails on b.BookId equals bd.BookId
                               where bd.StatusId == 3
                               select b).Count().ToString() + " ក្បាល",
                };
                DasbordListModel cate = new DasbordListModel()
                {
                    Description = "ប្រភេទសៀវភៅ",
                    TotalQty = db.TblItiCategory.Count().ToString(),
                };
                DasbordListModel loc = new DasbordListModel()
                {
                    Description = "ទីតាំង ឬ ទូដាក់សៀវភៅ",
                    TotalQty = db.TblItiLocation.Count().ToString(),
                };
                result.Add(book);
                result.Add(employee);
                result.Add(student);
                result.Add(cate);
                result.Add(loc);
                data.data = result;
                data.success = true;
            }
            catch(Exception ex)
            {
                data.success = false;
                data.message = ex.Message;
            }
            return data;
        }

        public async Task<Data> GetDasbordNowList()
        {
            Data data = new Data();
            try
            {


                DasbordNowModel result = new DasbordNowModel()
                {
                    ReadQty = db.TblItiRead.Where(w => w.ReadDate == DateTime.Today).Count(),
                    BorrowQty = db.TblItiBorrow.Where(w => w.BorStartDate == DateTime.Today).Count(),
                    ReturnQty = (from r in db.TblItiReturn 
                                 join rd in db.TblItiReturndeltails on r.RetId equals rd.RetId
                                 where r.RetDate == DateTime.Today select r).Count(),
                    ReturnIn = new ReturnIn()
                    {
                        ReturnInQty = db.TblItiBorrow.Where(w => w.BorEndDate==DateTime.Today && w.IsRequired == false).Count(),
                        Details = db.TblItiBorrow.Where(w=>w.BorEndDate == DateTime.Today && w.IsRequired == false).Select(s => new Detail
                        {
                            BorCode = s.BorCode,
                            BorStartDate = s.BorStartDate,
                            BorEndDate = s.BorEndDate,
                            StuId = s.StuId,
                            Student = db.TblItiStudent.Where(w=>w.StuId == s.StuId).Select(s => new Student
                            {
                                StuName= s.StuName,
                                StuAddress= s.StuAddress,
                                StuSex= s.StuSex,
                                StuPhone= s.StuPhone,
                            }).FirstOrDefault(),
                        }).ToList(),
                    },
                };
                data.data = result;
                data.success = true;
            }
            catch (Exception ex)
            {
                data.success = false;
                data.message = ex.Message;
            }
            return data;
        }
    }
}
