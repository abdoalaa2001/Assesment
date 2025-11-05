using AutoMapper;
using CompanyManagement.Application.DTOs;
using CompanyManagement.Domain.Interfaces;
using MediatR;

namespace CompanyManagement.Application.Features.LeaveRequests.Queries.GetAllLeaveRequests;

public class GetAllLeaveRequestsQueryHandler : IRequestHandler<GetAllLeaveRequestsQuery, List<LeaveRequestDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllLeaveRequestsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<LeaveRequestDto>> Handle(GetAllLeaveRequestsQuery request, CancellationToken cancellationToken)
    {
        var leaveRequests = request.EmployeeId.HasValue
            ? await _unitOfWork.LeaveRequests.FindAsync(lr => lr.EmployeeId == request.EmployeeId.Value)
            : await _unitOfWork.LeaveRequests.GetAllAsync();

        var employees = await _unitOfWork.Employees.GetAllAsync();
        var leaveRequestsList = leaveRequests.ToList();

        foreach (var lr in leaveRequestsList)
        {
            lr.Employee = employees.First(e => e.Id == lr.EmployeeId);
        }

        return _mapper.Map<List<LeaveRequestDto>>(leaveRequestsList);
    }
}