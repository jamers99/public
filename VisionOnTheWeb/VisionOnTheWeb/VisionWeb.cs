using Eagle.Framework.Client.Manager;
using Eagle.Framework.Client.UI;
using Eagle.Framework.Common.Data;
using System.Collections.ObjectModel;

namespace VisionOnTheWeb
{
    public class VisionWeb
    {
        public VisionWeb()
        {
            new WebApp().InitClientManager();
        }

        public async Task Login(string server = "localhost", string password = "Pw123456")
        {
            IsLoggedIn = Global.ClientManager.IsLoggedIn;
            if (IsLoggedIn)
                return;

            LoginManager login = Global.ClientManager.LoginManager;
            login.LoginData.Server = server;
            login.LoginData.CompanyId = "Demo";
            login.LoginData.Username = "Administrator";
            login.UnencryptedPassword = password;
            await login.LoginAsync();
            IsLoggedIn = Global.ClientManager.IsLoggedIn;
        }

        public async Task GetCustomer()
        {
            if (!IsLoggedIn)
                return;

            var context = await Global.ClientManager.Session.CreateContextAsync("GetCustomer");
            var customerMetaData = context.GetEntityMetaData("Customer");
            var fetchPlan = FetchPlan.GetEntity(customerMetaData);
            Customer = new PageContext(context, fetchPlan, delayedFetch: true);

            CustomerProperties.Clear();
            var general = new PropertyGroupContext(Customer, "General");
            foreach (var path in new[] { "PrimaryContact.FullName", "PrimaryContact.MailingAddress.City" })
            {
                var property = new PropertyContext<string>(general, path);
                CustomerProperties.Add(property);
            }

            await Customer.RequestDataAsync();
        }

        public async Task Save()
        {
            await Customer.SaveCommand.ExecuteAsync(this);
        }

        public PageContext Customer { get; set; }

        public List<PropertyContext<string>> CustomerProperties { get; } = new List<PropertyContext<string>>();

        public bool IsLoggedIn { get; private set; }
    }
}
