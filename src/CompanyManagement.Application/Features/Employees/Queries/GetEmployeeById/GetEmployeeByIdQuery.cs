using CompanyManagement.Application.DTOs;
using MediatR;

namespace CompanyManagement.Application.Features.Employees.Queries.GetEmployeeById;

public record GetEmployeeByIdQuery(int Id) : IRequest<EmployeeDto?>;