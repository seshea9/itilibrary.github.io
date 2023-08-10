using Aspose.Imaging.Xmp.Types.Derived;
using ITI_Libraly_Api.Contexts;
using ITI_Libraly_Api.Models;
using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using ITI_Libraly_Api.Utilities;
using Microsoft.EntityFrameworkCore;

namespace ITI_Libraly_Api.Service.Class
{
    public class EmployeeService : IEmployeeService
    {
        ITI_LibraySystemContext db;
        private readonly IWebHostEnvironment environment;

        public EmployeeService(ITI_LibraySystemContext db,IWebHostEnvironment environment)
        {
            this.db = db;
            this.environment = environment;
        }
        public async Task<Data> GetEmployeeInfo(int EmpId)
        {
            Data model = new Data();
            try
            {
                var list = db.TblItiEmployee.Where(w => w.EmpId == EmpId).Select(s => new
                {
                    s.EmpId,
                    s.EmpNoteId,
                    s.EmpNameKh,
                    s.EmpNameEn,
                    s.EmpSex,
                    EmpDob= s.EmpDob == null ? null : Convert.ToDateTime(s.EmpDob).ToString("dd-MM-yyyy"),
                    s.EmpAddress,
                    EmpDateIn= s.EmpDateIn == null ? null :Convert.ToDateTime(s.EmpDateIn).ToString("dd-MM-yyyy"),
                    EmpDateOut = s.EmpDateOut == null ? null : Convert.ToDateTime(s.EmpDateOut).ToString("dd-MM-yyyy"),
                    s.EmpPhone,
                    s.PosId,
                    EmpWorkId= Convert.ToInt32(s.EmpWorkId).ToString("0000"),
                    s.NationalId,
                    National = db.TblNational.Where(w=>w.Id == s.NationalId).FirstOrDefault(),
                    PosNameKh = s.PosId == null ? "" : db.TblItiPosition.Where(p=>p.PosId == s.PosId).FirstOrDefault().PosNameKh,
                    FilePath = new ReadPath(environment).GetImage(s.FilePath),
                    s.EmpAtive,
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

        public async Task<Data> GetEmployeeList(int pageSkip = 0, int pageSize = 10, bool isSearch = false, string? EmpNoteId = null, string? EmpNameKh = null, string? EmpNameEn= null)
        {
            Data model = new Data();
            try
            {
                if(isSearch == false)
                {
                    var list = db.TblItiEmployee.Select(s => new
                    {
                        s.EmpId,
                        s.EmpNoteId,
                        s.EmpNameKh,
                        s.EmpNameEn,
                        s.EmpSex,
                        EmpDob = s.EmpDob == null ? null : Convert.ToDateTime(s.EmpDob).ToString("dd-MM-yyyy"),
                        s.EmpAddress,
                        EmpDateIn = s.EmpDateIn == null ? null: Convert.ToDateTime(s.EmpDateIn).ToString("dd-MM-yyyy"),
                        EmpDateOut = s.EmpDateOut == null? "" : Convert.ToDateTime(s.EmpDateOut).ToString("dd-MM-yyyy"),
                        s.PosId,
                        EmpWorkId = Convert.ToInt32(s.EmpWorkId).ToString("0000"),
                        s.NationalId,
                        National = db.TblNational.Where(w => w.Id == s.NationalId).FirstOrDefault(),
                        PosNameKh = s.PosId == null ? "" : db.TblItiPosition.Where(p => p.PosId == s.PosId).FirstOrDefault().PosNameKh,
                        s.EmpPhone,
                        s.EmpAtive,
                    })/*.Skip(pageSkip).Take(pageSize)*/.ToList();
                    model.data = list;
                    model.success = true;
                    model.setTotalRecord = db.TblItiEmployee.Count();
                }
                else
                {
                    var search = db.TblItiEmployee.AsEnumerable().Where(w => (EmpNoteId == null || w.EmpNoteId.ToLower().Contains(EmpNoteId.ToLower()))
                    && (EmpNameKh == null || w.EmpNameKh.ToLower().Contains(EmpNameKh.ToLower()) || w.EmpNameEn.ToLower().Contains(EmpNameKh.ToLower()))).Select(s => new
                    {
                        s.EmpId,
                        s.EmpNoteId,
                        s.EmpNameKh,
                        s.EmpNameEn,
                        s.EmpSex,
                        EmpDob = s.EmpDob == null ? null : Convert.ToDateTime(s.EmpDob).ToString("dd-MM-yyyy"),
                        s.EmpAddress,
                        EmpDateIn = s.EmpDateIn == null ? null : Convert.ToDateTime(s.EmpDateIn).ToString("dd-MM-yyyy"),
                        EmpDateOut = s.EmpDateOut == null ? "" : Convert.ToDateTime(s.EmpDateOut).ToString("dd-MM-yyyy"),
                        s.EmpPhone,
                        s.PosId,
                        EmpWorkId = Convert.ToInt32(s.EmpWorkId).ToString("0000"),
                        s.NationalId,
                        National = db.TblNational.Where(w => w.Id == s.NationalId).FirstOrDefault(),
                        PosNameKh = s.PosId == null ? "" : db.TblItiPosition.Where(p => p.PosId == s.PosId).FirstOrDefault().PosNameKh,
                        s.EmpAtive,
                    })/*.Skip(pageSkip).Take(pageSize)*/.ToList();
                    model.data = search;
                    model.success = true;
                    model.setTotalRecord = db.TblItiEmployee.AsEnumerable().Where(w => (EmpNoteId == null || w.EmpNoteId.ToLower().Contains(EmpNoteId.ToLower()))
                    && (EmpNameKh == null || w.EmpNameKh.ToLower().Contains(EmpNameKh.ToLower()))
                    && (EmpNameEn == null || w.EmpNameEn.ToLower().Contains(EmpNameEn.ToLower()))).Count();
                }
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return model;
        }

        public async Task<Data> PostEmployeeInfo(EmployeeModel emp)
        {
            Data model = new Data();
            try
            {
                if (emp.EmpId == 0)
                {
                    var check = db.TblItiEmployee.AsEnumerable().Where(w => w.EmpNameKh == emp.EmpNameKh).ToList();
                    if (check.Count == 0)
                    {
                        
                        TblItiEmployee addEmp = new TblItiEmployee();
                        addEmp.EmpNoteId = emp.EmpNoteId;
                        addEmp.EmpNameKh = emp.EmpNameKh;
                        addEmp.EmpNameEn = emp.EmpNameEn;
                        addEmp.EmpSex = emp.EmpSex;
                        addEmp.EmpDob = ConvertDate.StringToDateTime(emp.EmpDob);
                        addEmp.EmpAddress = emp.EmpAddress;
                        addEmp.EmpDateIn = ConvertDate.StringToDateTime(emp.EmpDateIn);
                        addEmp.EmpDateOut = emp.EmpDateOut == "" ? null : ConvertDate.StringToDateTime(emp.EmpDateOut);
                        addEmp.PosId = emp.PosId;
                        addEmp.NationalId = emp.NationalId;
                        addEmp.EmpPhone = emp.EmpPhone;
                        addEmp.EmpAtive = emp.EmpAtive;
                        addEmp.EmpWorkId = emp.EmpWorkId;
                        addEmp.FilePath = string.IsNullOrEmpty(emp.FileName) == false ? new Upload(environment).UploadImage(emp.FileName, emp.EmpNameKh + "_" + emp.EmpNameEn) : null;
                        db.TblItiEmployee.Add(addEmp);
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
                    var updateEmp = db.TblItiEmployee.Where(w => w.EmpId == emp.EmpId).SingleOrDefault();
                    if( updateEmp != null)
                    {
                        updateEmp.EmpNoteId = emp.EmpNoteId;
                        updateEmp.EmpNameKh = emp.EmpNameKh;
                        updateEmp.EmpNameEn = emp.EmpNameEn;
                        updateEmp.EmpSex = emp.EmpSex;
                        updateEmp.EmpDob = ConvertDate.StringToDateTime(emp.EmpDob);
                        updateEmp.EmpAddress = emp.EmpAddress;
                        updateEmp.EmpDateIn = ConvertDate.StringToDateTime(emp.EmpDateIn);
                        updateEmp.EmpDateOut = ConvertDate.StringToDateTime(emp.EmpDateOut);
                        updateEmp.PosId = emp.PosId;
                        updateEmp.NationalId = emp.NationalId;
                        updateEmp.EmpPhone = emp.EmpPhone;
                        updateEmp.FilePath = string.IsNullOrEmpty(emp.FileName) == false ? new Upload(environment).UploadImage(emp.FileName, emp.EmpNameKh + "_" + emp.EmpNameEn) : null;
                        updateEmp.EmpAtive = emp.EmpAtive;
                        updateEmp.EmpWorkId = emp.EmpWorkId;
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
