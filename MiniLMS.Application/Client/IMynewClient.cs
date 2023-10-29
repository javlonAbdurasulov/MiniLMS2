namespace MiniLMS.Application.Client
{
    public interface IMynewClient
    {
        Task<string> GetFreeApi();
    }
}