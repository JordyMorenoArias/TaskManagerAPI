namespace TaskManagerAPI.Services
{
    public interface IETagHelper
    {
        public abstract string GenerateETag(object obj);
    }
}