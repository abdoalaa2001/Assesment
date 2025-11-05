using CompanyManagement.Application.DTOs;
using MediatR;

namespace CompanyManagement.Application.Features.LeaveRequests.Commands.UpdateLeaveStatus;

public record UpdateLeaveStatusCommand(int Id, string Status) : IRequest<LeaveRequestDto>;