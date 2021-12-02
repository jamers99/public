using Eagle.Framework.Client.Data;
using Eagle.Framework.Client.Manager;
using Eagle.Framework.Client.UI;
using Eagle.Framework.Common.Data;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace VisionOnTheWeb
{
    public class VisionWeb
    {
        public VisionWeb()
        {
            new WebApp().InitClientManager();
        }

        public bool IsLoggedIn { get; private set; }

        public string Message { get; private set; } = "Not logged in";

        public async Task Login(string server = "localhost", string password = "Pw123456")
        {
            IsLoggedIn = Global.ClientManager.IsLoggedIn;
            if (!IsLoggedIn)
            {
                LoginManager login = Global.ClientManager.LoginManager;
                login.LoginData.Server = server;
                login.LoginData.CompanyId = "Demo";
                login.LoginData.Username = "Administrator";
                login.UnencryptedPassword = password;
                await login.LoginAsync();
                IsLoggedIn = Global.ClientManager.IsLoggedIn;
            }

            Message = IsLoggedIn ? "Logged in!" : "Login failed";
        }

        public PageContext? Customer { get; set; }

        public List<PropertyContext<string>> CustomerProperties { get; } = new List<PropertyContext<string>>();

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
            foreach (var path in new[] { "PrimaryContact.FirstName", "PrimaryContact.LastName", "PrimaryContact.FullName", "PrimaryContact.MailingAddress.City" })
            {
                var property = new PropertyContext<string>(general, path);
                property.PropertyChanged += OnPropertyUpdated;
                CustomerProperties.Add(property);
            }

            await Customer.RequestDataAsync();
        }

        public async Task GetInvoice()
        {
            if (!IsLoggedIn)
                return;

            var context = await Global.ClientManager.Session.CreateContextAsync("GetInvoice");
            var customerMetaData = context.GetEntityMetaData("SalesInvoice");
            var fetchPlan = FetchPlan.GetEntity(customerMetaData);
            var invoiceContext = new PageContext(context, fetchPlan, delayedFetch: true);
            var invoiceDetailsContext = new PropertyContext(invoiceContext, "TopLevelDetails");


            foreach (var entity in (IEnumerable)invoiceDetailsContext.PropertyValue)
            {
                var entityContext = new BaseUIContext(invoiceDetailsContext, (DynamicEntity)entity);
                var pg = new PropertyGroupContext(entityContext, "MyCustomDetialsGroup"); //TODO should be element binding
                //blazor code: new PropertyPanel(pg);
            }

            var invoiceDetailsContext2 = new EntityCollectionPropertyContext<ItemPropertyGroupContext>(invoiceContext, "TopLevelDetails", (c, e) => new ItemPropertyGroupContext(c, e));
            foreach (var entity in invoiceDetailsContext2.Items)
            {
                //blazor code: new PropertyPanel(entity.Group);
            }
        }

        public class ItemPropertyGroupContext : BaseUIContext
        {
            public PropertyGroupContext Group { get; set; }
            public ItemPropertyGroupContext(BaseUIContext parentCollection, DynamicEntity collectionItem) : base(parentCollection, collectionItem)
            {
                Group = new PropertyGroupContext(this, "MyCustomDetialsGroup"); //TODO should be element binding
            }
        }

        public event EventHandler? PropertyUpdated;
        protected virtual void OnPropertyUpdated(object? s, PropertyChangedEventArgs e)
        {
            if (Customer != null)
                Message = Customer.Context.HasChanges ? "Not saved" : "All saved";

            PropertyUpdated?.Invoke(this, EventArgs.Empty);
        }

        public async Task Save()
        {
            if (Customer != null)
            {
                await Customer.SaveCommand.ExecuteAsync(this);
                Message = "Saved!";
            }
        }
    }
}
