using AutoMapper;
using ITI_Libraly_Api.Contexts;
using ITI_Libraly_Api.Models;
using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using ITI_Libraly_Api.Utilities;

namespace ITI_Libraly_Api.Service.Class
{
    
    public class PositionService : IPositionService
    {
        ITI_LibraySystemContext db;
        private readonly IMapper mapper;

        public PositionService(ITI_LibraySystemContext db ,IMapper mapper)
        {
            this.db = db;
            this.mapper= mapper;
        }
        public async Task<Data> GetPositionInfo(int PosId)
        {
            Data model = new Data();
            try
            {
                var list = db.TblItiPosition.Where(w => w.PosId == PosId).SingleOrDefault();
                model.data = mapper.Map<PositionModel>(list);
                model.success = true;
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return model;
        }

        public async Task<Data> GetPositionList(/*int pageSkip = 0, int pageSize = 10,*/ bool isSearch = false, string? PosNameKh = null, string? PosNameEn = null)
        {
            Data model = new Data();
            try
            {
                if(isSearch == false)
                {
                var list = db.TblItiPosition/*.Skip(pageSkip).Take(pageSize)*/.ToList();
                model.data = mapper.Map<List<PositionModel>>(list);
                model.success = true;
                model.setTotalRecord = db.TblItiPosition.Count();
                }
                else
                {
                    var search = db.TblItiPosition.AsEnumerable()
                        .Where(w=>(PosNameKh == null || w.PosNameKh.ToLower().Contains(PosNameKh.ToLower()))
                        && (PosNameEn == null || w.PosNameEn.ToLower().Contains(PosNameEn.ToLower())))/*.Skip(pageSkip).Take(pageSize)*/.ToList();
                    model.data = mapper.Map<List<PositionModel>>(search);
                    model.success = true;
                    model.setTotalRecord = db.TblItiPosition.AsEnumerable()
                        .Where(w => (PosNameKh == null || w.PosNameKh.ToLower().Contains(PosNameKh.ToLower()))
                        && (PosNameEn == null || w.PosNameEn.ToLower().Contains(PosNameEn.ToLower()))).Count();
                }
            }
            catch (Exception ex)
            {
                model.success = false;
                model.message = ex.Message;
            }
            return model;
        }

        public async Task<Data> PostPositionInfo(PositionModel positionModel)
        {
            Data model = new Data();
            try
            {
                if (positionModel.PosId == 0)
                {
                    var check = db.TblItiPosition.AsEnumerable().Where(w => w.PosNameKh == positionModel.PosNameKh).ToList();
                    if (check.Count == 0)
                    {
                        TblItiPosition add = mapper.Map<TblItiPosition>(positionModel);
                        db.TblItiPosition.Add(add);
                        db.SaveChanges();
                        model.success = true;
                        model.message = MessageStatus.SaveOK;
                    }
                    else
                    {
                        model.success = false;
                        model.message = MessageStatus.UnSave ;
                    }
                }
                else
                {
                    TblItiPosition add = mapper.Map<TblItiPosition>(positionModel);
                    db.TblItiPosition.Update(add);
                    db.SaveChanges();
                    model.success = true;
                    model.message = MessageStatus.UpdateOK;
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
