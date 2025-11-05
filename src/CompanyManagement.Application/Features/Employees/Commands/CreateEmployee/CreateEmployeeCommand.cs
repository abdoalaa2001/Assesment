using CompanyManagement.Application.DTOs;
using MediatR;

namespace CompanyManagement.Application.Features.Employees.Commands.CreateEmployee;

public record CreateEmployeeCommand(CreateEmployeeDto Employee) : IRequest<EmployeeDto>;