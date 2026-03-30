using AssetManagementSystem.Models;
using AutoMapper;

namespace AssetManagementSystem.MapperConfiguration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig() 
        {
            CreateMap<Asset, AssetDTO>()
                .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.Employee != null ? s.Employee.EmployeeName : null))
                .ForMember(d => d.WarrantyName, o => o.MapFrom(s => s.WarrantyCard != null ? s.WarrantyCard.Provider : null))
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status != null ? s.Status.StatusName : null))
                .ForMember(d => d.SoftwareLicenses, o => o.MapFrom(s => s.AssetSoftwares.Select(asw => asw.SoftwareLicense)));

            CreateMap<SoftwareLicense, SoftwareLicenseDTO>()
                .ForMember(d => d.LicenseName, o => o.MapFrom(s => s.SoftwareName));

            CreateMap<AssetDTO, Asset>()
                .ForMember(d => d.AssetId, opt => opt.Ignore()) // don't overwrite key
                .ForMember(d => d.WarrantyCard, opt => opt.MapFrom(s =>
                    string.IsNullOrEmpty(s.WarrantyName) ? null : new WarrantyCard { Provider = s.WarrantyName }))
                .ForMember(d => d.AssetSoftwares, opt => opt.Ignore());

            CreateMap<WarrantyCard, WarrantyCard>().ReverseMap();

            CreateMap<AddAssetDTO, Asset>()
                .ForMember(d => d.AssetId, opt => opt.Ignore())
                // map WarrantyProvider from DTO into a new WarrantyCard.Provider on the Asset
                .ForMember(d => d.WarrantyCard, opt => opt.MapFrom(s =>
                    string.IsNullOrEmpty(s.WarrantyName) ? null : new WarrantyCard { Provider = s.WarrantyName }))
                .ForMember(d => d.AssetSoftwares, opt => opt.Ignore())
                .ForMember(d => d.Status, opt => opt.Ignore());
    
            CreateMap<Employee, EmployeeDTO>();

            CreateMap<SoftwareLicense, SoftwareDTO>();
        }
    }
}
