using CompanyManagement.Application.DTOs;
using MediatR;

namespace CompanyManagement.Application.Features.LeaveRequests.Commands.CreateLeaveRequest;

public record CreateLeaveRequestCommand(CreateLeaveRequestDto LeaveRequest) : IRequest<LeaveRequestDto>;