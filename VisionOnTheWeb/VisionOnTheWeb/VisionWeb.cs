using Eagle.Framework.Client.Manager;
using Eagle.Framework.Client.UI;
using Eagle.Framework.Common.Data;

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
            var customerMetaData = context.GetEntityMetaData("SalesInvoice");
            var fetchPlan = FetchPlan.GetEntity(customerMetaData);
            Customer = new PageContext(context, fetchPlan, delayedFetch: true);

            var generalProperties = new PropertyGroupContext(Customer, "General");
            foreach (var link in generalProperties.PropertyGroupLinks.Where(l => l.Kind == Eagle.Framework.Common.MetaData.PropertyGroupLinkKind.Property))
                CustomerProperties.Add(new PropertyContext(generalProperties, link));

            await Customer.RequestDataAsync();
        }

        public PageContext Customer { get; set; }

        public List<PropertyContext> CustomerProperties { get; } = new List<PropertyContext>();

        public bool IsLoggedIn { get; private set; }
    }
}
