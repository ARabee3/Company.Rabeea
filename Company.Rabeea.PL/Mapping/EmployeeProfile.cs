using AutoMapper;
using Company.Rabeea.DAL.Models;
using Company.Rabeea.PL.Dto;
namespace Company.Rabeea.PL.Mapping;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<CreateEmployeeDto, Employee>().ReverseMap();
    }
}
