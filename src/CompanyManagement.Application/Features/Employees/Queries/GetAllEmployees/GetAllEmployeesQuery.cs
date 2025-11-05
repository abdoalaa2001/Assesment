using CompanyManagement.Application.DTOs;
using MediatR;

namespace CompanyManagement.Application.Features.Employees.Queries.GetAllEmployees;

public record GetAllEmployeesQuery(int PageNumber = 1, int PageSize = 10, string? SearchTerm = null) : IRequest<PagedResultDto<EmployeeDto>>;