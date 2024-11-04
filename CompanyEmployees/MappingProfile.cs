using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace CompanyEmployees;

// Profile allows for configuration
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Create maps between Models and Dtos
        CreateMap<Company, CompanyDto>().
            ForMember(c => c.FullAddress, options =>
            //ForCtorParam("FullAddress", options => -- If it is constructor based
        {
            // You can specify options where it would not be a direct assignment
            options.MapFrom(x => string.Join(' ', x.Address, x.Country));
        });

        CreateMap<Employee, EmployeeDto>();

        CreateMap<CompanyForCreationDto, Company>();
    }
}