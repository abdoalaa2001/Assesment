using CompanyManagement.Application.DTOs;
using FluentValidation;

namespace CompanyManagement.Application.Validators;

public class UpdateSalaryDtoValidator : AbstractValidator<UpdateSalaryDto>
{
    public UpdateSalaryDtoValidator()
    {
        RuleFor(x => x.EmployeeId)
            .GreaterThan(0).WithMessage("Employee ID is required");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Salary amount must be greater than 0")
            .LessThan(1000000000).WithMessage("Salary amount is too large");
    }
}