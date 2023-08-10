using AutoMapper;
using ITI_Libraly_Api.Contexts;
using ITI_Libraly_Api.Models;
using ITI_Libraly_Api.MyModels;
using ITI_Libraly_Api.Service.Interface;
using ITI_Libraly_Api.Utilities;

namespace ITI_Libraly_Api.Service.Class
{
    public class NationalService : INationalService
    {
        ITI_LibraySystemContext db;
        private readonly IMapper _mapper;

        public NationalService(ITI_LibraySystemContext db,IMapper _mapper)
        {
            this.db = db;
            this._mapper = _mapper;
        }
        public async Task<Data> Ctreate(NationalModel model)
        {
            Data data = new Data();
            try
            {
                TblNational add = _mapper.Map<TblNational>(model);
                db.TblNational.Add(add);
                db.SaveChanges();
                data.success = true;
                data.message = MessageStatus.SaveOK;
            }
            catch (Exception ex)
            {
                data.success = false;
                data.message = ex.ToString();
            }
            return data;
        }

        public async Task<Data> GetList()
        {
            Data data = new Data();
            try
            {
                var result = db.TblNational.ToList();
                data.success = true;
                data.data = result;
            }
            catch (Exception ex)
            {
                data.success = false;
                data.message = ex.ToString();
            }
            return data;
        }
    }
 }

