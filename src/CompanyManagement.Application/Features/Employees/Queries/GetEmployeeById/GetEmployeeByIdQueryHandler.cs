using AutoMapper;
using CompanyManagement.Application.DTOs;
using CompanyManagement.Domain.Interfaces;
using MediatR;

namespace CompanyManagement.Application.Features.Employees.Queries.GetEmployeeById;

public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetEmployeeByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<EmployeeDto?> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await _unitOfWork.Employees.GetByIdAsync(request.Id);
        if (employee == null)
            return null;

        var departments = await _unitOfWork.Departments.GetAllAsync();
        employee.Department = departments.First(d => d.Id == employee.DepartmentId);

        if (employee.ManagerId.HasValue)
        {
            employee.Manager = await _unitOfWork.Employees.GetByIdAsync(employee.ManagerId.Value);
        }

        var salaries = await _unitOfWork.Salaries.FindAsync(s => s.EmployeeId == employee.Id);
        employee.Salary = salaries.FirstOrDefault();

        return _mapper.Map<EmployeeDto>(employee);
    }
}