using ITI_Libraly_Api.Contexts;
using ITI_Libraly_Api.Models;
using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using ITI_Libraly_Api.Utilities;
using System.Drawing;
using System.Threading;
using WebHrApi.Utilities;

namespace ITI_Libraly_Api.Service.Class
{
    public class BorrowService : IBorrowService
    {
        ITI_LibraySystemContext db;
        public BorrowService(ITI_LibraySystemContext db)
        {
            this.db = db;
        }
        public async Task<Data> GetBorrowInfo(int BorId)
        {
            Data model = new Data();
            try
            {
                var listSingle = db.TblItiBorrow.Where(w => w.BorId == BorId).Select(s => new
                {
                    s.BorId,
                    s.BorCode,
                    s.StuId,
                    StuName = s.StuId == null ? "" : db.TblItiStudent.Where(w=>w.StuId == s.StuId).FirstOrDefault().StuName,
                    s.BorStartDate,
                    s.BorEndDate,
                    s.UseDelId,
                    Barcode = s.BorCode == null ? null : GenerateId.GenerateBacode(s.BorCode, Color.Black,Color.White),
                    IsRequired = db.TblItiReturndeltails.Where(w => w.BorId == s.BorId).FirstOrDefault() == null ? 0 : 1,
                    UseName = s.UseDelId == null ? null : (from u in db.TblUseLogintime
                                                           join ud in db.TblItiUser on u.UseId equals ud.UseId
                                                           join emp in db.TblItiEmployee on ud.EmpId equals emp.EmpId
                                                           where u.UseDelId == s.UseDelId
                                                           select emp.EmpNameKh).FirstOrDefault(),
                    UseEditDate = Convert.ToDateTime(s.UseEditDate).ToString("dd-MM-yyyy,ម៉ោង h'h':mm'mm':ss'ss'"),
                    borrowDetails= (from bor in db.TblBorrowDetails
                                    join bd in db.TblItiBookdetails on bor.BookDelId equals bd.BookDelId
                                    join b in db.TblItiBook on bd.BookId equals b.BookId
                                    where bor.BorId == s.BorId
                                    select new
                                    {
                                        bor.BorDelId,
                                        bor.BorId,
                                        bor.BookDelId,
                                        bd.BookId,
                                        BookNameKh = bd.BookId == null ? "" : db.TblItiBook.Where(w => w.BookId == bd.BookId).FirstOrDefault().BookNameKh,
                                        bd.ImpId,
                                        ImpDate = bd.ImpId == null ? null : Convert.ToDateTime(db.TblItiImport.Where(w => w.ImpId == bd.ImpId).FirstOrDefault().ImpDate).ToString("dd-MM-yyyy"),
                                        //bd.BookDelLabel,
                                        BookDelLabel = bd.ImpId == null ? null : "Loc:" + db.TblItiLocation.Where(w => w.LocId == b.LocId).FirstOrDefault().LocLabel + "/Imp:" + Convert.ToDateTime(db.TblItiImport.Where(w => w.ImpId == bd.ImpId).FirstOrDefault().ImpDate).ToString("dd-MM-yyyy") + "/No:" + bd.BookDelLabel,
                                        //bor.StatusId,
                                        StatusName = db.TblItiStatus.Where(w=>w.StatusId == bor.StatusId).FirstOrDefault().StatusName,
                                        bd.StatusId,
                                        StatusNameReturn=(from rn in db.TblItiReturndeltails 
                                             join st in db.TblItiStatus on rn.StatusId equals st.StatusId
                                             where rn.BorId == bor.BorId select st.StatusName).FirstOrDefault(),
                                        StatusPresent = bd.StatusId == null ? "" : db.TblItiStatus.Where(w => w.StatusId == bd.StatusId).FirstOrDefault().StatusName
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

        public async Task<Data> GetBorrowList(int pageSkip = 0, int pageSize = 10, bool isSearch = false, int? StuId = 0, DateTime? startDate = null, DateTime? endDate = null, string? BookDelId = null, string? BarCode = null)
        {
            Data model = new Data();
            try
            {
                if (isSearch == false)
                {
                    var list = db.TblItiBorrow/*.Where(w => w.BorId == BorId)*/.Select(s => new
                    {
                        s.BorId,
                        s.BorCode,
                        s.StuId,
                        StuName = s.StuId == null ? "" : db.TblItiStudent.Where(w => w.StuId == s.StuId).FirstOrDefault().StuName,
                        //BorStartDate = Convert.ToDateTime(s.BorStartDate).ToString("dd-MM-yyyy"),/*,ម៉ោង h'h':mm'mm':ss'ss'*/
                        //BorEndDate = Convert.ToDateTime(s.BorEndDate).ToString("dd-MM-yyyy"),
                        s.BorStartDate,
                        s.BorEndDate,
                        s.UseDelId,
                        IsRequired = db.TblItiReturndeltails.Where(w=>w.BorId == s.BorId).FirstOrDefault() == null ? 0:1,
                        UseName = s.UseDelId == null ? null : (from u in db.TblUseLogintime
                                                               join ud in db.TblItiUser on u.UseId equals ud.UseId
                                                               join emp in db.TblItiEmployee on ud.EmpId equals emp.EmpId
                                                               where u.UseDelId == s.UseDelId
                                                               select emp.EmpNameKh).FirstOrDefault(),
                        UseEditDate = Convert.ToDateTime(s.UseEditDate).ToString("dd-MM-yyyy,ម៉ោង h'h':mm'mm':ss'ss'"),
                        borrowDetails = (from bor in db.TblBorrowDetails
                                         join bd in db.TblItiBookdetails on bor.BookDelId equals bd.BookDelId
                                         join b in db.TblItiBook on bd.BookId equals b.BookId
                                         where bor.BorId == s.BorId
                                         select new
                                         {
                                             bor.BorDelId,
                                             bor.BorId,
                                             bor.BookDelId,
                                             BookDelLabel = bd.ImpId == null ? null : "Loc:" + db.TblItiLocation.Where(w => w.LocId == b.LocId).FirstOrDefault().LocLabel + "/Imp:" + Convert.ToDateTime(db.TblItiImport.Where(w => w.ImpId == bd.ImpId).FirstOrDefault().ImpDate).ToString("dd-MM-yyyy") + "/No:" + bd.BookDelLabel,
                                             bd.BookId,
                                             BookNameKh = bd.BookId == null ? "" : db.TblItiBook.Where(w => w.BookId == bd.BookId).FirstOrDefault().BookNameKh,
                                             bd.ImpId,
                                             ImpDate = bd.ImpId == null ? null : Convert.ToDateTime(db.TblItiImport.Where(w => w.ImpId == bd.ImpId).FirstOrDefault().ImpDate).ToString("dd-MM-yyyy"),
                                             //bd.BookDelLabel,
                                             StatusName = db.TblItiStatus.Where(w => w.StatusId == bor.StatusId).FirstOrDefault().StatusName,
                                             StatusNameReturn = (from rn in db.TblItiReturndeltails
                                                                 join st in db.TblItiStatus on rn.StatusId equals st.StatusId
                                                                 where rn.BorId == bor.BorId
                                                                 select st.StatusName).FirstOrDefault(),
                                             bd.StatusId,
                                             StatusPresent = bd.StatusId == null ? "" : db.TblItiStatus.Where(w => w.StatusId == bd.StatusId).FirstOrDefault().StatusName
                                         }).ToList(),
                    }).OrderByDescending(o=>o.BorId).Skip(pageSkip).Take(pageSize).ToList();
                    model.data = list;
                    model.success = true;
                    model.setTotalRecord = db.TblItiBorrow.Count();
                }
                else
                {
                    var check = db.TblBorrowDetails.Where(w => w.BookDelId == BookDelId).OrderByDescending(o=>o.BorDelId).FirstOrDefault();
                    if (check == null)
                    {
                        check = new TblBorrowDetails();
                    }
                    var listSearch = db.TblItiBorrow.Where(w =>
                   (StuId == 0 || w.StuId == StuId)&&
                   (startDate == null || (w.BorStartDate >= startDate || w.BorStartDate <= startDate) &&
                   startDate <= w.BorEndDate && (w.BorEndDate <= endDate || endDate >= w.BorStartDate))
                   && (BookDelId == null || w.BorId == check.BorId)
                   &&(BarCode == null || w.BorCode == BarCode)).Select(s => new
                    {
                        s.BorId,
                        s.BorCode,
                        s.StuId,
                        StuName = s.StuId == null ? "" : db.TblItiStudent.Where(w => w.StuId == s.StuId).FirstOrDefault().StuName,
                       s.BorStartDate,
                       s.BorEndDate,
                       s.UseDelId,
                       IsRequired = db.TblItiReturndeltails.Where(w => w.BorId == s.BorId).FirstOrDefault() == null ? 0 : 1,
                       UseName = s.UseDelId == null ? null : (from u in db.TblUseLogintime
                                                               join ud in db.TblItiUser on u.UseId equals ud.UseId
                                                               join emp in db.TblItiEmployee on ud.EmpId equals emp.EmpId
                                                               where u.UseDelId == s.UseDelId
                                                               select emp.EmpNameKh).FirstOrDefault(),
                       UseEditDate = Convert.ToDateTime(s.UseEditDate).ToString("dd-MM-yyyy,ម៉ោង h'h':mm'mm':ss'ss'"),
                        borrowDetails = (from bor in db.TblBorrowDetails
                                         join bd in db.TblItiBookdetails on bor.BookDelId equals bd.BookDelId
                                         join b in db.TblItiBook on bd.BookId equals b.BookId
                                         where bor.BorId == s.BorId
                                         && (BookDelId == null || bor.BookDelId == BookDelId)
                                         select new
                                         {
                                             bor.BorDelId,
                                             bor.BorId,
                                             bor.BookDelId,
                                             bd.BookId,
                                             BookNameKh = bd.BookId == null ? "" : db.TblItiBook.Where(w => w.BookId == bd.BookId).FirstOrDefault().BookNameKh,
                                             bd.ImpId,
                                             ImpDate = bd.ImpId == null ? null : Convert.ToDateTime(db.TblItiImport.Where(w => w.ImpId == bd.ImpId).FirstOrDefault().ImpDate).ToString("dd-MM-yyyy"),
                                             //bd.BookDelLabel,
                                             BookDelLabel = bd.ImpId == null ? null : "Loc:" + db.TblItiLocation.Where(w => w.LocId == b.LocId).FirstOrDefault().LocLabel + "/Imp:" + Convert.ToDateTime(db.TblItiImport.Where(w => w.ImpId == bd.ImpId).FirstOrDefault().ImpDate).ToString("dd-MM-yyyy") + "/No:" + bd.BookDelLabel,
                                             StatusName = db.TblItiStatus.Where(w => w.StatusId == bor.StatusId).FirstOrDefault().StatusName,
                                             StatusNameReturn = (from rn in db.TblItiReturndeltails
                                                                 join st in db.TblItiStatus on rn.StatusId equals st.StatusId
                                                                 where rn.BorId == bor.BorId
                                                                 select st.StatusName).FirstOrDefault(),
                                             bd.StatusId,
                                             StatusPresent = bd.StatusId == null ? "" : db.TblItiStatus.Where(w => w.StatusId == bd.StatusId).FirstOrDefault().StatusName
                                         }).ToList(),
                    }).OrderByDescending(o=>o.BorId).Skip(pageSkip).Take(pageSize).ToList();
                    model.data = listSearch;
                    model.success = true;
                    model.setTotalRecord = db.TblItiBorrow.Where(w =>
                    (StuId == 0 || w.StuId == StuId)&&
                   (w.BorStartDate >= startDate || w.BorStartDate <= startDate) &&
                   startDate <= w.BorEndDate && (w.BorEndDate <= endDate || endDate >= w.BorStartDate)).Count();
                }
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return model;
        }

        public async Task<Data> PostBorrowInfo(BorrowModel bor)
        {
            
            Data model = new Data();
            //var borDate = ConvertDate.StringToDateTime(bor.BorDate);
            try
            {
                
                if (bor.BorId == 0)
                {
                    var checkDoupicate = db.TblItiBorrow.Where(w =>((w.BorStartDate >= bor.BorStartDate || w.BorStartDate <= bor.BorStartDate)&&
                    bor.BorStartDate <= w.BorEndDate && (w.BorEndDate <= bor.BorEndDate || bor.BorEndDate >= w.BorStartDate))
                    && (w.StuId == bor.StuId)
                    ).ToList();
                    if(checkDoupicate.Count == 0)
                    {
                        TblItiBorrow addBor = new TblItiBorrow();
                        addBor.StuId = bor.StuId;
                        addBor.BorCode = new GenerateId(db).AutoIdBorrow();
                        addBor.BorStartDate = bor.BorStartDate; 
                        addBor.BorEndDate = bor.BorEndDate;
                        addBor.UseDelId = bor.UseDelId;
                        addBor.IsRequired = false;
                        addBor.UseEditDate = DateTime.Now;
                        db.TblItiBorrow.Add(addBor);
                        db.SaveChanges();
                        var borId = db.TblItiBorrow.OrderByDescending(o => o.BorId).FirstOrDefault().BorId;
                        foreach (var item in bor.borrowDetails)
                        {
                            TblBorrowDetails addBorDel = new TblBorrowDetails();
                            addBorDel.BorId = borId;
                            addBorDel.BookDelId = item.BookDelId;
                            addBorDel.StatusId = 3;
                            db.TblBorrowDetails.Add(addBorDel);
                            var updateBookDel = db.TblItiBookdetails.Where(w => w.BookDelId == addBorDel.BookDelId).SingleOrDefault();
                            if (updateBookDel != null)
                            {
                                updateBookDel.StatusId = 3;
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
                    var upBor = db.TblItiBorrow.Where(w => w.BorId == bor.BorId).SingleOrDefault();
                    if(upBor != null)
                    {
                        upBor.StuId = bor.StuId;
                        upBor.BorStartDate = bor.BorStartDate;
                        upBor.BorEndDate = bor.BorEndDate;
                        upBor.UseDelId = bor.UseDelId;
                        upBor.IsRequired = false;
                        upBor.UseEditDate = DateTime.Now;
                        // Remove
                        var delBorDel = db.TblBorrowDetails.Where(w => w.BorId == upBor.BorId).ToList();
                        db.TblBorrowDetails.RemoveRange(delBorDel);
                        var upStatus = (from bo in db.TblBorrowDetails
                                        join bd in db.TblItiBookdetails on bo.BookDelId equals bd.BookDelId
                                        where bo.BorId == upBor.BorId
                                        select new { bo, bd }).ToList();
                        foreach (var itemUp in upStatus)
                        {
                            if (itemUp != null)
                            {
                                itemUp.bd.StatusId = 1;
                                db.SaveChanges();
                            }
                        }
                        foreach (var item in bor.borrowDetails)
                        {
                            TblBorrowDetails addBorDel = new TblBorrowDetails();
                            addBorDel.BorId = upBor.BorId;
                            addBorDel.BookDelId = item.BookDelId;
                            addBorDel.StatusId = 3;
                            db.TblBorrowDetails.Add(addBorDel);
                            var updateBookDel = db.TblItiBookdetails.Where(w => w.BookDelId == addBorDel.BookDelId).SingleOrDefault();
                            if (updateBookDel != null)
                            {
                                updateBookDel.StatusId = 3;
                            }
                            db.SaveChanges();
                        }
                        db.SaveChanges();
                        model.success = true;
                        model.message = MessageStatus.UpdateOK;
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
