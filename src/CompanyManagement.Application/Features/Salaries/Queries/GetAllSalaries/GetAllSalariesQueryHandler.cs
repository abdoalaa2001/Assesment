using AutoMapper;
using CompanyManagement.Application.DTOs;
using CompanyManagement.Domain.Interfaces;
using MediatR;

namespace CompanyManagement.Application.Features.Salaries.Queries.GetAllSalaries;

public class GetAllSalariesQueryHandler : IRequestHandler<GetAllSalariesQuery, List<SalaryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllSalariesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<SalaryDto>> Handle(GetAllSalariesQuery request, CancellationToken cancellationToken)
    {
        var salaries = await _unitOfWork.Salaries.GetAllAsync();
        var employees = await _unitOfWork.Employees.GetAllAsync();

        var salariesList = salaries.ToList();
        foreach (var salary in salariesList)
        {
            salary.Employee = employees.First(e => e.Id == salary.EmployeeId);
        }

        return _mapper.Map<List<SalaryDto>>(salariesList);
    }
}
