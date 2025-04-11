using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace OfflineTasks.MyEbms;

internal class MyEbmsOdata(IOptions<ODataConnection> options)
{
    readonly ODataConnection connection = options.Value;
    readonly HttpClient client = new();

    public async Task<ODataToken?> TokenAsync(string username, string password)
    {
        var body = $"{{\"Username\":\"{username}\",\"Password\":\"{password}\"}}";
        return await TokenPost(body, "Token");
    }

    public async Task<ODataToken?> RefreshTokenAsync(string refreshToken)
    {
        var body = $"{{\"RefreshToken\":\"{refreshToken}\"}}";
        return await TokenPost(body, "RefreshToken");
    }

    async Task<ODataToken?> TokenPost(string body, string path)
    {
        try
        {
            var url = $"{connection.Url}/{path}";
            var content = new StringContent(body, new MediaTypeHeaderValue("application/json"));
            var response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<ODataToken>();
        }
        catch { }
        return null;
    }
}