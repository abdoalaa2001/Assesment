using CompanyManagement.Domain.Entities;

namespace CompanyManagement.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<User> Users { get; }
    IRepository<Department> Departments { get; }
    IRepository<Employee> Employees { get; }
    IRepository<Salary> Salaries { get; }
    IRepository<LeaveRequest> LeaveRequests { get; }
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}