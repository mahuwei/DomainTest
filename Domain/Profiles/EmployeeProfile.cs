using AutoMapper;
using Domain.Dtos;
using Domain.Entities;

namespace Domain.Profiles {
  public class EmployeeProfile : Profile {
    public EmployeeProfile() {
      CreateMap<Employee, EmployeeDto>()
        .ForMember(desc => desc.PhoneNo, opt => opt.MapFrom(src => src.MobileNo))
        .ForMember(desc => desc.Name, opt => opt.MapFrom(src => src.Name))
        .ForMember(desc => desc.Status, opt => opt.MapFrom(src => src.Status))
        .ForMember(desc => desc.Address, opt => opt.MapFrom(src => src.Address));

      CreateMap<EmployeeDto, Employee>()
        .ForMember(desc => desc.MobileNo, opt => opt.MapFrom(src => src.PhoneNo))
        .ForMember(desc => desc.Name, opt => opt.MapFrom(src => src.Name))
        .ForMember(desc => desc.Status, opt => opt.MapFrom(src => src.Status))
        .ForMember(desc => desc.Address, opt => opt.MapFrom(src => src.Address));
    }
  }
}