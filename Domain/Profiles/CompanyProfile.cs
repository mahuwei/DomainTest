using AutoMapper;
using Domain.Dtos;
using Domain.Entities;

namespace Domain.Profiles {
  public class CompanyProfile : Profile {
    public CompanyProfile() {
      CreateMap<Company, CompanyDto>()
        .ForMember(desc => desc.No, opt => opt.MapFrom(src => src.No))
        .ForMember(desc => desc.Name, opt => opt.MapFrom(src => src.Name))
        .ForMember(desc => desc.Status, opt => opt.MapFrom(src => src.Status))
        .ForMember(desc => desc.Address, opt => opt.MapFrom(src => src.Address));

      CreateMap<CompanyDto, Company>()
        .ForMember(desc => desc.No, opt => opt.MapFrom(src => src.No))
        .ForMember(desc => desc.Name, opt => opt.MapFrom(src => src.Name))
        .ForMember(desc => desc.Status, opt => opt.MapFrom(src => src.Status))
        .ForMember(desc => desc.Address, opt => opt.MapFrom(src => src.Address));
    }
  }
}