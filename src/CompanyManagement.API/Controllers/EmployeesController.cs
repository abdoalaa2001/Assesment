using CompanyManagement.Application.DTOs;
using CompanyManagement.Application.Features.Employees.Commands.CreateEmployee;
using CompanyManagement.Application.Features.Employees.Commands.DeleteEmployee;
using CompanyManagement.Application.Features.Employees.Commands.UpdateEmployee;
using CompanyManagement.Application.Features.Employees.Queries.GetAllEmployees;
using CompanyManagement.Application.Features.Employees.Queries.GetEmployeeById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResultDto<EmployeeDto>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null)
    {
        var result = await _mediator.Send(new GetAllEmployeesQuery(pageNumber, pageSize, searchTerm));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDto>> GetById(int id)
    {
        var employee = await _mediator.Send(new GetEmployeeByIdQuery(id));
        if (employee == null)
            return NotFound();

        return Ok(employee);
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> Create(CreateEmployeeDto dto)
    {
        var employee = await _mediator.Send(new CreateEmployeeCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<EmployeeDto>> Update(int id, UpdateEmployeeDto dto)
    {
        var employee = await _mediator.Send(new UpdateEmployeeCommand(id, dto));
        return Ok(employee);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteEmployeeCommand(id));
        if (!result)
            return NotFound();

        return NoContent();
    }
}
