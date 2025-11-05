using CompanyManagement.Application.DTOs;
using CompanyManagement.Application.Features.LeaveRequests.Commands.CreateLeaveRequest;
using CompanyManagement.Application.Features.LeaveRequests.Commands.UpdateLeaveStatus;
using CompanyManagement.Application.Features.LeaveRequests.Queries.GetAllLeaveRequests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LeaveRequestsController : ControllerBase
{
    private readonly IMediator _mediator;

    public LeaveRequestsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<LeaveRequestDto>>> GetAll([FromQuery] int? employeeId = null)
    {
        var leaveRequests = await _mediator.Send(new GetAllLeaveRequestsQuery(employeeId));
        return Ok(leaveRequests);
    }

    [HttpPost]
    public async Task<ActionResult<LeaveRequestDto>> Create(CreateLeaveRequestDto dto)
    {
        var leaveRequest = await _mediator.Send(new CreateLeaveRequestCommand(dto));
        return CreatedAtAction(nameof(GetAll), new { id = leaveRequest.Id }, leaveRequest);
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{id}/status")]
    public async Task<ActionResult<LeaveRequestDto>> UpdateStatus(int id, [FromBody] string status)
    {
        var leaveRequest = await _mediator.Send(new UpdateLeaveStatusCommand(id, status));
        return Ok(leaveRequest);
    }
}