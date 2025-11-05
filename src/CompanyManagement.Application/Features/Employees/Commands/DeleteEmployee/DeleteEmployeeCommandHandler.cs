using CompanyManagement.Domain.Interfaces;
using MediatR;

namespace CompanyManagement.Application.Features.Employees.Commands.DeleteEmployee;

public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteEmployeeCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _unitOfWork.Employees.GetByIdAsync(request.Id);
        if (employee == null)
            return false;

        await _unitOfWork.Employees.DeleteAsync(employee);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
