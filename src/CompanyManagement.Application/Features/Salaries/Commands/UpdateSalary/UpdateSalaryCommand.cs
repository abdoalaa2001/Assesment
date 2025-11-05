using CompanyManagement.Application.DTOs;
using MediatR;

namespace CompanyManagement.Application.Features.Salaries.Commands.UpdateSalary;

public record UpdateSalaryCommand(UpdateSalaryDto Salary) : IRequest<SalaryDto>;