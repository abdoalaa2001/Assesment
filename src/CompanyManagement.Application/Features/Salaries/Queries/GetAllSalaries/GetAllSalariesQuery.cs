using CompanyManagement.Application.DTOs;
using MediatR;

namespace CompanyManagement.Application.Features.Salaries.Queries.GetAllSalaries;

public record GetAllSalariesQuery : IRequest<List<SalaryDto>>;