using Eagle.Core;
using Eagle.Framework.Client.Manager;
using Eagle.Framework.Client.Navigation;
using Eagle.Framework.Common;

namespace VisionOnTheWeb
{
    public class WebApp : IEagleApp
    {
        public string Name { get; } = "Vision";
        public string Title { get; } = "Vision on the web";
        public string Version { get; }
        public string StoreUrl { get; }
        public LoginData OverrideCredentials { get; set; }
        public string AzureAppCenterSecret { get; }

        public event EventHandler<KeyStateChangedEventArgs> KeyStateChanged;
        public event EventHandler<DecodedDataEventArgs> DecodedData;

        public Task<GeolocationPoint> GetCurrentLocation(TimeSpan timeout)
        {
            return Task.FromResult(new GeolocationPoint(0, 0));
        }

        public LocalSettings GetNewLocalSettings()
        {
            return new WebSettings();
        }

        public BasePageManager GetNewPageManager()
        {
            return new WebPageManager(Global.ClientManager);
        }

        public void InitPlatform()
        {
        }

        public void InitRegions()
        {
        }

        public void NavigateToLogin()
        {
        }

        public void NavigateToShell()
        {
        }

        public void OpenLocationSettings()
        {
        }

        public void PlaySound(Stream stream)
        {
        }

        public void RemoveNotification(string tag)
        {
        }

        public void SendNotification(string title, string message, string tag)
        {
        }
    }
}
