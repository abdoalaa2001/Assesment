using CompanyManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyManagement.Infrastructure.Data.Configurations;

public class SalaryConfiguration : IEntityTypeConfiguration<Salary>
{
    public void Configure(EntityTypeBuilder<Salary> builder)
    {
        builder.ToTable("Salaries");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .ValueGeneratedOnAdd();

        builder.Property(s => s.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(s => s.LastUpdated)
            .IsRequired();

        builder.HasOne(s => s.Employee)
            .WithOne(e => e.Salary)
            .HasForeignKey<Salary>(s => s.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
