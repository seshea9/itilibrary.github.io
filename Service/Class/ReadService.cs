using ITI_Libraly_Api.Contexts;
using ITI_Libraly_Api.Models;
using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using ITI_Libraly_Api.Utilities;
using System.Diagnostics.Metrics;
using System.Threading;

namespace ITI_Libraly_Api.Service.Class
{
    public class ReadService : IReadService
    {
        ITI_LibraySystemContext db;
        public ReadService(ITI_LibraySystemContext db)
        {
            this.db = db;
        }
        public async Task<Data> GetReadInfo(int ReadId)
        {
            Data model = new Data();
            try
            {
                var result = db.TblItiRead.Where(w => w.ReadId == ReadId).Select(
                    s=> new
                    {
                        s.ReadId,
                        s.StuId,
                        StuName = s.StuId == null ? "" : db.TblItiStudent.Where(w=>w.StuId == s.StuId).FirstOrDefault().StuName,
                        ReadDate = Convert.ToDateTime(s.ReadDate).ToString("dd-MM-yyyy"),
                        s.StartTime,
                        s.EndTime,
                        s.TimeTypeId,
                        TimeType = s.TimeTypeId == null ? null : db.TblTimeType.Where(w=>w.TimeTypeId == s.TimeTypeId).FirstOrDefault().TimeType,
                        UseName = s.UseDelId == null ? null : (from u in db.TblUseLogintime
                                                               join ud in db.TblItiUser on u.UseId equals ud.UseId
                                                               join emp in db.TblItiEmployee on ud.EmpId equals emp.EmpId
                                                               where u.UseDelId == s.UseDelId
                                                               select emp.EmpNameKh).FirstOrDefault(),
                        UseEditDate = Convert.ToDateTime(s.UseEditDate).ToString("dd-MM-yyyy"),
                        s.IsReturn,
                        readDeltailsModels= (from rd in db.TblItiReadDetails 
                                                            join bd in db.TblItiBookdetails on rd.BookDelId equals bd.BookDelId
                                                            join b in db.TblItiBook on bd.BookId equals b.BookId
                                                            where rd.ReadId == s.ReadId /*&& bd.StatusId ==2*/
                                                            select new
                                                            {
                                                                rd.ReadDelId,
                                                                rd.ReadId,
                                                                bd.BookDelId,
                                                                bd.BookId,
                                                                BookNameKh = bd.BookId == null ? "" : db.TblItiBook.Where(w=>w.BookId == bd.BookId).FirstOrDefault().BookNameKh,
                                                                bd.ImpId,
                                                                ImpDate = bd.ImpId == null ? null : Convert.ToDateTime(db.TblItiImport.Where(w=>w.ImpId == bd.ImpId).FirstOrDefault().ImpDate).ToString("dd-MM-yyyy"),
                                                                //bd.BookDelLabel,
                                                                BookDelLabel = bd.ImpId == null ? null : "Loc:" + db.TblItiLocation.Where(w => w.LocId == b.LocId).FirstOrDefault().LocLabel + "/Imp:" + Convert.ToDateTime(db.TblItiImport.Where(w => w.ImpId == bd.ImpId).FirstOrDefault().ImpDate).ToString("dd-MM-yyyy") + "/No:" + bd.BookDelLabel,
                                                                bd.StatusId,
                                                                StatusName = db.TblItiStatus.Where(w => w.StatusId == rd.StatusId).FirstOrDefault().StatusName,
                                                                StatusPresent = bd.StatusId == null ? "" : db.TblItiStatus.Where(w=>w.StatusId== bd.StatusId).FirstOrDefault().StatusName
                                                            }).ToList(),
                    }).SingleOrDefault();
                model.data = result;
                model.success = true;
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return model;
        }

        public async Task<Data> GetReadList(/*int pageSkip = 0, int pageSize = 10,*/ bool isSearch = false, int? StuId = 0, string? startDate = null, string? endDate = null)
        {
            Data model = new Data();
            var stDate = ConvertDate.StringToDateTime(startDate);
            var enDate = ConvertDate.StringToDateTime(endDate);
            try
            {
                if(isSearch == false)
                {
                    var list = db.TblItiRead./*Where(w => w.ReadId == ReadId).*/Select(
                    s => new
                    {
                        s.ReadId,
                        s.StuId,
                        StuName = s.StuId == null ? "" : db.TblItiStudent.Where(w => w.StuId == s.StuId).FirstOrDefault().StuName,
                        ReadDate = Convert.ToDateTime(s.ReadDate).ToString("dd-MM-yyyy"),
                        s.StartTime,
                        s.EndTime,
                        s.TimeTypeId,
                        TimeType = s.TimeTypeId == null ? null : db.TblTimeType.Where(w => w.TimeTypeId == s.TimeTypeId).FirstOrDefault().TimeType,
                        UseName = s.UseDelId == null ? null : (from u in db.TblUseLogintime
                                                               join ud in db.TblItiUser on u.UseId equals ud.UseId
                                                               join emp in db.TblItiEmployee on ud.EmpId equals emp.EmpId
                                                               where u.UseDelId == s.UseDelId
                                                               select emp.EmpNameKh).FirstOrDefault(),
                        s.IsReturn,
                        UseEditDate = Convert.ToDateTime(s.UseEditDate).ToString("dd-MM-yyyy"),
                        readDeltailsModels = (from rd in db.TblItiReadDetails
                                              join bd in db.TblItiBookdetails on rd.BookDelId equals bd.BookDelId
                                              join b in db.TblItiBook on bd.BookId equals b.BookId
                                              where rd.ReadId == s.ReadId /*&& bd.StatusId == 2*/
                                              select new
                                              {
                                                  rd.ReadDelId,
                                                  rd.ReadId,
                                                  bd.BookDelId,
                                                  bd.BookId,
                                                  BookNameKh = bd.BookId == null ? "" : db.TblItiBook.Where(w => w.BookId == bd.BookId).FirstOrDefault().BookNameKh,
                                                  bd.ImpId,
                                                  ImpDate = bd.ImpId == null ? null : Convert.ToDateTime(db.TblItiImport.Where(w => w.ImpId == bd.ImpId).FirstOrDefault().ImpDate).ToString("dd-MM-yyyy"),
                                                  BookDelLabel= bd.ImpId == null ? null : "Loc:" + db.TblItiLocation.Where(w => w.LocId == b.LocId).FirstOrDefault().LocLabel + "/Imp:" + Convert.ToDateTime(db.TblItiImport.Where(w => w.ImpId == bd.ImpId).FirstOrDefault().ImpDate).ToString("dd-MM-yyyy") + "/No:" + bd.BookDelLabel,
                                                  StatusName = db.TblItiStatus.Where(w => w.StatusId == rd.StatusId).FirstOrDefault().StatusName,
                                                  //StatusNameReturn = (from rn in db.TblItiReturnReadDetails
                                                  //                    join st in db.TblItiStatus on rn.StatusId equals st.StatusId
                                                  //                    where rn.ReadDelId == rd.ReadDelId
                                                  //                    select st.StatusName).FirstOrDefault(),
                                                  StatusNameReturn = s.IsReturn,
                                                  bd.StatusId,
                                                  StatusPresent = bd.StatusId == null ? "" : db.TblItiStatus.Where(w => w.StatusId == bd.StatusId).FirstOrDefault().StatusName
                                              }).ToList(),
                    }).OrderByDescending(o=>o.ReadId)/*.Skip(pageSkip).Take(pageSize)*/.ToList();
                    model.data = list;
                    model.success = true;
                    model.setTotalRecord = db.TblItiRead.Count();
                }
                else
                {
                    var search = db.TblItiRead.Where(w => (StuId == 0 || w.StuId.Equals(StuId))
                    && (startDate == null || w.ReadDate >= stDate)
                    &&(endDate == null || w.ReadDate <= enDate)).Select(
                    s => new
                    {
                        s.ReadId,
                        s.StuId,
                        StuName = s.StuId == null ? "" : db.TblItiStudent.Where(w => w.StuId == s.StuId).FirstOrDefault().StuName,
                        ReadDate = Convert.ToDateTime(s.ReadDate).ToString("dd-MM-yyyy"),
                        s.StartTime,
                        s.EndTime,
                        s.TimeTypeId,
                        TimeType = s.TimeTypeId == null ? null : db.TblTimeType.Where(w => w.TimeTypeId == s.TimeTypeId).FirstOrDefault().TimeType,
                        UseName = s.UseDelId == null ? null : (from u in db.TblUseLogintime
                                                               join ud in db.TblItiUser on u.UseId equals ud.UseId
                                                               join emp in db.TblItiEmployee on ud.EmpId equals emp.EmpId
                                                               where u.UseDelId == s.UseDelId
                                                               select emp.EmpNameKh).FirstOrDefault(),
                        s.IsReturn,
                        UseEditDate = Convert.ToDateTime(s.UseEditDate).ToString("dd-MM-yyyy"),
                        readDeltailsModels = (from rd in db.TblItiReadDetails
                                              join bd in db.TblItiBookdetails on rd.BookDelId equals bd.BookDelId
                                              join b in db.TblItiBook on bd.BookId equals b.BookId
                                              where rd.ReadId == s.ReadId /*&& bd.StatusId == 2*/
                                              select new
                                              {
                                                  rd.ReadDelId,
                                                  rd.ReadId,
                                                  bd.BookDelId,
                                                  bd.BookId,
                                                  BookNameKh = bd.BookId == null ? "" : db.TblItiBook.Where(w => w.BookId == bd.BookId).FirstOrDefault().BookNameKh,
                                                  bd.ImpId,
                                                  ImpDate = bd.ImpId == null ? null : Convert.ToDateTime(db.TblItiImport.Where(w => w.ImpId == bd.ImpId).FirstOrDefault().ImpDate).ToString("dd-MM-yyyy"),
                                                  BookDelLabel = bd.ImpId == null ? null : "Loc:" + db.TblItiLocation.Where(w => w.LocId == b.LocId).FirstOrDefault().LocLabel + "/Imp:" + Convert.ToDateTime(db.TblItiImport.Where(w => w.ImpId == bd.ImpId).FirstOrDefault().ImpDate).ToString("dd-MM-yyyy") + "/No:" + bd.BookDelLabel,
                                                  StatusName = db.TblItiStatus.Where(w => w.StatusId == rd.StatusId).FirstOrDefault().StatusName,
                                                  //StatusNameReturn = (from rn in db.TblItiReturnReadDetails
                                                  //                    join st in db.TblItiStatus on rn.StatusId equals st.StatusId
                                                  //                    where rn.ReadDelId == rd.ReadDelId
                                                  //                    select st.StatusName).FirstOrDefault(),
                                                  StatusNameReturn = s.IsReturn,
                                                  bd.StatusId,
                                                  StatusPresent = bd.StatusId == null ? "" : db.TblItiStatus.Where(w => w.StatusId == bd.StatusId).FirstOrDefault().StatusName
                                              }).ToList(),
                    }).OrderByDescending(o => o.ReadId)/*.Skip(pageSkip).Take(pageSize)*/.ToList();
                    model.data = search;
                    model.success = true;
                    model.setTotalRecord = db.TblItiRead.Where(w => (StuId == 0 || w.StuId.Equals(StuId))
                    && (startDate == null || w.ReadDate >= stDate)
                    && (endDate == null || w.ReadDate <= enDate)).Count();
                }
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return model;
        }

        public async Task<Data> PostReadInfo(ReadModel read)
        {
            Data model = new Data();
            var readDate = ConvertDate.StringToDateTime(read.ReadDate);
            try
            {
                if(read.ReadId == 0)
                {
                    var check = db.TblItiRead.Where(w => w.ReadDate == readDate &&  w.StuId == read.StuId).ToList();
                    if (check.Count == 0)
                    {
                        TblItiRead addRead = new TblItiRead();
                        addRead.StuId = read.StuId;
                        addRead.ReadDate = readDate ;
                        addRead.StartTime = read.StartTime;
                        //addRead.EndTime = read.EndTime;
                        addRead.TimeTypeId = read.TimeTypeId;
                        addRead.UseDelId = read.UseDelId;
                        addRead.UseEditDate = DateTime.Now;
                        addRead.IsReturn = false;
                        db.TblItiRead.Add(addRead);
                        db.SaveChanges();
                        foreach(var item in read.readDeltailsModels)
                        {
                            TblItiReadDetails addReadDel = new TblItiReadDetails();
                            addReadDel.ReadId = addRead.ReadId;
                            addReadDel.BookDelId = item.BookDelId;
                            addReadDel.StatusId = 2;
                            db.TblItiReadDetails.Add(addReadDel);
                            db.SaveChanges();
                            var updateBookDel = db.TblItiBookdetails.Where(w => w.BookDelId == addReadDel.BookDelId).SingleOrDefault();
                            if(updateBookDel != null)
                            {
                                updateBookDel.StatusId = 2;
                            }
                            db.SaveChanges();
                        }
                        db.SaveChanges();
                        model.success = true;
                        model.message = MessageStatus.SaveOK;
                    }
                    else
                    {
                        model.success = false;
                        model.message = MessageStatus.UnSave;
                    }
                }
                else
                {
                    var updateCheck = db.TblItiRead.Where(w => w.ReadId == read.ReadId).SingleOrDefault();
                    if (updateCheck.IsReturn == true)
                    {
                        model.success = false;
                        model.message = "ទិន្នន័យបានសងត្រឡប់មកវិញហើយ!​ អ្នកមិនអាចកែប្រែទិន្នន័យនេះបានទេ";
                        return model;
                    }
                    if (read.StatusId == 2)
                    {
                        var upRead = db.TblItiRead.Where(w => w.ReadId == read.ReadId).SingleOrDefault();
                        if (upRead != null)
                        {
                            upRead.StuId = read.StuId;
                            upRead.ReadDate = ConvertDate.StringToDateTime(read.ReadDate);
                            upRead.StartTime = read.StartTime;
                            upRead.EndTime = read.EndTime;
                            upRead.TimeTypeId = read.TimeTypeId;
                            upRead.UseDelId = read.UseDelId;
                            upRead.UseEditDate = DateTime.Now;
                            // Remove
                            var delReadDel = db.TblItiReadDetails.Where(w => w.ReadId == upRead.ReadId).ToList();
                            db.TblItiReadDetails.RemoveRange(delReadDel);

                            var upStatus = (from rd in db.TblItiReadDetails
                                            join bd in db.TblItiBookdetails on rd.BookDelId equals bd.BookDelId
                                            where rd.ReadId == upRead.ReadId
                                            select new { rd, bd }).ToList();
                            foreach (var itemUp in upStatus)
                            {
                                if (itemUp != null)
                                {
                                    itemUp.bd.StatusId = 1;
                                    db.SaveChanges();
                                }
                            }
                            foreach (var item in read.readDeltailsModels)
                            {

                                TblItiReadDetails addReadDel = new TblItiReadDetails();
                                addReadDel.ReadId = upRead.ReadId;
                                addReadDel.BookDelId = item.BookDelId;
                                addReadDel.StatusId = 2;
                                db.TblItiReadDetails.Add(addReadDel);
                                db.SaveChanges();
                                var updateBookDel = db.TblItiBookdetails.Where(w => w.BookDelId == addReadDel.BookDelId).Single();
                                if (updateBookDel != null)
                                {
                                    updateBookDel.StatusId = 2;
                                }
                                db.SaveChanges();
                            }
                            db.SaveChanges();
                            model.success = true;
                            model.message = MessageStatus.UpdateOK;
                        }
                    }
                    if(read.StatusId == 4)
                    {
                        var update = db.TblItiRead.Where(w => w.ReadId == read.ReadId).SingleOrDefault();
                        if(update != null)
                        {
                            update.IsReturn = true;
                            update.EndTime = DateTime.Now.Hour.ToString()+":"+ DateTime.Now.Minute.ToString("00");
                            foreach (var item in read.readDeltailsModels)
                            {
                                var upStatus = (from rd in db.TblItiReadDetails
                                                join bd in db.TblItiBookdetails on rd.BookDelId equals bd.BookDelId
                                                where rd.ReadId == update.ReadId
                                                select new { rd, bd }).ToList();
                                foreach (var itemUp in upStatus)
                                {
                                    if (itemUp != null)
                                    {
                                        itemUp.bd.StatusId = 1;
                                        db.SaveChanges();
                                    }
                                }
                            }
                            db.SaveChanges();
                            model.success = true;
                            model.message = "សំណើរស្នើរសុំ!​សងមកវិញ​ ទទួលបានជោគជ័យ";
                        }
                    }
                }
                //model.data = db.TblItiBookdetails.Where(w => w.StatusId == 1).Select(s => new
                //{
                //    s.BookDelId,
                //    s.BookId,
                //    BookNameKh = s.BookId == null ? "" : db.TblItiBook.Where(w => w.BookId == s.BookId).FirstOrDefault().BookNameKh,
                //    s.ImpId,
                //    ImpDate = s.ImpId == null ? null : Convert.ToDateTime(db.TblItiImport.Where(w => w.ImpId == s.ImpId).FirstOrDefault().ImpDate).ToString("dd-MM-yyyy"),
                //    s.BookDelLabel,
                //    s.StatusId,
                //    StatusPresent = s.StatusId == null ? "" : db.TblItiStatus.Where(w => w.StatusId == s.StatusId).FirstOrDefault().StatusName
                //}).ToList();
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return model;
        }
    }
}
