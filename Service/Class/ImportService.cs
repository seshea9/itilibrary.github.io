using ITI_Libraly_Api.Contexts;
using ITI_Libraly_Api.Models;
using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using ITI_Libraly_Api.Utilities;
using System.Runtime.Intrinsics.Arm;
using WebHrApi.Utilities;

namespace ITI_Libraly_Api.Service.Class
{
    public class ImportService : IImportService
    {
        ITI_LibraySystemContext db;
        public ImportService(ITI_LibraySystemContext db)
        {
            this.db = db;
        }
        public async Task<Data> GetImportInfo(int ImpId)
        {
            Data model = new Data();
            try
            {
                var list = db.TblItiImport.Where(w => w.ImpId == ImpId).Select(s => new
                {
                    s.ImpId,
                    ImpDate= Convert.ToDateTime(s.ImpDate).ToString("dd-MM-yyyy"),
                    s.SupId,
                    SupName = s.SupId == null ? "" : db.TblItiSupplyer.Where(w=>w.SupId == s.SupId).FirstOrDefault().SupName,
                    s.UseDelId,
                    UseName = s.UseDelId == null ? null : (from u in db.TblUseLogintime
                                                           join ud in db.TblItiUser on u.UseId equals ud.UseId
                                                           join emp in db.TblItiEmployee on ud.EmpId equals emp.EmpId
                                                           where u.UseDelId == s.UseDelId
                                                           select emp.EmpNameKh).FirstOrDefault(),
                    UseEditDate = Convert.ToDateTime(s.UseEditDate).ToString("dd-MM-yyyy,ម៉ោង h'h':mm'mm':ss'ss'"),
                    importDetails= db.TblItiImportDetails.Where(w=>w.ImpId== ImpId).Select(s => new
                    {
                        s.ImpDelId,
                        s.BookId,
                        BookNameKh = s.BookId == null ? "": db.TblItiBook.Where(w=>w.BookId == s.BookId).FirstOrDefault().BookNameKh,
                        s.ImpQty,
                        s.ImpPrice
                    }).ToList()
                }).SingleOrDefault();
                model.data = list;
                model.success = true;
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return model;
        }

        public async Task<Data> GetImportList(/*int pageSkip = 0, int pageSize = 10,*/ bool isSearch = false, string? startDate = null, string? endDate = null, int? SupId = 0)
        {
            Data model = new Data();
            try
            {
                if (isSearch == false)
                {
                    var list = db.TblItiImport.Select(s => new
                    {
                        s.ImpId,
                        ImpDate = Convert.ToDateTime(s.ImpDate).ToString("dd-MM-yyyy"),
                        s.SupId,
                        SupName = s.SupId == null ? "" : db.TblItiSupplyer.Where(w => w.SupId == s.SupId).FirstOrDefault().SupName,
                        s.UseDelId,
                        UseName = s.UseDelId == null ? null : (from u in db.TblUseLogintime
                                                               join ud in db.TblItiUser on u.UseId equals ud.UseId
                                                               join emp in db.TblItiEmployee on ud.EmpId equals emp.EmpId
                                                               where u.UseDelId == s.UseDelId
                                                               select emp.EmpNameKh).FirstOrDefault(),
                        UseEditDate = Convert.ToDateTime(s.UseEditDate).ToString("dd-MM-yyyy,ម៉ោង h'h':mm'mm':ss'ss'"),
                        importDetails = db.TblItiImportDetails.Where(w => w.ImpId == s.ImpId).Select(s => new
                        {
                            s.ImpDelId,
                            s.BookId,
                            BookNameKh = s.BookId == null ? "" : db.TblItiBook.Where(w => w.BookId == s.BookId).FirstOrDefault().BookNameKh,
                            s.ImpQty,
                            s.ImpPrice
                        }).ToList()
                    })/*.Skip(pageSkip).Take(pageSize)*/.OrderByDescending(o=>o.ImpId).ToList();
                    model.data = list;
                    model.success = true;
                    model.setTotalRecord = db.TblItiImport.Count();
                }
                else
                {
                    var search = db.TblItiImport.Where(w=>(SupId == 0 || w.SupId == SupId)
                    &&(startDate == null || w.ImpDate>= ConvertDate.StringToDateTime(startDate))
                    &&(endDate == null || w.ImpDate <= ConvertDate.StringToDateTime(endDate))).Select(s => new
                    {
                        s.ImpId,
                        ImpDate = Convert.ToDateTime(s.ImpDate).ToString("dd-MM-yyyy"),
                        s.SupId,
                        SupName = s.SupId == null ? "" : db.TblItiSupplyer.Where(w => w.SupId == s.SupId).FirstOrDefault().SupName,
                        s.UseDelId,
                        UseName = s.UseDelId == null ? null : (from u in db.TblUseLogintime
                                                               join ud in db.TblItiUser on u.UseId equals ud.UseId
                                                               join emp in db.TblItiEmployee on ud.EmpId equals emp.EmpId
                                                               where u.UseDelId == s.UseDelId
                                                               select emp.EmpNameKh).FirstOrDefault(),
                        UseEditDate = Convert.ToDateTime(s.UseEditDate).ToString("dd-MM-yyyy,ម៉ោង h'h':mm'mm':ss'ss'"),
                        importDetails = db.TblItiImportDetails.Where(w => w.ImpId == s.ImpId).Select(s => new
                        {
                            s.ImpDelId,
                            s.BookId,
                            BookNameKh = s.BookId == null ? "" : db.TblItiBook.Where(w => w.BookId == s.BookId).FirstOrDefault().BookNameKh,
                            s.ImpQty,
                            s.ImpPrice
                        }).ToList()
                    })/*.Skip(pageSkip).Take(pageSize)*/.OrderByDescending(o=>o.ImpId).ToList();
                    model.data = search;
                    model.success = true;
                    model.setTotalRecord = db.TblItiImport.Where(w => (SupId == 0 || w.SupId == SupId)
                                                         && (startDate == null || w.ImpDate >= ConvertDate.StringToDateTime(startDate))
                                                        && (endDate == null || w.ImpDate <= ConvertDate.StringToDateTime(endDate))).Count();
                }
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return model;
        }

        public async Task<Data> PostImportInfo(ImportModel imp)
        {
            var impDate = ConvertDate.StringToDateTime(imp.ImpDate);
            var conDate = Convert.ToDateTime(impDate).ToString("dd-MM-yyyy");
            
            Data model = new Data();
            try
            {
                if (imp.ImpId == 0)
                {
                     var check = db.TblItiImport.Where(w => w.ImpDate == impDate && w.SupId == imp.SupId).ToList();
                    if (check.Count == 0)
                    {
                        //imp
                        TblItiImport addImp = new TblItiImport();
                        addImp.ImpDate = impDate;
                        addImp.SupId = imp.SupId;
                        addImp.UseDelId = imp.UseDelId;
                        addImp.UseEditDate = DateTime.Now;
                        db.TblItiImport.Add(addImp);
                        db.SaveChanges();
                        var impId = db.TblItiImport.OrderByDescending(o => o.ImpId).Select(s => s.ImpId).FirstOrDefault();
                        // impdel
                        foreach (var item in imp.importDetails)
                        {
                            TblItiImportDetails addImd = new TblItiImportDetails();
                            addImd.ImpId = impId;
                            addImd.BookId = item.BookId;
                            addImd.ImpQty = item.ImpQty;
                            addImd.ImpPrice = item.ImpPrice;
                            db.TblItiImportDetails.Add(addImd);
                            for (int i = 1; i <= addImd.ImpQty; i++)
                            {
                                // select loc
                                //var locName = (from b in db.TblItiBook
                                //               join l in db.TblItiLocation on b.LocId equals l.LocId
                                //               where b.BookId == addImd.BookId select l.LocLabel).FirstOrDefault();
                                var checkId = db.TblItiBookdetails.Select(s => s.BookDelId).FirstOrDefault() == null ? "0" : db.TblItiBookdetails.OrderByDescending(o => o.BookDelId).FirstOrDefault().BookDelId;
                                int con = Convert.ToInt32(checkId) + 1;
                                var bookDelId = con.ToString("0000000");
                                TblItiBookdetails add = new TblItiBookdetails();
                                add.BookDelId = bookDelId;
                                add.ImpId = impId;
                                add.BookId = addImd.BookId;
                                add.BookDelLabel = /*db.TblItiBook.Where(w => w.BookId == addImd.BookId).FirstOrDefault().BookNameKh + "/Imp:" + conDate + "/" + locName + "/" +*/ new GenerateId(db).AutoId((DateTime)impDate, (int)item.BookId);
                                add.StatusId = 1;
                                db.TblItiBookdetails.Add(add);
                                db.SaveChanges();
                            }
                            var updatePro = db.TblItiBook.Where(w => w.BookId == addImd.BookId).SingleOrDefault();
                            if (updatePro != null)
                            {
                                updatePro.BookQty = updatePro.BookQty + addImd.ImpQty;
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
                    var updateImp = db.TblItiImport.Where(w => w.ImpId == imp.ImpId).SingleOrDefault();
                    if (updateImp != null)
                    {
                        updateImp.ImpDate = impDate;
                        updateImp.SupId = imp.SupId;
                        updateImp.UseDelId = imp.UseDelId;
                        updateImp.UseEditDate = DateTime.Now;
                        // Remove
                        var removeImpDel = db.TblItiImportDetails.Where(w => w.ImpId == updateImp.ImpId).ToList();
                        foreach (var item in removeImpDel)
                        {
                            var removeBookDel = db.TblItiBookdetails.Where(w => w.ImpId == updateImp.ImpId && w.BookId == item.BookId).ToList();
                            db.TblItiBookdetails.RemoveRange(removeBookDel);
                            var updateRemovePro = db.TblItiBook.Where(w => w.BookId == item.BookId).SingleOrDefault();
                            if (updateRemovePro != null)
                            {
                                updateRemovePro.BookQty = updateRemovePro.BookQty - removeBookDel.Count;
                            }
                        }
                        db.RemoveRange(removeImpDel);
                        db.SaveChanges();
                        foreach (var item in imp.importDetails)
                        {
                            var upImpDel = db.TblItiImportDetails.Where(w => w.ImpId == updateImp.ImpId && w.BookId == item.BookId).SingleOrDefault();
                            if(upImpDel != null)
                            {
                                upImpDel.ImpId = updateImp.ImpId;
                                upImpDel.BookId = item.BookId;
                                upImpDel.ImpQty = item.ImpQty;
                                upImpDel.ImpPrice = item.ImpPrice;
                                var removeBookDel = db.TblItiBookdetails.Where(w => w.ImpId == updateImp.ImpId && w.BookId == upImpDel.BookId).ToList();
                                db.TblItiBookdetails.RemoveRange(removeBookDel);
                             
                                var updateRemovePro = db.TblItiBook.Where(w => w.BookId == upImpDel.BookId).SingleOrDefault();
                                if (updateRemovePro != null)
                                {
                                    updateRemovePro.BookQty = updateRemovePro.BookQty - removeBookDel.Count;
                                }
                                for (int i = 1; i <= upImpDel.ImpQty; i++)
                                {
                                    // select loc
                                    //var locName = (from b in db.TblItiBook
                                    //               join l in db.TblItiLocation on b.LocId equals l.LocId
                                    //               where b.BookId == upImpDel.BookId
                                    //               select l.LocLabel).FirstOrDefault();
                                    var checkId = db.TblItiBookdetails.Select(s => s.BookDelId).FirstOrDefault() == null ? "0" : db.TblItiBookdetails.OrderByDescending(o => o.BookDelId).FirstOrDefault().BookDelId;
                                    int con = Convert.ToInt32(checkId) + 1;
                                    var bookDelId = con.ToString("0000000");
                                    TblItiBookdetails add = new TblItiBookdetails();
                                    add.BookDelId = bookDelId;
                                    add.ImpId = updateImp.ImpId;
                                    add.BookId = item.BookId;
                                    add.BookDelLabel = /*db.TblItiBook.Where(w => w.BookId == upImpDel.BookId).FirstOrDefault().BookNameKh + "/Imp:" + conDate + "/" + locName + "/" +*/ new GenerateId(db).AutoId((DateTime)impDate, (int)item.BookId);
                                    add.StatusId = 1;
                                    db.TblItiBookdetails.Add(add);
                                    db.SaveChanges();
                                }
                                var updatePro = db.TblItiBook.Where(w => w.BookId == upImpDel.BookId).SingleOrDefault();
                                if (updatePro != null)
                                {
                                    updatePro.BookQty = updatePro.BookQty + upImpDel.ImpQty;
                                }
                                db.SaveChanges();
                            }
                            else
                            {
                                TblItiImportDetails addImd = new TblItiImportDetails();
                                addImd.ImpId = updateImp.ImpId;
                                addImd.BookId = item.BookId;
                                addImd.ImpQty = item.ImpQty;
                                addImd.ImpPrice = item.ImpPrice;
                                db.TblItiImportDetails.Add(addImd);
                                for (int i = 1; i <= addImd.ImpQty; i++)
                                {

                                    //var locName = (from b in db.TblItiBook
                                    //               join l in db.TblItiLocation on b.LocId equals l.LocId
                                    //               where b.BookId == item.BookId
                                    //               select l.LocLabel).FirstOrDefault();
                                    var checkId = db.TblItiBookdetails.Select(s => s.BookDelId).FirstOrDefault() == null ? "0" : db.TblItiBookdetails.OrderByDescending(o => o.BookDelId).FirstOrDefault().BookDelId;
                                    int con = Convert.ToInt32(checkId) + 1;
                                    var bookDelId = con.ToString("0000000");
                                    TblItiBookdetails add = new TblItiBookdetails();
                                    add.BookDelId = bookDelId;
                                    add.ImpId = updateImp.ImpId;
                                    add.BookId = addImd.BookId;
                                    add.BookDelLabel = /*db.TblItiBook.Where(w => w.BookId == item.BookId).FirstOrDefault().BookNameKh + "/Imp:" + conDate + "/" + locName + "/" +*/ /*i.ToString("0000")*/new GenerateId(db).AutoId((DateTime)impDate, (int)item.BookId);
                                    add.StatusId = 1;
                                    db.TblItiBookdetails.Add(add);
                                    db.SaveChanges();
                                }
                                var updatePro = db.TblItiBook.Where(w => w.BookId == item.BookId).SingleOrDefault();
                                if (updatePro != null)
                                {
                                    updatePro.BookQty = updatePro.BookQty + item.ImpQty;
                                }
                                db.SaveChanges();
                            }
                        }
                        //var upImpdel = db.TblItiImportDetails.Where(w => w.ImpId == updateImp.ImpId).ToList();

                        //if (upImpdel != null)
                        //{
                        //    foreach (var item in imp.importDetails)
                        //    {
                        //        foreach (var itemUp in upImpdel)
                        //        {
                        //            if (itemUp.BookId == item.BookId)
                        //            {
                        //                itemUp.ImpId = updateImp.ImpId;
                        //                itemUp.BookId = item.BookId;
                        //                itemUp.ImpQty = item.ImpQty;
                        //                itemUp.ImpPrice = item.ImpPrice;
                        //                var removeBookDel = db.TblItiBookdetails.Where(w => w.BookId == item.BookId).ToList();
                        //                db.TblItiBookdetails.RemoveRange(removeBookDel);
                        //                var updateRemovePro = db.TblItiBook.Where(w => w.BookId == itemUp.BookId).SingleOrDefault();
                        //                if (updateRemovePro != null)
                        //                {
                        //                    updateRemovePro.BookQty = updateRemovePro.BookQty - removeBookDel.Count;
                        //                }
                        //                for (int i = 1; i <= itemUp.ImpQty; i++)
                        //                {
                        //                    // select loc
                        //                    var locName = (from b in db.TblItiBook
                        //                                   join l in db.TblItiLocation on b.LocId equals l.LocId
                        //                                   where b.BookId == itemUp.BookId
                        //                                   select l.LocLabel).FirstOrDefault();
                        //                    var checkId = db.TblItiBookdetails.Select(s => s.BookDelId).FirstOrDefault() == null ? "0" : db.TblItiBookdetails.OrderByDescending(o => o.BookDelId).FirstOrDefault().BookDelId;
                        //                    int con = Convert.ToInt32(checkId) + 1;
                        //                    var bookDelId = con.ToString("0000000");
                        //                    TblItiBookdetails add = new TblItiBookdetails();
                        //                    add.BookDelId = bookDelId;
                        //                    add.BookId = itemUp.BookId;
                        //                    add.BookDelLabel = db.TblItiBook.Where(w => w.BookId == itemUp.BookId).FirstOrDefault().BookNameKh + "/Imp:" + conDate + "/" + locName + "/" + i.ToString("0000");
                        //                    add.StatusId = 1;
                        //                    db.TblItiBookdetails.Add(add);
                        //                    db.SaveChanges();
                        //                }
                        //                var updatePro = db.TblItiBook.Where(w => w.BookId == itemUp.BookId).SingleOrDefault();
                        //                if (updatePro != null)
                        //                {
                        //                    updatePro.BookQty = updatePro.BookQty + itemUp.ImpQty;
                        //                }
                        //                db.SaveChanges();
                        //            }
                        //            else
                        //            {
                        //                TblItiImportDetails addImd = new TblItiImportDetails();
                        //                addImd.ImpId = updateImp.ImpId;
                        //                addImd.BookId = item.BookId;
                        //                addImd.ImpQty = item.ImpQty;
                        //                addImd.ImpPrice = item.ImpPrice;
                        //                db.TblItiImportDetails.Add(addImd);
                        //                for (int i = 1; i <= addImd.ImpQty; i++)
                        //                {

                        //                    var locName = (from b in db.TblItiBook
                        //                                   join l in db.TblItiLocation on b.LocId equals l.LocId
                        //                                   where b.BookId == item.BookId
                        //                                   select l.LocLabel).FirstOrDefault();
                        //                    var checkId = db.TblItiBookdetails.Select(s => s.BookDelId).FirstOrDefault() == null ? "0" : db.TblItiBookdetails.OrderByDescending(o => o.BookDelId).FirstOrDefault().BookDelId;
                        //                    int con = Convert.ToInt32(checkId) + 1;
                        //                    var bookDelId = con.ToString("0000000");
                        //                    TblItiBookdetails add = new TblItiBookdetails();
                        //                    add.BookDelId = bookDelId;
                        //                    add.BookId = addImd.BookId;
                        //                    add.BookDelLabel = db.TblItiBook.Where(w => w.BookId == item.BookId).FirstOrDefault().BookNameKh + "/Imp:" + conDate + "/" + locName + "/" + i.ToString("0000");
                        //                    add.StatusId = 1;
                        //                    db.TblItiBookdetails.Add(add);
                        //                    db.SaveChanges();
                        //                }
                        //                var updatePro = db.TblItiBook.Where(w => w.BookId == item.BookId).SingleOrDefault();
                        //                if (updatePro != null)
                        //                {
                        //                    updatePro.BookQty = updatePro.BookQty + item.ImpQty;
                        //                }
                        //                db.SaveChanges();
                        //            }
                        //        }
                        //    }
                        //}
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
