using CompanyManagement.Application.DTOs;
using MediatR;

namespace CompanyManagement.Application.Features.Departments.Queries.GetAllDepartments;

public record GetAllDepartmentsQuery : IRequest<List<DepartmentDto>>;