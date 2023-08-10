using ITI_Libraly_Api.Contexts;
using ITI_Libraly_Api.Models;
using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using ITI_Libraly_Api.Utilities;

namespace ITI_Libraly_Api.Service.Class
{
    public class ReturnReadService : IReturnReadService
    {
        ITI_LibraySystemContext db;
        public ReturnReadService(ITI_LibraySystemContext db)
        {
            this.db = db;
        }
        public async Task<Data> GetReturnReadInfo(int RetReadId)
        {
            Data model = new Data();
            try
            {
                var listSingle = db.TblReturnRead.Where(w => w.RetReadId == RetReadId).Select(s => new
                {
                    s.RetReadId,
                    Date = Convert.ToDateTime(s.RetReadDate).ToString("dd-MM-yyyy"),
                    UseName = s.UseDelId == null ? null : (from u in db.TblUseLogintime
                                                           join ud in db.TblItiUser on u.UseId equals ud.UseId
                                                           where u.UseDelId == s.UseDelId
                                                           select ud.UseName).FirstOrDefault(),
                    UseEditDate = Convert.ToDateTime(s.UseEditDate).ToString("dd-MM-yyyy,ម៉ោង h'h':mm'mm':ss'ss'"),
                    returnReadDetailsModels = (from redd in db.TblItiReturnReadDetails
                                               join red in db.TblItiReadDetails on redd.ReadDelId equals red.ReadDelId
                                               join r in db.TblItiRead on red.ReadId equals r.ReadId
                                               join bkd in db.TblItiBookdetails on red.BookDelId equals bkd.BookDelId
                                               where redd.RetReadId == RetReadId
                                               select new
                                               {
                                                   redd.RetDelReadId,
                                                   r.StuId,
                                                   StuName = r.StuId == null ? "" : db.TblItiStudent.Where(w => w.StuId == r.StuId).FirstOrDefault().StuName,
                                                   red.ReadDelId,
                                                   bkd.BookId,
                                                   BookNameKh = bkd.BookId == null ? "" : db.TblItiBook.Where(w => w.BookId == bkd.BookId).FirstOrDefault().BookNameKh,
                                                   bkd.ImpId,
                                                   ImpDate = bkd.ImpId == null ? null : db.TblItiImport.Where(w=>w.ImpId == bkd.ImpId).FirstOrDefault().ImpDate,
                                                   bkd.BookDelLabel,
                                                   StatusName = db.TblItiStatus.Where(w=>w.StatusId == redd.StatusId).FirstOrDefault().StatusName,
                                                   StatusPresent = db.TblItiStatus.Where(w=>w.StatusId == bkd.StatusId).FirstOrDefault().StatusName,
                                               }).ToList(),
                }).SingleOrDefault();
                model.data = listSingle;
                model.success = true;
                }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return model;
        }

        public async Task<Data> GetReturnReadList(int pageSkip = 0, int pageSize = 10, bool isSearch = false, string? startDate = null, string? endDate = null, int StuId = 0, string? BookDelId = null)
        {
            Data model = new Data();
            var startRetRead = ConvertDate.StringToDateTime(startDate);
            var endRetRead = ConvertDate.StringToDateTime(endDate);
            try
            {
                if(isSearch == false)
                {
                    var list = db.TblReturnRead/*.Where(w => w.RetReadId == RetReadId)*/.Select(s => new
                    {
                        s.RetReadId,
                        Date = Convert.ToDateTime(s.RetReadDate).ToString("dd-MM-yyyy"),
                        UseName = s.UseDelId == null ? null : (from u in db.TblUseLogintime
                                                               join ud in db.TblItiUser on u.UseId equals ud.UseId
                                                               where u.UseDelId == s.UseDelId
                                                               select ud.UseName).FirstOrDefault(),
                        UseEditDate = Convert.ToDateTime(s.UseEditDate).ToString("dd-MM-yyyy,ម៉ោង h'h':mm'mm':ss'ss'"),
                        returnReadDetailsModels = (from redd in db.TblItiReturnReadDetails
                                                   join red in db.TblItiReadDetails on redd.ReadDelId equals red.ReadDelId
                                                   join r in db.TblItiRead on red.ReadId equals r.ReadId
                                                   join bkd in db.TblItiBookdetails on red.BookDelId equals bkd.BookDelId
                                                   where redd.RetReadId == s.RetReadId
                                                   select new
                                                   {
                                                       redd.RetDelReadId,
                                                       r.StuId,
                                                       StuName = r.StuId == null ? "" : db.TblItiStudent.Where(w => w.StuId == r.StuId).FirstOrDefault().StuName,
                                                       red.ReadDelId,
                                                       bkd.BookId,
                                                       BookNameKh = bkd.BookId == null ? "" : db.TblItiBook.Where(w => w.BookId == bkd.BookId).FirstOrDefault().BookNameKh,
                                                       bkd.ImpId,
                                                       ImpDate = bkd.ImpId == null ? null : db.TblItiImport.Where(w => w.ImpId == bkd.ImpId).FirstOrDefault().ImpDate,
                                                       bkd.BookDelLabel,
                                                       StatusName = db.TblItiStatus.Where(w => w.StatusId == redd.StatusId).FirstOrDefault().StatusName,
                                                       StatusPresent = db.TblItiStatus.Where(w => w.StatusId == bkd.StatusId).FirstOrDefault().StatusName,
                                                   }).ToList(),
                    }).OrderByDescending(o=>o.RetReadId).Skip(pageSkip).Take(pageSize).ToList();
                    model.data = list;
                    model.success = true;
                    model.setTotalRecord = db.TblReturnRead.Count();
                }
                else
                {
                    var list = db.TblReturnRead.Where(w => (startDate == null || w.RetReadDate>=startRetRead)
                    &&(endDate == null || w.RetReadDate <= endRetRead)).Select(s => new
                    {
                        s.RetReadId,
                        Date = Convert.ToDateTime(s.RetReadDate).ToString("dd-MM-yyyy"),
                        UseName = s.UseDelId == null ? null : (from u in db.TblUseLogintime
                                                               join ud in db.TblItiUser on u.UseId equals ud.UseId
                                                               where u.UseDelId == s.UseDelId
                                                               select ud.UseName).FirstOrDefault(),
                        UseEditDate = Convert.ToDateTime(s.UseEditDate).ToString("dd-MM-yyyy,ម៉ោង h'h':mm'mm':ss'ss'"),
                        returnReadDetailsModels = (from redd in db.TblItiReturnReadDetails
                                                   join red in db.TblItiReadDetails on redd.ReadDelId equals red.ReadDelId
                                                   join r in db.TblItiRead on red.ReadId equals r.ReadId
                                                   join bkd in db.TblItiBookdetails on red.BookDelId equals bkd.BookDelId
                                                   where (redd.RetReadId == s.RetReadId)
                                                   &&(StuId == 0 || r.StuId == StuId)
                                                   && (BookDelId == null || bkd.BookDelId == BookDelId)
                                                   select new
                                                   {
                                                       redd.RetDelReadId,
                                                       r.StuId,
                                                       StuName = r.StuId == null ? "" : db.TblItiStudent.Where(w => w.StuId == r.StuId).FirstOrDefault().StuName,
                                                       red.ReadDelId,
                                                       bkd.BookId,
                                                       BookNameKh = bkd.BookId == null ? "" : db.TblItiBook.Where(w => w.BookId == bkd.BookId).FirstOrDefault().BookNameKh,
                                                       bkd.ImpId,
                                                       ImpDate = bkd.ImpId == null ? null : db.TblItiImport.Where(w => w.ImpId == bkd.ImpId).FirstOrDefault().ImpDate,
                                                       bkd.BookDelLabel,
                                                       StatusName = db.TblItiStatus.Where(w => w.StatusId == redd.StatusId).FirstOrDefault().StatusName,
                                                       StatusPresent = db.TblItiStatus.Where(w => w.StatusId == bkd.StatusId).FirstOrDefault().StatusName,
                                                   }).ToList(),
                    }).OrderByDescending(o => o.RetReadId).Skip(pageSkip).Take(pageSize).ToList();
                    model.data = list;
                    model.success = true;
                    model.setTotalRecord = db.TblReturnRead.Where(w => (startDate == null || w.RetReadDate>=startRetRead)
                    &&(endDate == null || w.RetReadDate <= endRetRead)).Count();
                }
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return model;
        }

        public async Task<Data> PostReturnReadInfo(ReturnReadModel rrm)
        {
            var retReadDate = ConvertDate.StringToDateTime(rrm.RetReadDate);
            Data model = new Data();
            try
            {
                if (rrm.RetReadId == 0)
                {
                    var check = db.TblReturnRead.Where(w => w.RetReadDate == retReadDate).ToList();
                    if( check.Count == 0)
                    {
                        TblReturnRead addrr = new TblReturnRead();
                        addrr.RetReadDate = retReadDate;
                        addrr.UseDelId = rrm.UseDelId;
                        addrr.UseEditDate = DateTime.Now;
                        db.TblReturnRead.Add(addrr);
                        db.SaveChanges();
                        foreach (var item in rrm.returnReadDetailsModels)
                        {
                            TblItiReturnReadDetails addrrd = new TblItiReturnReadDetails();
                            addrrd.RetReadId = addrr.RetReadId;
                            addrrd.ReadDelId = item.ReadDelId;
                            addrrd.StatusId = 4;
                            db.TblItiReturnReadDetails.Add(addrrd);
                            var upStatus = (from red in db.TblItiReadDetails
                                            join bd in db.TblItiBookdetails on red.BookDelId equals bd.BookDelId
                                            where red.ReadDelId == addrrd.ReadDelId
                                            select new { bd, red }).SingleOrDefault();
                            //foreach (var itemUp in upStatus)
                            //{
                            if (upStatus != null)
                            {
                                upStatus.bd.StatusId = 1;
                                db.SaveChanges();
                            }
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
                    var upRetRead = db.TblReturnRead.Where(w => w.RetReadId == rrm.RetReadId).SingleOrDefault();
                    if (upRetRead != null)
                    {
                        upRetRead.RetReadDate = retReadDate;
                        upRetRead.UseDelId = rrm.UseDelId;
                        upRetRead.UseEditDate = DateTime.Now;
                        // Remove
                        var removeRetReadDel = db.TblItiReturnReadDetails.Where(w => w.RetReadId == upRetRead.RetReadId).ToList();
                        db.TblItiReturnReadDetails.RemoveRange(removeRetReadDel);
                        var upStatus = (from rd in db.TblItiReadDetails
                                        join bd in db.TblItiBookdetails on rd.BookDelId equals bd.BookDelId
                                        join rrd in db.TblItiReturnReadDetails on rd.ReadDelId equals rrd.ReadDelId
                                        where rrd.RetReadId == upRetRead.RetReadId
                                        select new { rd,bd,rrd}).ToList();
                        foreach (var itemUp in upStatus)
                        {
                            if(upStatus != null)
                            {
                                itemUp.bd.StatusId = 2;
                                db.SaveChanges();
                            }
                        }
                        // Add Details
                        foreach (var item in rrm.returnReadDetailsModels)
                        {
                            TblItiReturnReadDetails addrrd = new TblItiReturnReadDetails();
                            addrrd.RetReadId = upRetRead.RetReadId;
                            addrrd.ReadDelId = item.ReadDelId;
                            addrrd.StatusId = 4;
                            db.TblItiReturnReadDetails.Add(addrrd);
                            var upStatus1 = (from red in db.TblItiReadDetails
                                            join bd in db.TblItiBookdetails on red.BookDelId equals bd.BookDelId
                                            where red.ReadDelId == addrrd.ReadDelId
                                            select new { bd, red }).SingleOrDefault();
                            //foreach (var itemUp in upStatus)
                            //{
                            if (upStatus1 != null)
                            {
                                upStatus1.bd.StatusId = 1;
                                db.SaveChanges();
                            }
                        }
                        db.SaveChanges();
                        model.success = true;
                        model.message = MessageStatus.UpdateOK;
                    }
                }
                model.data = (from rd in db.TblItiReadDetails
                              join bd in db.TblItiBookdetails on rd.BookDelId equals bd.BookDelId
                              where bd.StatusId == 2
                              select new
                              {
                                  rd.ReadDelId,
                                  rd.ReadId,
                                  bd.BookDelId,
                                  bd.BookId,
                                  BookNameKh = bd.BookId == null ? "" : db.TblItiBook.Where(w => w.BookId == bd.BookId).FirstOrDefault().BookNameKh,
                                  bd.ImpId,
                                  ImpDate = bd.ImpId == null ? null : Convert.ToDateTime(db.TblItiImport.Where(w => w.ImpId == bd.ImpId).FirstOrDefault().ImpDate).ToString("dd-MM-yyyy"),
                                  bd.BookDelLabel,
                                  StatusName = db.TblItiStatus.Where(w => w.StatusId == rd.StatusId).FirstOrDefault().StatusName,
                                  StatusNameReturn = (from rn in db.TblItiReturnReadDetails
                                                      join st in db.TblItiStatus on rn.StatusId equals st.StatusId
                                                      where rn.ReadDelId == rd.ReadDelId
                                                      select st.StatusName).FirstOrDefault(),
                                  bd.StatusId,
                                  StatusPresent = bd.StatusId == null ? "" : db.TblItiStatus.Where(w => w.StatusId == bd.StatusId).FirstOrDefault().StatusName
                              }).ToList();
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
