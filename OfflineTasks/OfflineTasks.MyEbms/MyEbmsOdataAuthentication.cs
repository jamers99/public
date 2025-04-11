namespace OfflineTasks.MyEbms
{
    class MyEbmsOdataAuthentication(MyEbmsOdata odata) : IMyEbmsAuthentication
    {
        DateTime? lastAuthentication;
        string? accessToken;
        public bool IsAuthenticated
        {
            get =>
                !string.IsNullOrEmpty(accessToken) &&
                lastAuthentication > DateTime.Today - TimeSpan.FromDays(1);
        }

        public async Task<string?> LoginAsync(string userName, string password)
        {
            if (await odata.TokenAsync(userName, password) is ODataToken token &&
                token.RefreshToken is string refreshToken &&
                token.AccessToken is string accessToken)
            {
                this.accessToken = accessToken;
                lastAuthentication = DateTime.Now;
                return refreshToken;
            }

            return null;
        }

        public async Task<string?> RefreshAsync(string refreshToken)
        {
            if (await odata.RefreshTokenAsync(refreshToken) is ODataToken token &&
                token.RefreshToken is string newRefreshToken &&
                token.AccessToken is string accessToken)
            {
                this.accessToken = accessToken;
                lastAuthentication = DateTime.Now;
                return newRefreshToken;
            }

            return null;
        }
    }
}
