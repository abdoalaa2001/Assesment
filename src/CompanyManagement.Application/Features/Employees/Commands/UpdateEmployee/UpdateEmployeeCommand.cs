using CompanyManagement.Application.DTOs;
using MediatR;

namespace CompanyManagement.Application.Features.Employees.Commands.UpdateEmployee;

public record UpdateEmployeeCommand(int Id, UpdateEmployeeDto Employee) : IRequest<EmployeeDto>;