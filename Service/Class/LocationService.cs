using ITI_Libraly_Api.Contexts;
using ITI_Libraly_Api.Models;
using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using ITI_Libraly_Api.Utilities;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Runtime.CompilerServices;

namespace ITI_Libraly_Api.Service.Class
{
    public class LocationService : ILocationService
    {
        ITI_LibraySystemContext db;
        public LocationService(ITI_LibraySystemContext db)
        {
            this.db = db;
        }
        public async Task <Data> GetLocationInfo(string? LocId)
        {
            Data model = new Data();
            try
            {
                var list = db.TblItiLocation.Where(w => w.LocId == LocId).Select(s => new
                {
                    s.LocId,
                    s.LocLabel,
                    s.LocActive,
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

        public async Task<Data> GetLocationList(/*int pageSkip = 0, int pageSize = 10,*/ bool isSearch = false, string? LocLabel = null, bool? LocActive = true)
        {
            Data model = new Data();
            try
            {
                if (isSearch == false)
                {
                    var list = db.TblItiLocation.Select(s => new
                    {
                        s.LocId,
                        s.LocLabel,
                        s.LocActive,
                    })/*.Skip(pageSkip).Take(pageSize)*/.ToList();
                    model.data = list;
                    model.success = true;
                    model.setTotalRecord = db.TblItiLocation.Count();
                }
                else
                {
                    var search = db.TblItiLocation.AsEnumerable().Where(w => (LocLabel == null || w.LocLabel.ToLower().Contains(LocLabel.ToLower()))
                    &&(LocActive == null || LocActive == w.LocActive)).Select(s => new
                    {
                        s.LocId,
                        s.LocLabel,
                        s.LocActive,
                    })/*.Skip(pageSkip).Take(pageSize)*/.ToList();
                    model.data = search;
                    model.success = true;
                    model.setTotalRecord = db.TblItiLocation.AsEnumerable().
                    Where(w => (LocLabel == null || w.LocLabel.ToLower().Contains(LocLabel.ToLower()))
                    && (LocActive == null || LocActive == w.LocActive)).Count();
                }
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return model;
        }

        public async Task<Data> PostLocationInfo(LocationModel loc)
        {
            Data model = new Data();
            try
            {
                var checkId = db.TblItiLocation.Select(s => s.LocId).FirstOrDefault() == null ? "0" : db.TblItiLocation.OrderByDescending(o=>o.LocId).FirstOrDefault().LocId;
                int con = Convert.ToInt32(checkId) + 1;
                var LocId = con.ToString("000");
                if (loc.LocId == "0")
                {
                    var check = db.TblItiLocation.AsEnumerable().Where(w => w.LocLabel == (loc.LocLabel + "-" + LocId)).ToList();
                    if (check.Count == 0)
                    {
                        TblItiLocation addLoc = new TblItiLocation();
                        addLoc.LocId = LocId;
                        addLoc.LocLabel = loc.LocLabel + "-" + LocId;
                        addLoc.LocActive = loc.LocActive;
                        db.TblItiLocation.Add(addLoc);
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
                    var updateLoc = db.TblItiLocation.Where(w => w.LocId == loc.LocId).SingleOrDefault();
                    //var checkUp = (from b in db.TblItiBook 
                    //               join bd in db.TblItiBookdetails on b.BookId equals bd.BookId
                    //               where b.LocId == updateLoc.LocId select bd).ToList();
                    //if (checkUp.Count == 0)
                    //{
                        if (updateLoc != null)
                        {
                            updateLoc.LocId = loc.LocId;
                            updateLoc.LocLabel = loc.LocLabel /*+ "-" + updateLoc.LocId*/;
                            updateLoc.LocActive = loc.LocActive;
                            db.SaveChanges();
                            model.success = true;
                            model.message = MessageStatus.UpdateOK;
                        }
                    //}
                    //else
                    //{
                    //    model.success = false;
                    //    model.message = MessageStatus.UnUpdate;
                    //}
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
