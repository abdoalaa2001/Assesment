using CompanyManagement.Domain.Entities;
using CompanyManagement.Domain.Interfaces;
using CompanyManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace CompanyManagement.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Users = new Repository<User>(_context);
        Departments = new Repository<Department>(_context);
        Employees = new Repository<Employee>(_context);
        Salaries = new Repository<Salary>(_context);
        LeaveRequests = new Repository<LeaveRequest>(_context);
    }

    public IRepository<User> Users { get; private set; }
    public IRepository<Department> Departments { get; private set; }
    public IRepository<Employee> Employees { get; private set; }
    public IRepository<Salary> Salaries { get; private set; }
    public IRepository<LeaveRequest> LeaveRequests { get; private set; }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}