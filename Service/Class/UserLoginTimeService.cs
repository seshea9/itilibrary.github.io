using ITI_Libraly_Api.Contexts;
using ITI_Libraly_Api.Models;
using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Security.Cryptography.Xml;

namespace ITI_Libraly_Api.Service.Class
{
    
    public class UserLoginTimeService : IUserLoginTimerService​​
    {
        ITI_LibraySystemContext db;
        public UserLoginTimeService(ITI_LibraySystemContext db)
        {
            this.db = db;
        }
        public async Task<Data> PostUserLoginTimeInfo(UserLoginTimeModel userl)
        {
            Data model = new Data();
            try
            {
                TblUseLogintime addu = new TblUseLogintime();
                addu.UseLoginDate = DateTime.Now;
                addu.UseId = userl.UseId;
                db.TblUseLogintime.Add(addu);
                db.SaveChanges();
                model.success = true;
                model.message = "ការស្នើរសុំចូលគណនី!​ ជោគជ័យ";
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
