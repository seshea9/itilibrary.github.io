using ITI_Libraly_Api.Contexts;
using ITI_Libraly_Api.Models;
using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using ITI_Libraly_Api.Utilities;
using System;

namespace ITI_Libraly_Api.Service.Class
{
    public class SupplyerService : ISupplyerService
    {
        ITI_LibraySystemContext db;
        public SupplyerService(ITI_LibraySystemContext db)
        {
            this.db = db;
        }
        public async Task<Data> GetSupplyerInfo(int SupId)
        {
            Data model = new Data();
            try
            {
                var list = db.TblItiSupplyer.Where(w => w.SupId == SupId).Select(s => new
                {
                    s.SupId,
                    s.SupName,
                    s.SupAddress,
                    s.SupPhone
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

        public async Task<Data> GetSupplyerList(/*int pageSkip = 0, int pageSize = 10,*/ bool isSearch = false, string? SupName = null, string? SupPhone = null)
        {
            Data model = new Data();
            try
            {
                if (isSearch == false)
                {
                    var list = db.TblItiSupplyer.Select(s => new
                    {
                        s.SupId,
                        s.SupName,
                        s.SupAddress,
                        s.SupPhone
                    }).OrderByDescending(o => o.SupId)/*.Skip(pageSkip).Take(pageSize)*/.ToList();
                    model.data = list;
                    model.success = true;
                    model.setTotalRecord = db.TblItiSupplyer.Count();
                }
                else
                {
                    var list = db.TblItiSupplyer.AsEnumerable().Where(w=>(SupName == null || w.SupName.ToLower().Contains(SupName.ToLower()))
                    &&(SupPhone == null || w.SupPhone.ToLower().Contains(SupPhone.ToLower()))).Select(s => new
                    {
                        s.SupId,
                        s.SupName,
                        s.SupAddress,
                        s.SupPhone
                    }).OrderByDescending(o => o.SupId)/*.Skip(pageSkip).Take(pageSize)*/.ToList();
                    model.data = list;
                    model.success = true;
                    model.setTotalRecord = db.TblItiSupplyer.AsEnumerable().Where(w => (SupName == null || w.SupName.ToLower().Contains(SupName.ToLower()))
                    && (SupPhone == null || w.SupPhone.ToLower().Contains(SupPhone.ToLower()))).Count();
                }
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return model;
        }

        public async Task<Data> PostSupplyerInfo(SupplyerModel sup)
        {
            Data model = new Data();
            try
            {
                if (sup.SupId == 0)
                {
                    var check = db.TblItiSupplyer.Where(w => w.SupName.Equals(sup.SupName)).ToList();
                    if (check.Count == 0)
                    {
                        TblItiSupplyer addSup = new TblItiSupplyer();
                        addSup.SupName = sup.SupName;
                        addSup.SupAddress = sup.SupAddress;
                        addSup.SupPhone = sup.SupPhone;
                        db.TblItiSupplyer.Add(addSup);
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
                    var updateSup = db.TblItiSupplyer.Where(w => w.SupId == sup.SupId).SingleOrDefault();
                    if( updateSup != null)
                    {
                        updateSup.SupName = sup.SupName;
                        updateSup.SupAddress = sup.SupAddress;
                        updateSup.SupPhone = sup.SupPhone;
                        db.SaveChanges();
                        model.success = true;
                        model.message = MessageStatus.UpdateOK;
                    }
                }
            }
            catch(Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return model;
        }
    }
}
