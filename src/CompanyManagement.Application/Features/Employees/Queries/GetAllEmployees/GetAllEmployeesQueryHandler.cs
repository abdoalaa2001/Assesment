using AutoMapper;
using CompanyManagement.Application.DTOs;
using CompanyManagement.Domain.Interfaces;
using MediatR;

namespace CompanyManagement.Application.Features.Employees.Queries.GetAllEmployees;

public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, PagedResultDto<EmployeeDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllEmployeesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResultDto<EmployeeDto>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
    {
        var allEmployees = await _unitOfWork.Employees.GetAllAsync();
        var departments = await _unitOfWork.Departments.GetAllAsync();
        var salaries = await _unitOfWork.Salaries.GetAllAsync();

        var employeesList = allEmployees.ToList();

        // Apply manual relationships
        foreach (var emp in employeesList)
        {
            emp.Department = departments.FirstOrDefault(d => d.Id == emp.DepartmentId)!;
            emp.Salary = salaries.FirstOrDefault(s => s.EmployeeId == emp.Id);
            if (emp.ManagerId.HasValue)
            {
                emp.Manager = employeesList.FirstOrDefault(e => e.Id == emp.ManagerId);
            }
        }

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchLower = request.SearchTerm.ToLower();
            employeesList = employeesList.Where(e =>
                e.Name.ToLower().Contains(searchLower) ||
                e.Email.ToLower().Contains(searchLower) ||
                e.Department.Name.ToLower().Contains(searchLower)
            ).ToList();
        }

        var totalCount = employeesList.Count;
        var pagedEmployees = employeesList
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var employeeDtos = _mapper.Map<List<EmployeeDto>>(pagedEmployees);

        return new PagedResultDto<EmployeeDto>
        {
            Items = employeeDtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}