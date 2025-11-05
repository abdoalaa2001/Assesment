using CompanyManagement.Application.DTOs;
using CompanyManagement.Application.Features.Salaries.Commands.UpdateSalary;
using CompanyManagement.Application.Features.Salaries.Queries.GetAllSalaries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManagement.API.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class SalariesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SalariesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<SalaryDto>>> GetAll()
    {
        var salaries = await _mediator.Send(new GetAllSalariesQuery());
        return Ok(salaries);
    }

    [HttpPut]
    public async Task<ActionResult<SalaryDto>> Update(UpdateSalaryDto dto)
    {
        var salary = await _mediator.Send(new UpdateSalaryCommand(dto));
        return Ok(salary);
    }
}