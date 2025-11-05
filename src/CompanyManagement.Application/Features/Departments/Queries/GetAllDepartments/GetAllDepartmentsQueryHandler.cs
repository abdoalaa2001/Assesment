using AutoMapper;
using CompanyManagement.Application.DTOs;
using CompanyManagement.Domain.Interfaces;
using MediatR;

namespace CompanyManagement.Application.Features.Departments.Queries.GetAllDepartments;

public class GetAllDepartmentsQueryHandler : IRequestHandler<GetAllDepartmentsQuery, List<DepartmentDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllDepartmentsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<DepartmentDto>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
    {
        var departments = await _unitOfWork.Departments.GetAllAsync();
        return _mapper.Map<List<DepartmentDto>>(departments);
    }
}