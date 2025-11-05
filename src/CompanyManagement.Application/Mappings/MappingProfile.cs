using AutoMapper;
using CompanyManagement.Application.DTOs;
using CompanyManagement.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CompanyManagement.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User mappings
        CreateMap<User, UserDto>();
        CreateMap<RegisterDto, User>();

        // Department mappings
        CreateMap<Department, DepartmentDto>();
        CreateMap<CreateDepartmentDto, Department>();

        // Employee mappings
        CreateMap<Employee, EmployeeDto>()
            .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.Manager != null ? src.Manager.Name : null))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
            .ForMember(dest => dest.SalaryAmount, opt => opt.MapFrom(src => src.Salary != null ? src.Salary.Amount : (decimal?)null));

        CreateMap<CreateEmployeeDto, Employee>();
        CreateMap<UpdateEmployeeDto, Employee>();

        // Salary mappings
        CreateMap<Salary, SalaryDto>()
            .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.Name));
        CreateMap<UpdateSalaryDto, Salary>();

        // Leave Request mappings
        CreateMap<LeaveRequest, LeaveRequestDto>()
            .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.Name))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<CreateLeaveRequestDto, LeaveRequest>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => LeaveStatus.Pending))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
    }
}