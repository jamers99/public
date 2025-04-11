namespace OfflineTasks.Components;

using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using OfflineTasks.MyEbms;
using OfflineTasks.Networking;

public partial class EbmsAuthenticationPopup : IDisposable
{
    [Parameter] public bool ShowPrompt { get; set; }
    [Parameter] public EventCallback OnAuthenticated { get; set; }

    [Inject] public required ILocalStorageService Storage { get; set; }
    [Inject] public required NetworkStatus Network { get; set; }
    [Inject] public required IMyEbmsAuthentication MyEbmsAuth { get; set; }

    string Username = "";
    string Password = "";
    string ErrorMessage = "";

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await Network.InitializeAsync();
        await UpdateAuthentication();
        Network.Changed += Network_Changed;
    }

    void Network_Changed(object? sender, EventArgs e)
    {
        if (Network.IsOnline)
            _ = UpdateAuthentication();
    }

    const string EBMS_TOKEN = "ebms_token";
    async Task UpdateAuthentication()
    {
        if (MyEbmsAuth.IsAuthenticated)
            return;

        if (!Network.IsOnline)
            return;

        if (await Storage.GetItemAsync<string>(EBMS_TOKEN) is string token &&
            await MyEbmsAuth.RefreshAsync(token) is string refreshedToken)
        {
            await Storage.SetItemAsync(EBMS_TOKEN, refreshedToken);
        }
        else
        {
            ShowPrompt = true;
        }
    }

    async Task PasswordLogin()
    {
        if (await MyEbmsAuth.LoginAsync(Username, Password) is string token)
        {
            await Storage.SetItemAsync(EBMS_TOKEN, token);
            ShowPrompt = false;
        }
        else
        {
            ErrorMessage = "Sign in failed, check your username and password.";
        }
    }

    public void Dispose()
    {
        Network.Changed -= Network_Changed;
    }
}
