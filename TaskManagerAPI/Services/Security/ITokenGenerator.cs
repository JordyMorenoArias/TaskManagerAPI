namespace TaskManagerAPI.Services.Security
{
    public interface ITokenGenerator
    {
        string GenerateToken();
    }
}