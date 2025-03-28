using AutoMapper;
using Company.Rabeea.DAL.Models;
using Company.Rabeea.PL.Dto;
namespace Company.Rabeea.PL.Mapping;

public class DepartmentProfile : Profile
{
    public DepartmentProfile()
    {
        CreateMap<CreateDepartmentDto, Department>().ReverseMap();
    }
}
