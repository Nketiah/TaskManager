
namespace TaskManager.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Guid id, string email);
    }
}
