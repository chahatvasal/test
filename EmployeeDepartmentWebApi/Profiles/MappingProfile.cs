using AutoMapper;
using empDeptWebApi.EmployeeDTO;
using empDeptWebApi.Models;


namespace empDeptWebApi.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeTransferDTO>()
                .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.DepartmentName,
                opt => opt.MapFrom(src => src.Department.DepartmentName));

            CreateMap<EmployeeCreateDTO, Employee>();
        }
    }
}
