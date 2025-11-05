using CompanyManagement.Application.DTOs;
using MediatR;

namespace CompanyManagement.Application.Features.LeaveRequests.Queries.GetAllLeaveRequests;

public record GetAllLeaveRequestsQuery(int? EmployeeId = null) : IRequest<List<LeaveRequestDto>>;