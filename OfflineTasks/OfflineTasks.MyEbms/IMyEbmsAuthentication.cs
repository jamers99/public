namespace OfflineTasks.MyEbms;

public interface IMyEbmsAuthentication
{
    bool IsAuthenticated { get; }

    Task<string?> LoginAsync(string userName, string password);

    Task<string?> RefreshAsync(string refreshToken);
}
