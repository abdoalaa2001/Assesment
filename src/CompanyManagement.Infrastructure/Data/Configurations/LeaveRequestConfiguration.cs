using CompanyManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyManagement.Infrastructure.Data.Configurations;

public class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
{
    public void Configure(EntityTypeBuilder<LeaveRequest> builder)
    {
        builder.ToTable("LeaveRequests");

        builder.HasKey(lr => lr.Id);

        builder.Property(lr => lr.Id)
            .ValueGeneratedOnAdd();

        builder.Property(lr => lr.StartDate)
            .IsRequired();

        builder.Property(lr => lr.EndDate)
            .IsRequired();

        builder.Property(lr => lr.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(lr => lr.Reason)
            .HasMaxLength(500);

        builder.Property(lr => lr.CreatedAt)
            .IsRequired();

        builder.HasOne(lr => lr.Employee)
            .WithMany(e => e.LeaveRequests)
            .HasForeignKey(lr => lr.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}