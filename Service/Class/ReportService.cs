using ITI_Libraly_Api.Contexts;
using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using ITI_Libraly_Api.Utilities;

namespace ITI_Libraly_Api.Service.Class
{
    public class ReportService : IReportService
    {
        ITI_LibraySystemContext db;
        public ReportService(ITI_LibraySystemContext db)
        {
            this.db = db;
        }
        public async Task<Data> ReportReading(DateTime? starDate = null, DateTime? endDate = null, long id = 0)
        {
            Data model = new Data();
            try
            {
                if (starDate != null || endDate != null)
                {
                    var result = (from r in db.TblItiRead
                                  join stu in db.TblItiStudent on r.StuId equals stu.StuId
                                  join pos in db.TblItiPosition on stu.PosId equals pos.PosId
                                  join type in db.TblTimeType on r.TimeTypeId equals type.TimeTypeId
                                  where (id == 0 || r.ReadId == id)
                                  && (starDate == null || r.ReadDate >= starDate.Value.Date)
                                  && (endDate == null || r.ReadDate <= endDate.Value.Date)
                                  select new
                                  {
                                      Date = starDate == null || endDate == null ? null : "ចាប់ពីថ្ងៃទី " + starDate!.Value.Day + " ខែ " + ConvertMonth.Month(starDate!.Value.Month) + " ឆ្នាំ " + starDate!.Value.Year​ +
                        "ដល់ថ្ងៃទី​ " + endDate!.Value.Day + " ខែ " + ConvertMonth.Month(endDate!.Value.Month) + " ឆ្នាំ " + endDate!.Value.Year​,
                                      ReadDate = r.ReadDate.Value.Date.ToString("dd-MM-yyyy"),
                                      r.StartTime,
                                      r.EndTime,
                                      r.StuId,
                                      stu.StuName,
                                      stu.StuSex,
                                      pos.PosNameKh,
                                      r.TimeTypeId,
                                      type.TimeType,

                                  }).ToList();
                    model.data = result;
                    model.setTotalRecord = result.Count;
                    model.total_record_details = (from r in db.TblItiRead
                                          join stu in db.TblItiStudent on r.StuId equals stu.StuId
                                          join pos in db.TblItiPosition on stu.PosId equals pos.PosId
                                          where (id == 0 || r.ReadId == id)
                                          && (starDate == null || r.ReadDate >= starDate.Value.Date)
                                          && (endDate == null || r.ReadDate <= endDate.Value.Date)
                                          && (stu.StuSex.Equals("ប្រុស"))
                                          select r).Count();
                    model.total_qty = (from r in db.TblItiRead
                                       join stu in db.TblItiStudent on r.StuId equals stu.StuId
                                       join pos in db.TblItiPosition on stu.PosId equals pos.PosId
                                       where (id == 0 || r.ReadId == id)
                                       && (starDate == null || r.ReadDate >= starDate.Value.Date)
                                       && (endDate == null || r.ReadDate <= endDate.Value.Date)
                                       && (stu.StuSex.Equals("ស្រី"))
                                       select r).Count();
                    model.success = true;
                }
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.ToString();
            }
            return model;
        }
    }
}
