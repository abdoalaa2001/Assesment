using AutoMapper;
using CompanyManagement.Application.DTOs;
using CompanyManagement.Domain.Entities;
using CompanyManagement.Domain.Interfaces;
using MediatR;

namespace CompanyManagement.Application.Features.LeaveRequests.Commands.CreateLeaveRequest;

public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, LeaveRequestDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateLeaveRequestCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<LeaveRequestDto> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        // Business Rule: Check pending requests limit (max 3)
        var pendingRequests = await _unitOfWork.LeaveRequests.FindAsync(lr =>
            lr.EmployeeId == request.LeaveRequest.EmployeeId &&
            lr.Status == LeaveStatus.Pending);

        if (pendingRequests.Count() >= 3)
            throw new InvalidOperationException("Cannot have more than 3 pending leave requests");

        // Business Rule: Check for overlapping dates
        var overlappingRequests = await _unitOfWork.LeaveRequests.FindAsync(lr =>
            lr.EmployeeId == request.LeaveRequest.EmployeeId &&
            lr.Status != LeaveStatus.Rejected &&
            ((lr.StartDate <= request.LeaveRequest.EndDate && lr.EndDate >= request.LeaveRequest.StartDate)));

        if (overlappingRequests.Any())
            throw new InvalidOperationException("Leave request overlaps with existing request");

        var leaveRequest = _mapper.Map<LeaveRequest>(request.LeaveRequest);
        await _unitOfWork.LeaveRequests.AddAsync(leaveRequest);
        await _unitOfWork.SaveChangesAsync();

        var employee = await _unitOfWork.Employees.GetByIdAsync(leaveRequest.EmployeeId);
        leaveRequest.Employee = employee!;

        return _mapper.Map<LeaveRequestDto>(leaveRequest);
    }
}