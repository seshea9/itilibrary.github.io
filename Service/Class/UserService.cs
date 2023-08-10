
using ITI_Libraly_Api.Contexts;
using ITI_Libraly_Api.Models;
using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using ITI_Libraly_Api.Utilities;
namespace ITI_Libraly_Api.Service.Class
{
    public class UserService : IUserService
    {
        ITI_LibraySystemContext db;
        public UserService(ITI_LibraySystemContext db)
        {
            this.db = db;
        }
        public async Task<Data> GetUserInfo(int UseId)
        {
            Data model = new Data();
            try
            {
                var list = db.TblItiUser.Where(w => w.UseId == UseId).Select(s => new
                {
                    s.UseId,
                    s.UseName,
                    s.UseType,
                    s.UsePasswords,
                    UseCreateDate=Convert.ToDateTime(s.UseCreateDate).ToString("dd-MM-yyyy"),
                    s.EmpId
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

        public async Task<Data> GetUserList(/*int pageSkip = 0, int pageSize = 10,*/ bool isSearch = false, string? UseName = null)
        {
            Data model = new Data();
            try
            {
                if( isSearch == false)
                {
                    var list = db.TblItiUser.Select(s => new
                    {
                        s.UseId,
                        s.UseName,
                        s.UseType,
                        s.UsePasswords,
                        UseCreateDate = Convert.ToDateTime(s.UseCreateDate).ToString("dd-MM-yyyy"),
                        s.EmpId,
                        UserLoginTimeModel = s.UseId == null ? null : db.TblUseLogintime.Where(w => w.UseId == s.UseId).Select(su => new
                        {
                            su.UseDelId,
                            su.UseId,
                            UseLoginDate = Convert.ToDateTime(su.UseLoginDate).ToString("dd-MM-yyyy,ម៉ោង h'h':mm'm':ss's'")
                        }).ToList(),
                    })/*.Skip(pageSkip).Take(pageSize)*/.ToList();
                    model.data = list;
                    model.success = true;
                    model.setTotalRecord = db.TblItiUser.Count();
                }
                else
                {
                    var search = db.TblItiUser.AsEnumerable().Where(w=>(UseName == null || w.UseName.ToLower().Contains(UseName.ToLower()))).Select(s => new
                    {
                        s.UseId,
                        s.UseName,
                        s.UseType,
                        s.UsePasswords,
                        UseCreateDate = Convert.ToDateTime(s.UseCreateDate).ToString("dd-MM-yyyy"),
                        s.EmpId,
                        UserLoginTimeModel = s.UseId == null ? null : db.TblUseLogintime.Where(w => w.UseId == s.UseId).Select(su => new
                        {
                            su.UseDelId,
                            su.UseId,
                            UseLoginDate =Convert.ToDateTime(su.UseLoginDate).ToString("dd-MM-yyyy,ម៉ោង h'h':mm'mm':ss'ss'")
                        }).OrderByDescending(o=>o.UseDelId).ToList(),
                    }).OrderBy(o=>o.UseId)/*.Skip(pageSkip).Take(pageSize)*/.ToList();
                    model.data = search;
                    model.success = true;
                    model.setTotalRecord = db.TblItiUser.AsEnumerable().Where(w => (UseName == null || w.UseName.ToLower().Contains(UseName.ToLower()))).Count();
                }
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return model;
        }

        public async Task<Data> PostUserInfo(UserModel use)
        {
            Data model = new Data();
            try
            {
                if(use.UseId == 0)
                {
                    var check = db.TblItiUser.Where(w => w.UseName == use.UseName || w.EmpId == use.EmpId).ToList();
                    if(check.Count== 0)
                    {
                        TblItiUser addUse = new TblItiUser();
                        addUse.UseName = use.UseName;
                        addUse.UseType = use.UseType;
                        addUse.UsePasswords = use.UsePasswords;
                        addUse.UseCreateDate = DateTime.Now;
                        addUse.EmpId = use.EmpId;
                        db.TblItiUser.Add(addUse);
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
                    var updateUse = db.TblItiUser.Where(w => w.UseId == use.UseId).SingleOrDefault();
                    if(updateUse != null)
                    {
                        updateUse.UseName = use.UseName;
                        updateUse.UseType = use.UseType;
                        updateUse.UsePasswords = use.UsePasswords;
                        updateUse.UseCreateDate = DateTime.Now;
                        updateUse.EmpId = use.EmpId;
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
