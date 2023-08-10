using ITI_Libraly_Api.Contexts;
using ITI_Libraly_Api.Models;
using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using ITI_Libraly_Api.Utilities;

namespace ITI_Libraly_Api.Service.Class
{
    public class ReturnBorrowService : IReturnBorrowService
    {
        ITI_LibraySystemContext db;
        public ReturnBorrowService(ITI_LibraySystemContext db)
        {
            this.db = db;
        }

        public async Task<Data> GetReturnBorrowInfo(int RetId)
        {
            Data model = new Data();
            try
            {
                var listSingle = db.TblItiReturn.Where(w => w.RetId == RetId).Select(s => new
                {
                    s.RetId,
                    s.RetDate,
                    s.UseDelId,
                    UseName = s.UseDelId == null ? null : (from u in db.TblUseLogintime
                                                           join ud in db.TblItiUser on u.UseId equals ud.UseId
                                                           join emp in db.TblItiEmployee on ud.EmpId equals emp.EmpId
                                                           where u.UseDelId == s.UseDelId
                                                           select emp.EmpNameKh).FirstOrDefault(),
                    UseEditDate = Convert.ToDateTime(s.UseEditDate).ToString("dd-MM-yyyy,ម៉ោង h'h':mm'mm':ss'ss'"),
                    returndeltails = (from rn in db.TblItiReturndeltails
                                      //join bo in db.TblBorrowDetails on rn.BorId equals bo.BorId
                                      join st in db.TblItiBorrow on rn.BorId equals st.BorId
                                      //join bd in db.TblItiBookdetails on bo.BookDelId equals bd.BookDelId
                                      where rn.RetId == s.RetId
                                      select new
                                      {
                                          rn.BorId,
                                          rn.RetDelId,
                                          st.StuId,
                                          st.BorCode,
                                          st.BorStartDate,
                                          st.BorEndDate,
                                          StuName = st.StuId == null ? "" : db.TblItiStudent.Where(w => w.StuId == st.StuId).FirstOrDefault().StuName,
                                          StatusName = db.TblItiStatus.Where(w => w.StatusId == rn.StatusId).FirstOrDefault().StatusName,
                                          borrowDetails = (from bor in db.TblBorrowDetails
                                                           join bd in db.TblItiBookdetails on bor.BookDelId equals bd.BookDelId
                                                           join b in db.TblItiBook on bd.BookId equals b.BookId
                                                           where bor.BorId == st.BorId
                                                           select new
                                                           {
                                                               bor.BorDelId,
                                                               bor.BookDelId,
                                                               bd.BookId,
                                                               BookNameKh = bd.BookId == null ? "" : db.TblItiBook.Where(w => w.BookId == bd.BookId).FirstOrDefault().BookNameKh,
                                                               bd.ImpId,
                                                               BookDelLabel = bd.ImpId == null ? null : "Loc:" + db.TblItiLocation.Where(w => w.LocId == b.LocId).FirstOrDefault().LocLabel + "/Imp:" + Convert.ToDateTime(db.TblItiImport.Where(w => w.ImpId == bd.ImpId).FirstOrDefault().ImpDate).ToString("dd-MM-yyyy") + "/No:" + bd.BookDelLabel,
                                                               bd.StatusId,
                                                               StatusNameReturn = (from rn in db.TblItiReturndeltails
                                                                                   join st in db.TblItiStatus on rn.StatusId equals st.StatusId
                                                                                   where rn.BorId == bor.BorId
                                                                                   select st.StatusName).FirstOrDefault(),
                                                               StatusPresent = bd.StatusId == null ? "" : db.TblItiStatus.Where(w => w.StatusId == bd.StatusId).FirstOrDefault().StatusName
                                                           }).ToList(),
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

        public async Task<Data> GetReturnBorrowList(int pageSkip = 0, int pageSize = 10, bool isSearch = false, int? StuId = 0, DateTime? startDate = null, DateTime? endDate = null, string? BorCode = null,string? BookDelId = null)
        {
            Data model = new Data();
            try
            {
                if( isSearch == false)
                {
                    var list = db.TblItiReturn./*Where(w => w.RetId == RetId).*/Select(s => new
                    {
                        s.RetId,
                        s.RetDate,
                        s.UseDelId,
                        UseName = s.UseDelId == null ? null : (from u in db.TblUseLogintime
                                                               join ud in db.TblItiUser on u.UseId equals ud.UseId
                                                               join emp in db.TblItiEmployee on ud.EmpId equals emp.EmpId
                                                               where u.UseDelId == s.UseDelId
                                                               select emp.EmpNameKh).FirstOrDefault(),
                        UseEditDate = Convert.ToDateTime(s.UseEditDate).ToString("dd-MM-yyyy,ម៉ោង h'h':mm'mm':ss'ss'"),
                        returndeltails = (from rn in db.TblItiReturndeltails
                                          join st in db.TblItiBorrow on rn.BorId equals st.BorId
                                          where rn.RetId == s.RetId
                                          select new
                                          {
                                              rn.BorId,
                                              rn.RetDelId,
                                              st.StuId,
                                              st.BorCode,
                                              st.BorStartDate,
                                              st.BorEndDate,
                                              StuName = st.StuId == null ? "" : db.TblItiStudent.Where(w => w.StuId == st.StuId).FirstOrDefault().StuName,
                                              StatusName = db.TblItiStatus.Where(w => w.StatusId == rn.StatusId).FirstOrDefault().StatusName,
                                              borrowDetails = (from bor in db.TblBorrowDetails
                                                               join bd in db.TblItiBookdetails on bor.BookDelId equals bd.BookDelId
                                                               join b in db.TblItiBook on bd.BookId equals b.BookId
                                                               where bor.BorId == st.BorId
                                                               select new
                                                               {
                                                                   bor.BorDelId,
                                                                   bor.BookDelId,
                                                                   bd.BookId,
                                                                   BookNameKh = bd.BookId == null ? "" : db.TblItiBook.Where(w => w.BookId == bd.BookId).FirstOrDefault().BookNameKh,
                                                                   bd.ImpId,
                                                                   BookDelLabel = bd.ImpId == null ? null : "Loc:" + db.TblItiLocation.Where(w => w.LocId == b.LocId).FirstOrDefault().LocLabel + "/Imp:" + Convert.ToDateTime(db.TblItiImport.Where(w => w.ImpId == bd.ImpId).FirstOrDefault().ImpDate).ToString("dd-MM-yyyy") + "/No:" + bd.BookDelLabel,
                                                                   bd.StatusId,
                                                                   StatusNameReturn = (from rn in db.TblItiReturndeltails
                                                                                       join st in db.TblItiStatus on rn.StatusId equals st.StatusId
                                                                                       where rn.BorId == bor.BorId
                                                                                       select st.StatusName).FirstOrDefault(),
                                                                   StatusPresent = bd.StatusId == null ? "" : db.TblItiStatus.Where(w => w.StatusId == bd.StatusId).FirstOrDefault().StatusName
                                                               }).ToList(),
                                          }).ToList(),
                    }).OrderByDescending(o=>o.RetId).Skip(pageSkip).Take(pageSize).ToList();
                    model.data = list;
                    model.success = true;
                    model.setTotalRecord = db.TblItiReturn.Count();
                }
                else
                {
                    //var check = (from rn in db.TblItiReturndeltails
                    //            join bo in db.TblBorrowDetails on rn.BorId equals bo.BorId
                    //            join st in db.TblItiBorrow on rn.BorId equals st.BorId 
                    //            where st.StuId == StuId
                    //            select new { rn }).FirstOrDefault();
                    //if (check == null)
                    //{

                    //}
                    var check = (from rn in db.TblItiReturndeltails
                                 join b in db.TblItiBorrow on rn.BorId equals b.BorId
                                 where b.BorCode == BorCode
                                 select rn).FirstOrDefault();
                    if (check == null)
                    {
                        check = new TblItiReturndeltails(); 
                    }
                    var list = db.TblItiReturn.Where(w =>(startDate == null || w.RetDate >= startDate)
                    && (endDate == null || w.RetDate <= endDate)
                    && (BorCode == null || w.RetId == check.RetId)).Select(s => new
                    {
                        s.RetId,
                        s.RetDate,
                        s.UseDelId,
                        UseName = s.UseDelId == null ? null : (from u in db.TblUseLogintime
                                                               join ud in db.TblItiUser on u.UseId equals ud.UseId
                                                               join emp in db.TblItiEmployee on ud.EmpId equals emp.EmpId
                                                               where u.UseDelId == s.UseDelId
                                                               select emp.EmpNameKh).FirstOrDefault(),
                        UseEditDate = Convert.ToDateTime(s.UseEditDate).ToString("dd-MM-yyyy,ម៉ោង h'h':mm'mm':ss'ss'"),
                        returndeltails = (from rn in db.TblItiReturndeltails
                                              //join bo in db.TblBorrowDetails on rn.BorId equals bo.BorId
                                          join st in db.TblItiBorrow on rn.BorId equals st.BorId
                                          //join bd in db.TblItiBookdetails on bo.BookDelId equals bd.BookDelId
                                          where rn.RetId == s.RetId
                                          && (StuId == 0 || st.StuId == StuId)
                                          && (BorCode == null || st.BorCode == BorCode   )
                                          select new
                                          {
                                              rn.BorId,
                                              rn.RetDelId,
                                              st.StuId,
                                              st.BorCode,
                                              st.BorStartDate,
                                              st.BorEndDate,
                                              StuName = st.StuId == null ? "" : db.TblItiStudent.Where(w => w.StuId == st.StuId).FirstOrDefault().StuName,
                                              StatusName = db.TblItiStatus.Where(w => w.StatusId == rn.StatusId).FirstOrDefault().StatusName,
                                              borrowDetails = (from bor in db.TblBorrowDetails
                                                               join bd in db.TblItiBookdetails on bor.BookDelId equals bd.BookDelId
                                                               join b in db.TblItiBook on bd.BookId equals b.BookId
                                                               where bor.BorId == st.BorId
                                                               && (BookDelId == null || bd.BookDelId == BookDelId)
                                                               select new
                                                               {
                                                                   bor.BorDelId,
                                                                   bor.BookDelId,
                                                                   bd.BookId,
                                                                   BookNameKh = bd.BookId == null ? "" : db.TblItiBook.Where(w => w.BookId == bd.BookId).FirstOrDefault().BookNameKh,
                                                                   bd.ImpId,
                                                                   BookDelLabel = bd.ImpId == null ? null : "Loc:" + db.TblItiLocation.Where(w => w.LocId == b.LocId).FirstOrDefault().LocLabel + "/Imp:" + Convert.ToDateTime(db.TblItiImport.Where(w => w.ImpId == bd.ImpId).FirstOrDefault().ImpDate).ToString("dd-MM-yyyy") + "/No:" + bd.BookDelLabel,
                                                                   bd.StatusId,
                                                                   StatusNameReturn = (from rn in db.TblItiReturndeltails
                                                                                       join st in db.TblItiStatus on rn.StatusId equals st.StatusId
                                                                                       where rn.BorId == bor.BorId
                                                                                       select st.StatusName).FirstOrDefault(),
                                                                   StatusPresent = bd.StatusId == null ? "" : db.TblItiStatus.Where(w => w.StatusId == bd.StatusId).FirstOrDefault().StatusName
                                                               }).ToList(),
                                          }).ToList(),
                    }).OrderByDescending(o => o.RetId).Skip(pageSkip).Take(pageSize).ToList();
                    model.data = list;
                    model.success = true;
                    model.setTotalRecord = db.TblItiReturn.Where(w => (startDate == null || w.RetDate >= startDate)
                                                         && (endDate == null || w.RetDate <= endDate)).Count();
                }
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return model;
        }

        public async Task<Data> PostReturnBorrowInfo(ReturnModel ret)
        {
            Data model = new Data();
            //var retDate = ConvertDate.StringToDateTime(ret.RetDate);
            try
            {
                if (ret.RetId == 0)
                {
                    var check = db.TblItiReturn.Where(w => w.RetDate == ret.RetDate).ToList();

                    if (check.Count == 0)
                    {
                        TblItiReturn addRet = new TblItiReturn();
                        addRet.RetDate = ret.RetDate;
                        addRet.UseDelId = ret.UseDelId;
                        addRet.UseEditDate = DateTime.Now;
                        db.TblItiReturn.Add(addRet);
                        db.SaveChanges();
                        foreach (var item in ret.returndeltails)
                        {
                            TblItiReturndeltails addRetDel = new TblItiReturndeltails();
                            addRetDel.RetId = addRet.RetId;
                            addRetDel.BorId = item.BorId;
                            addRetDel.StatusId = 4;
                            db.TblItiReturndeltails.Add(addRetDel);
                            var update = db.TblItiBorrow.Where(w => w.BorId == addRetDel.BorId).FirstOrDefault();
                            if(update != null)
                            {
                                update.IsRequired = true;
                            }
                            var upStatus = (from bo in db.TblBorrowDetails
                                            join bd in db.TblItiBookdetails on bo.BookDelId equals bd.BookDelId
                                            where bo.BorId == addRetDel.BorId
                                            select new { bo, bd }).ToList();
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
                        model.message = MessageStatus.SaveOK;
                    }
                    else
                    {
                        model.success = false;
                        model.message = MessageStatus.UnSave;
                    }
                }
                // For Update
                else
                {
                    var upReturn = db.TblItiReturn.Where(w => w.RetId == ret.RetId).SingleOrDefault();
                    if(upReturn != null)
                    {
                        upReturn.RetDate = ret.RetDate;
                        upReturn.UseDelId = ret.UseDelId;
                        upReturn.UseEditDate = DateTime.Now;
                        model.success = true;
                        model.message = MessageStatus.UpdateOK;
                        // Remove RnDetails
                        var removeReturnDel = db.TblItiReturndeltails.Where(w => w.RetId == upReturn.RetId).ToList();
                        db.TblItiReturndeltails.RemoveRange(removeReturnDel);
                        var upStatus = (from bo in db.TblBorrowDetails
                                        join bor in db.TblItiBorrow on bo.BorId equals bor.BorId
                                        join bd in db.TblItiBookdetails on bo.BookDelId equals bd.BookDelId
                                        join rn in db.TblItiReturndeltails on bo.BorId equals rn.BorId
                                        where rn.RetId == upReturn.RetId
                                        select new { bo, bd,bor}).ToList();
                        foreach (var itemUp in upStatus)
                        {
                            if (upStatus != null)
                            {
                                itemUp.bd.StatusId = 3;
                                itemUp.bor.IsRequired = false;
                                db.SaveChanges();
                            }
                        }
                        foreach (var item in ret.returndeltails)
                        {
                            TblItiReturndeltails addRetDel = new TblItiReturndeltails();
                            addRetDel.RetId =upReturn.RetId;
                            addRetDel.BorId = item.BorId;
                            addRetDel.StatusId = 4;
                            db.TblItiReturndeltails.Add(addRetDel);
                            var update = db.TblItiBorrow.Where(w => w.BorId == addRetDel.BorId).FirstOrDefault();
                            if (update != null)
                            {
                                update.IsRequired = true;
                            }
                            var upStatus1 = (from bo in db.TblBorrowDetails
                                            join bd in db.TblItiBookdetails on bo.BookDelId equals bd.BookDelId
                                            where bo.BorId == addRetDel.BorId
                                            select new { bo, bd }).ToList();
                            foreach (var itemUp in upStatus1)
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
                        model.message = MessageStatus.UpdateOK;
                    }
                }
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
