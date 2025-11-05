using AutoMapper;
using CompanyManagement.Application.DTOs;
using CompanyManagement.Domain.Entities;
using CompanyManagement.Domain.Interfaces;
using MediatR;

namespace CompanyManagement.Application.Features.Salaries.Commands.UpdateSalary;

public class UpdateSalaryCommandHandler : IRequestHandler<UpdateSalaryCommand, SalaryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateSalaryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SalaryDto> Handle(UpdateSalaryCommand request, CancellationToken cancellationToken)
    {
        var salaries = await _unitOfWork.Salaries.FindAsync(s => s.EmployeeId == request.Salary.EmployeeId);
        var salary = salaries.FirstOrDefault();

        if (salary == null)
        {
            salary = new Salary
            {
                EmployeeId = request.Salary.EmployeeId,
                Amount = request.Salary.Amount,
                LastUpdated = DateTime.UtcNow
            };
            await _unitOfWork.Salaries.AddAsync(salary);
        }
        else
        {
            salary.Amount = request.Salary.Amount;
            salary.LastUpdated = DateTime.UtcNow;
            await _unitOfWork.Salaries.UpdateAsync(salary);
        }

        await _unitOfWork.SaveChangesAsync();

        var employee = await _unitOfWork.Employees.GetByIdAsync(salary.EmployeeId);
        salary.Employee = employee!;

        return _mapper.Map<SalaryDto>(salary);
    }
}