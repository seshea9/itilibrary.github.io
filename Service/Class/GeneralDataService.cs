using AutoMapper;
using BarcodeLib;
using ITI_Libraly_Api.Contexts;
using ITI_Libraly_Api.Models;
using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;

namespace ITI_Libraly_Api.Service.Class
{
    public class GeneralDataService : IGeneralDataService
    {
        ITI_LibraySystemContext db;
        private readonly IMapper mapper;

        public GeneralDataService(ITI_LibraySystemContext db,IMapper mapper) 
        {
            this.db = db;
            this.mapper = mapper;
        }
        public async Task<Data> GeneralDataLocation()
        {
            Data model = new Data();
            try
            {
                var query = db.TblItiLocation.ToList();
                model.data = query;
                model.success = true;
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.ToString();
            }
            return model;
        }

        public async Task<Data> GenerlDataCategory(int? expert_id = 0)
        {
            Data model = new Data();
            try
            {
                var query = db.TblItiCategory.Where(w=>expert_id == 0).ToList();
                model.data = query;
                model.success = true;
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.ToString();
            }
            return model;
        }

        public async Task<Data> GenerlDataPosition()
        {
            Data model = new Data();
            try
            {
                var query = db.TblItiPosition.ToList();
                model.data = query;
                model.success = true;
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.ToString();
            }
            return model;
        }
        public async Task<Data> GeneralSupplyer()
        {
            Data model = new Data();
            try
            {
                var query = db.TblItiSupplyer/*.Select(s => new
                {
                //    s.SupId,
                //    s.SupName,
                })*/.ToList();
                model.data = mapper.Map<List<SupplyerModel>>(query);
                model.success = true;
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.ToString();
            }
            return model;
        }

        public async Task<Data> GeneralDataEmployee()
        {
            Data model = new Data();
            try
            {
                var query = db.TblItiEmployee./*Select(s => new
                {
                    //s.EmpId,
                    //s.EmpNameKh,
                }).*/ToList();
                model.data = mapper.Map<List<EmployeeModel>>(query);
                model.success = true;
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.ToString();
            }
            return model;
        }

        public async Task<Data> GeneralDataBook(bool isSearch = false, string? BookNameKh = null, int? CateId = 0)
        {
            Data model = new Data();
            try
            {
                var List = db.TblItiBook.AsEnumerable().Where(w => (BookNameKh == null ||  w.BookNameKh.ToLower().Contains(BookNameKh.ToLower()))
                     && (CateId == 0 || w.CateId.Equals(CateId))).Select(s => new
                     {
                         s.BookId,
                         s.BookNameKh,
                         s.BookNameEn,
                         BookYear = Convert.ToDateTime(s.BookYear).ToString("dd-MM-yyyy"),
                         s.BookQty,
                         s.CateId,
                         CateNameKh = s.CateId == null ? "" : db.TblItiCategory.Where(w => w.CateId == s.CateId).FirstOrDefault().CateNameKh,
                         //BookPhoto = GetImage(s.BookPhoto),
                         s.BookAuthor,
                         s.BookDescription,
                         //FilePath = new ReadPath(environment).GetImage(s.FilePath),
                         s.LocId,
                         LocLabel = s.LocId == null ? "" : db.TblItiLocation.Where(l => l.LocId == s.LocId).FirstOrDefault().LocLabel,
                         //bookDatails = db.TblItiBookdetails.Where(w => w.BookId == s.BookId).Select(sd => new
                         //{
                         //    sd.BookDelId,
                         //    BookNameKh = sd.BookId == null ? "" : db.TblItiBook.Where(w => w.BookId == s.BookId).FirstOrDefault().BookNameKh,
                         //    sd.ImpId,
                         //    ImpDate = sd.ImpId == null ? null : Convert.ToDateTime(db.TblItiImport.Where(w => w.ImpId == sd.ImpId).FirstOrDefault().ImpDate).ToString("dd-MM-yyyy"),
                         //    LocLabel = s.LocId == null ? "" : db.TblItiLocation.Where(l => l.LocId == s.LocId).FirstOrDefault().LocLabel,
                         //    sd.BookDelLabel,
                         //    sd.StatusId,
                         //    Status = sd.StatusId == null ? "" : db.TblItiStatus.Where(w => w.StatusId == sd.StatusId).FirstOrDefault().StatusName,
                         //}).OrderBy(o => o.ImpId).ToList(),
                     })/*.Skip(pageSkip).Take(pageSize)*/.ToList();
                model.data = List;
                model.success = true;
                model.total_qty = (int)db.TblItiBook.AsEnumerable().Where(w => (BookNameKh == null || w.BookNameKh.ToLower().Contains(BookNameKh.ToLower()))
                && (CateId == 0 || w.CateId.Equals(CateId))).Sum(s => s.BookQty);
                model.setTotalRecord = db.TblItiBook.AsEnumerable().Where(w => (BookNameKh == null || w.BookNameKh.ToLower().Contains(BookNameKh.ToLower()))
                && (CateId == 0 || w.CateId.Equals(CateId))).Count();
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.ToString();
            }
            return model;
        }

        public async Task<Data> GeneralDataBookDetail(int pageSkip = 0, int pageSize = 10, bool isSearch = false, int BookId = 0, string? BookNameKh = null, int CateId = 0, string? LocId = null, string? BookDelId = null, string baseUri = "",int StatusId=0)
        {
            Data model = new Data();
            try
            {
                var query = (from bd in db.TblItiBookdetails.AsEnumerable()
                             join b in db.TblItiBook on bd.BookId equals b.BookId
                             where (BookId == 0 || b.BookId == BookId)
                             && (BookNameKh == null || b.BookNameKh.ToLower().Contains(BookNameKh.ToLower()))
                             && (CateId == 0 || b.CateId == CateId)
                             && (LocId == null || b.LocId == LocId)
                             && (BookDelId == null || bd.BookDelId == BookDelId)
                             && (StatusId ==0 || bd.StatusId == StatusId)
                             select new
                             {
                                 bd.BookDelId,
                                 b.BookId,
                                 b.BookNameKh,
                                 CateNameKh = b.CateId == null ? null : db.TblItiCategory.Where(w=>w.CateId == b.CateId).FirstOrDefault().CateNameKh,
                                 BookDelLabel = bd.ImpId == null ? null : "Loc:" + db.TblItiLocation.Where(w => w.LocId == b.LocId).FirstOrDefault().LocLabel+"/Imp:"+Convert.ToDateTime(db.TblItiImport.Where(w=>w.ImpId == bd.ImpId).FirstOrDefault().ImpDate).ToString("dd-MM-yyyy")+ "/No:" + bd.BookDelLabel,
                                 bd.StatusId,
                                 StatusName = bd.StatusId == null ? null : db.TblItiStatus.Where(w=>w.StatusId == bd.StatusId).FirstOrDefault().StatusName,
                                 BookPhoto = baseUri +"/images/"+b.FilePath,

                             }).Skip(pageSkip).Take(pageSize).ToList();
                model.data = query;
                model.success = true;
                model.setTotalRecord = (from bd in db.TblItiBookdetails.AsEnumerable()
                                        join b in db.TblItiBook on bd.BookId equals b.BookId
                                        where (BookId == 0 || b.BookId == BookId)
                                        && (BookNameKh == null || b.BookNameKh.Contains(BookNameKh.ToLower()))
                                        && (CateId == 0 || b.CateId == CateId)
                                        && (LocId == null || b.LocId == LocId)
                                        && (BookDelId == null || bd.BookDelId == BookDelId)
                                        && (StatusId == 0 || bd.StatusId == StatusId)select bd).Count();
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.ToString();
            }
            return model;
        }

        public async Task<Data> GeneralDataStudent(bool isSearch = false, string? StuName = null, int StuId = 0)
        {
            Data model = new Data();
            try
            {
                var query = db.TblItiStudent.AsEnumerable().Where(w => (StuName == null || w.StuName.ToLower().Contains(StuName.ToLower()))
                    && (StuId == 0 || w.StuId == StuId)
                    ).Select(s => new
                    {
                        s.StuId,
                        s.StuName,
                        s.StuSex,
                        s.StuAddress,
                        s.PosId,
                        PosNameKh = s.PosId == null ? null : db.TblItiPosition.Where(w => w.PosId == s.PosId).FirstOrDefault().PosNameKh,
                        s.StuPhone
                    }).OrderByDescending(o => o.StuId)/*.Skip(pageSkip).Take(pageSize)*/.ToList();
                model.data = query;
                model.success = true;
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.ToString();
            }
            return model;
        }

        public async Task<Data> GeneralDataBorrow(int pageSkip = 0, int pageSize = 10, bool isSearch = false, DateTime? BorrowDate = null/*, string? BookDelId = null*/, string? BorCode = null, int StuId = 0, bool IsRequried = false)
        {
            Data model = new Data();
            try
            {
                //var check = db.TblBorrowDetails.Where(w => w.BookDelId == BookDelId).OrderByDescending(o => o.BorDelId).FirstOrDefault();
                //if (check == null)
                //{
                //    check = new TblBorrowDetails();
                //}
               var listSearch = db.TblItiBorrow.Where(w =>
               (StuId == 0 || w.StuId == StuId)
               && (BorrowDate == null || w.BorStartDate == BorrowDate.Value.Date)
               //&& (BookDelId == null || w.BorId == check.BorId)
               && (BorCode == null || w.BorCode == BorCode)
               && (w.IsRequired == IsRequried)).Select(s => new
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
                                        //&& (BookDelId == null || bor.BookDelId == BookDelId)
                                    select new
                                    {
                                        bor.BorDelId,
                                        bor.BorId,
                                        bor.BookDelId,
                                        bd.BookId,
                                        BookNameKh = bd.BookId == null ? "" : db.TblItiBook.Where(w => w.BookId == bd.BookId).FirstOrDefault().BookNameKh,
                                        //bd.ImpId,
                                        //ImpDate = bd.ImpId == null ? null : Convert.ToDateTime(db.TblItiImport.Where(w => w.ImpId == bd.ImpId).FirstOrDefault().ImpDate).ToString("dd-MM-yyyy"),
                                        ////bd.BookDelLabel,
                                        BookDelLabel = bd.ImpId == null ? null : "Loc:" + db.TblItiLocation.Where(w => w.LocId == b.LocId).FirstOrDefault().LocLabel + "/Imp:" + Convert.ToDateTime(db.TblItiImport.Where(w => w.ImpId == bd.ImpId).FirstOrDefault().ImpDate).ToString("dd-MM-yyyy") + "/No:" + bd.BookDelLabel,
                                        StatusName = db.TblItiStatus.Where(w => w.StatusId == bor.StatusId).FirstOrDefault().StatusName,
                                        StatusNameReturn = (from rn in db.TblItiReturndeltails
                                                            join st in db.TblItiStatus on rn.StatusId equals st.StatusId
                                                            where rn.BorId == bor.BorId
                                                            select st.StatusName).FirstOrDefault(),
                                        bd.StatusId,
                                        StatusPresent = bd.StatusId == null ? "" : db.TblItiStatus.Where(w => w.StatusId == bd.StatusId).FirstOrDefault().StatusName
                                    }).ToList(),
               }).OrderByDescending(o => o.BorId).Skip(pageSkip).Take(pageSize).ToList();
                model.data = listSearch;
                model.success = true;
                model.total_record = db.TblItiBorrow.Where(w =>
                (StuId == 0 || w.StuId == StuId)
                && (BorrowDate == null || w.BorStartDate == BorrowDate.Value.Date)
                && (BorCode == null || w.BorCode == BorCode)
                && (w.IsRequired == IsRequried)).Count();
            }
            catch(Exception ex)
            {
                model.success = false;
                model.message = ex.ToString();
            }
            return model;
        }
    }
}
