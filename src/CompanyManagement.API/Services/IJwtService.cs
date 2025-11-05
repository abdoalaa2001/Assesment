using CompanyManagement.Domain.Entities;

namespace CompanyManagement.API.Services;

public interface IJwtService
{
    string GenerateToken(User user);
}