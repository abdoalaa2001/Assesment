using AutoMapper;
using CompanyManagement.Application.DTOs;
using CompanyManagement.Domain.Interfaces;
using MediatR;

namespace CompanyManagement.Application.Features.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateEmployeeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<EmployeeDto> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _unitOfWork.Employees.GetByIdAsync(request.Id);
        if (employee == null)
            throw new KeyNotFoundException($"Employee with ID {request.Id} not found");

        employee.Name = request.Employee.Name;
        employee.Email = request.Employee.Email;
        employee.ManagerId = request.Employee.ManagerId;
        employee.DepartmentId = request.Employee.DepartmentId;

        await _unitOfWork.Employees.UpdateAsync(employee);
        await _unitOfWork.SaveChangesAsync();

        var departments = await _unitOfWork.Departments.GetAllAsync();
        employee.Department = departments.First(d => d.Id == employee.DepartmentId);

        if (employee.ManagerId.HasValue)
        {
            employee.Manager = await _unitOfWork.Employees.GetByIdAsync(employee.ManagerId.Value);
        }

        return _mapper.Map<EmployeeDto>(employee);
    }
}