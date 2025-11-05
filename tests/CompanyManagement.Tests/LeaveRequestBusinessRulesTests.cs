using CompanyManagement.Application.DTOs;
using CompanyManagement.Application.Features.LeaveRequests.Commands.CreateLeaveRequest;
using CompanyManagement.Domain.Entities;
using CompanyManagement.Domain.Interfaces;
using AutoMapper;
using Moq;
using Xunit;

namespace CompanyManagement.Tests;

public class LeaveRequestBusinessRulesTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateLeaveRequestCommandHandler _handler;

    public LeaveRequestBusinessRulesTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _handler = new CreateLeaveRequestCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task CreateLeaveRequest_WithMoreThan3PendingRequests_ThrowsException()
    {
        // Arrange
        var employeeId = 1;
        var createDto = new CreateLeaveRequestDto
        {
            EmployeeId = employeeId,
            StartDate = DateTime.Today.AddDays(1),
            EndDate = DateTime.Today.AddDays(2)
        };

        var pendingRequests = new List<LeaveRequest>
        {
            new() { Id = 1, EmployeeId = employeeId, Status = LeaveStatus.Pending },
            new() { Id = 2, EmployeeId = employeeId, Status = LeaveStatus.Pending },
            new() { Id = 3, EmployeeId = employeeId, Status = LeaveStatus.Pending }
        };

        _unitOfWorkMock.Setup(u => u.LeaveRequests.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<LeaveRequest, bool>>>()))
            .ReturnsAsync(pendingRequests);

        var command = new CreateLeaveRequestCommand(createDto);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Cannot have more than 3 pending leave requests", exception.Message);
    }

    [Fact]
    public async Task CreateLeaveRequest_WithOverlappingDates_ThrowsException()
    {
        // Arrange
        var employeeId = 1;
        var createDto = new CreateLeaveRequestDto
        {
            EmployeeId = employeeId,
            StartDate = new DateTime(2025, 12, 10),
            EndDate = new DateTime(2025, 12, 15)
        };

        var existingRequests = new List<LeaveRequest>
        {
            new()
            {
                Id = 1,
                EmployeeId = employeeId,
                StartDate = new DateTime(2025, 12, 12),
                EndDate = new DateTime(2025, 12, 18),
                Status = LeaveStatus.Approved
            }
        };

        _unitOfWorkMock.SetupSequence(u => u.LeaveRequests.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<LeaveRequest, bool>>>()))
            .ReturnsAsync(new List<LeaveRequest>()) // For pending check
            .ReturnsAsync(existingRequests); // For overlap check

        var command = new CreateLeaveRequestCommand(createDto);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Leave request overlaps with existing request", exception.Message);
    }

    [Fact]
    public async Task CreateLeaveRequest_WithValidData_Succeeds()
    {
        // Arrange
        var employeeId = 1;
        var createDto = new CreateLeaveRequestDto
        {
            EmployeeId = employeeId,
            StartDate = DateTime.Today.AddDays(1),
            EndDate = DateTime.Today.AddDays(3)
        };

        var employee = new Employee { Id = employeeId, Name = "John Doe" };
        var leaveRequest = new LeaveRequest
        {
            Id = 1,
            EmployeeId = employeeId,
            StartDate = createDto.StartDate,
            EndDate = createDto.EndDate,
            Status = LeaveStatus.Pending
        };

        _unitOfWorkMock.Setup(u => u.LeaveRequests.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<LeaveRequest, bool>>>()))
            .ReturnsAsync(new List<LeaveRequest>());

        _unitOfWorkMock.Setup(u => u.LeaveRequests.AddAsync(It.IsAny<LeaveRequest>()))
            .ReturnsAsync(leaveRequest);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync())
            .ReturnsAsync(1);

        _unitOfWorkMock.Setup(u => u.Employees.GetByIdAsync(employeeId))
            .ReturnsAsync(employee);

        _mapperMock.Setup(m => m.Map<LeaveRequest>(createDto))
            .Returns(leaveRequest);

        _mapperMock.Setup(m => m.Map<LeaveRequestDto>(It.IsAny<LeaveRequest>()))
            .Returns(new LeaveRequestDto
            {
                Id = 1,
                EmployeeId = employeeId,
                EmployeeName = "John Doe",
                Status = "Pending"
            });

        var command = new CreateLeaveRequestCommand(createDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(employeeId, result.EmployeeId);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}