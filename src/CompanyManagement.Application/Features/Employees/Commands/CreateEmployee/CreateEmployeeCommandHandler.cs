using AutoMapper;
using CompanyManagement.Application.DTOs;
using CompanyManagement.Domain.Entities;
using CompanyManagement.Domain.Interfaces;
using MediatR;

namespace CompanyManagement.Application.Features.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateEmployeeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<EmployeeDto> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = _mapper.Map<Employee>(request.Employee);
        await _unitOfWork.Employees.AddAsync(employee);
        await _unitOfWork.SaveChangesAsync();

        var departments = await _unitOfWork.Departments.GetAllAsync();
        var dept = departments.FirstOrDefault(d => d.Id == employee.DepartmentId);
        employee.Department = dept!;

        if (employee.ManagerId.HasValue)
        {
            var manager = await _unitOfWork.Employees.GetByIdAsync(employee.ManagerId.Value);
            employee.Manager = manager;
        }

        return _mapper.Map<EmployeeDto>(employee);
    }
}