namespace CompanyManagement.Domain.Entities;

public class Salary
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
    public decimal Amount { get; set; }
    public DateTime LastUpdated { get; set; }
}