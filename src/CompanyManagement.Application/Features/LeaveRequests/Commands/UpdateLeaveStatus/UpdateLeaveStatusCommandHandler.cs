using AutoMapper;
using CompanyManagement.Application.DTOs;
using CompanyManagement.Domain.Entities;
using CompanyManagement.Domain.Interfaces;
using MediatR;

namespace CompanyManagement.Application.Features.LeaveRequests.Commands.UpdateLeaveStatus;

public class UpdateLeaveStatusCommandHandler : IRequestHandler<UpdateLeaveStatusCommand, LeaveRequestDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateLeaveStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<LeaveRequestDto> Handle(UpdateLeaveStatusCommand request, CancellationToken cancellationToken)
    {
        var leaveRequest = await _unitOfWork.LeaveRequests.GetByIdAsync(request.Id);
        if (leaveRequest == null)
            throw new KeyNotFoundException($"Leave request with ID {request.Id} not found");

        if (!Enum.TryParse<LeaveStatus>(request.Status, true, out var status))
            throw new ArgumentException($"Invalid status: {request.Status}");

        leaveRequest.Status = status;
        await _unitOfWork.LeaveRequests.UpdateAsync(leaveRequest);
        await _unitOfWork.SaveChangesAsync();

        var employee = await _unitOfWork.Employees.GetByIdAsync(leaveRequest.EmployeeId);
        leaveRequest.Employee = employee!;

        return _mapper.Map<LeaveRequestDto>(leaveRequest);
    }
}