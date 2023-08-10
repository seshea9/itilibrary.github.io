using AutoMapper;
using ITI_Libraly_Api.Contexts;
using ITI_Libraly_Api.Models;
using ITI_Libraly_Api.MyModels;

namespace ITI_Libraly_Api.Utilities
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMap()
        {
            var mappingConfig = new MapperConfiguration(config=> 
            {
                config.CreateMap<TblItiSupplyer,SupplyerModel>();
                config.CreateMap<TblItiEmployee,EmployeeModel>();
                config.CreateMap<TblItiPosition,PositionModel>();
                config.CreateMap<PositionModel, TblItiPosition>();
                config.CreateMap<TblItiBorrow, BorrowModel>();
                config.CreateMap<TblBorrowDetails, BorrowDetailsModel>();
                config.CreateMap<TblNational,NationalModel>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
