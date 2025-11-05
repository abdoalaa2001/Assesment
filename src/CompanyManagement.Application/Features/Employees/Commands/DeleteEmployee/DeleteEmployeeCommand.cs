using MediatR;

namespace CompanyManagement.Application.Features.Employees.Commands.DeleteEmployee;

public record DeleteEmployeeCommand(int Id) : IRequest<bool>;