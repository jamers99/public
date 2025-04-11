using Microsoft.JSInterop;

namespace OfflineTasks.Networking;

public class NetworkStatus(IJSRuntime jsRuntime) : IAsyncDisposable
{
    private DotNetObjectReference<NetworkStatus>? _dotNetRef;

    public bool IsOnline { get; private set; }

    public void OnChanged(bool isOnline)
    {
        IsOnline = isOnline;
        Changed?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler? Changed;

    public async Task InitializeAsync()
    {
        IsOnline = await jsRuntime.InvokeAsync<bool>("networkHelper.isOnline");
        _dotNetRef = DotNetObjectReference.Create(this);
        await jsRuntime.InvokeVoidAsync("networkHelper.registerOnlineOfflineHandlers", _dotNetRef);
    }

    [JSInvokable]
    public void SetOnlineStatus(bool online)
    {
        IsOnline = online;
        OnChanged(online);
    }

    public ValueTask DisposeAsync()
    {
        _dotNetRef?.Dispose();
        return ValueTask.CompletedTask;
    }
}
