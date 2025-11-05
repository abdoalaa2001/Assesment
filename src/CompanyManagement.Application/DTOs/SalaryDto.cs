namespace CompanyManagement.Application.DTOs;

public class SalaryDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime LastUpdated { get; set; }
}

public class UpdateSalaryDto
{
    public int EmployeeId { get; set; }
    public decimal Amount { get; set; }
}