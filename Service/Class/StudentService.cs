using ITI_Libraly_Api.Contexts;
using ITI_Libraly_Api.Models;
using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using ITI_Libraly_Api.Utilities;
using System;
using System.Drawing;
using WebHrApi.Utilities;

namespace ITI_Libraly_Api.Service.Class
{
    
    public class StudentService : IStudentService
    {
        ITI_LibraySystemContext db;
        public StudentService(ITI_LibraySystemContext db)
        {
            this.db = db;
        }

        public async Task<Data> GetStudentInfo(int StuId)
        {
            Data model = new Data();
            try
            {
                var stuInfo = db.TblItiStudent.Where(w => w.StuId == StuId).Select(s => new
                {
                    s.StuId,
                    s.StuCode,
                    s.StuName,
                    s.StuSex,
                    s.PosId,
                    PosNameKh = s.PosId == null ? null : db.TblItiPosition.Where(w=>w.PosId== s.PosId).FirstOrDefault().PosNameKh,
                    s.StuAddress,
                    s.StuPhone,
                    NationalId = s.NattionalId,
                    National = db.TblNational.Where(w => w.Id == s.NattionalId).FirstOrDefault(),
                    s.StuDob,
                    s.IsPrintCart,
                    BarCode = GenerateId.GenerateBacode(s.StuCode, Color.Black,Color.White),
                }).SingleOrDefault();
                model.data = stuInfo;
                model.success = true;
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return model;
        }

        public async Task<Data> GetStudentList(/*int pageSkip = 0, int pageSize = 10,*/ bool isSearch = false, string? StuName = null, string? StuPhone = null, string? StuCode = null)
        {
            Data model = new Data();
            try
            {
                if (isSearch == false)
                {
                    var list = db.TblItiStudent.Select(s => new
                    {
                        s.StuId,
                        s.StuCode,
                        s.StuName,
                        s.StuSex,
                        s.PosId,
                        PosNameKh = s.PosId == null ? null : db.TblItiPosition.Where(w => w.PosId == s.PosId).FirstOrDefault().PosNameKh,
                        s.StuAddress,
                        s.StuPhone,
                        NationalId = s.NattionalId,
                        National = db.TblNational.Where(w=>w.Id == s.NattionalId).FirstOrDefault(),
                        s.StuDob,
                        s.IsPrintCart,
                    }).OrderByDescending(o=>o.StuId)/*.Skip(pageSkip).Take(pageSize)*/.ToList();
                    model.data = list;
                    model.success = true;
                    model.setTotalRecord = db.TblItiStudent.Count();
                }
                else
                {
                    var search = db.TblItiStudent.AsEnumerable().Where(w=>(StuName== null || w.StuName.ToLower().Contains(StuName.ToLower()))
                    &&( StuPhone == null || w.StuPhone.ToLower().Contains(StuPhone.ToLower()))
                    &&( StuCode == null || w.StuCode.Equals(StuCode))
                    ).Select(s => new
                    {
                        s.StuId,
                        s.StuCode,
                        s.StuName,
                        s.StuSex,
                        s.StuAddress,
                        s.PosId,
                        PosNameKh = s.PosId == null ? null : db.TblItiPosition.Where(w => w.PosId == s.PosId).FirstOrDefault().PosNameKh,
                        NationalId=s.NattionalId,
                        National = db.TblNational.Where(w => w.Id == s.NattionalId).FirstOrDefault(),
                        s.StuDob,
                        s.StuPhone,
                        s.IsPrintCart,
                    }).OrderByDescending(o => o.StuId)/*.Skip(pageSkip).Take(pageSize)*/.ToList();
                    model.data = search;
                    model.success = true;
                    model.setTotalRecord = db.TblItiStudent.AsEnumerable().Where(w => (StuName == null || w.StuName.ToLower().Contains(StuName.ToLower()))
                    && (StuPhone == null || w.StuPhone.ToLower().Contains(StuPhone.ToLower()))
                    && (StuCode == null || w.StuCode.Equals(StuCode))).Count();
                }
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return model;
        }

        public async Task<Data> PostStudentInfo(StudentModel stu)
        {
            Data model = new Data();
            try
            {
                if(stu.StuId == 0)
                {
                    var checkName = db.TblItiStudent.Where(w => w.StuName.Equals(stu.StuName)).ToList();
                    if (checkName.Count == 0)
                    {
                        TblItiStudent addStu = new TblItiStudent();
                        addStu.StuName = stu.StuName;
                        addStu.StuSex = stu.StuSex;
                        addStu.StuAddress = stu.StuAddress;
                        addStu.StuPhone = stu.StuPhone;
                        addStu.PosId = stu.PosId;
                        addStu.IsPrintCart = false;
                        addStu.StuCode = new GenerateId(db).AutoIdStudent();
                        addStu.StuDob = stu.StuDob;
                        addStu.NattionalId = stu.NationalId;
                        db.TblItiStudent.Add(addStu);
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
                    var upStu = db.TblItiStudent.Where(w => w.StuId == stu.StuId).SingleOrDefault();
                    if (upStu != null)
                    {
                        upStu.StuName = stu.StuName;
                        upStu.StuSex = stu.StuSex;
                        upStu.StuAddress = stu.StuAddress;
                        upStu.StuPhone = stu.StuPhone;
                        upStu.PosId = stu.PosId;
                        upStu.StuDob = stu.StuDob;
                        upStu.NattionalId = stu.NationalId;
                        upStu.IsPrintCart = stu.IsPrintCart;
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
