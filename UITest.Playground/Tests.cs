using Services;
using Stages;
namespace Tests;

public class TestExampleSandbox(RunningStage stage) : TestBase<RunningStage>(stage)
{
    //The ui takes care of creating the entity because we're "clicking" (actually running hyperlink) the new button
    [Fact]
    public void Opening_NewPage_Test()
    {
        var page = Stage
            .NewEntity("Customer")
            .Open();
    }

    //The server creates the entity (via odata) and the client opens the data that was created.
    [Fact]
    public void Opening_Existing_ViaOData_Test()
    {
        var invoice = new JObject()
        {
            ["SellToCustomer"] = new JObject()
            {
                ["Category"] = "Customers",
                ["Id"] = "ESHJAM99",
                ["PrimaryContact.FullName"] = "Jamers99"
            }
        };
        var page = Stage
            .SetupEntity("Invoice", invoice)
            .Open();
    }

    [Fact]
    public void DefineData_SearchView_Test()
    {
        var searchView = Stage
            .EntitySearchView("Customer")
            .Open();
    }

    [Fact]
    public void Customer_EditTest()
    {
        var page = Stage
            .NewEntity("Customer")
            .Open();
        var fullName = page
            .SelectTab("General")
            .GetProperty<ITextEntry>("PrimaryContact.FullName");

        fullName.EnterText("James Esh");
        Assert.Equal(fullName.Value, "James Esh");
    }

    [Fact]
    public void DataGrid_Test()
    {
        var page = Stage
            .NewEntity("Invoice")
            .Open();
        var details = page
            .SelectTab("Details")
            .GetProperty<IDataGrid>("Details");

        var quantity = details
            .Rows[0]
            .GetCell<ITextEntry>("Quantity");
        quantity.EnterText("5");
    }

    [Fact]
    public void DefineData_ViaOdata_WithoutOpening_Test()
    {
        var invoiceData = new JObject()
        {
            ["SellToCustomer"] = new JObject()
            {
                ["Category"] = "Customers",
                ["Id"] = "ESHJAM99",
                ["PrimaryContact.FullName"] = "Jamers99"
            }
        };
        var invoice = Stage.SetupEntity("Invoice", invoiceData);

        var customerData = new JObject()
        {
            ["Something"] = invoice["Something from the invoice"],
        };
        var page = Stage
            .SetupEntity("Customer", customerData)
            .Open();
    }
}


//////////  SANDBOX PLAYING AROUND  //////////

public class TestSandbox(RunningStage stage) : TestBase<RunningStage>(stage)
{
    // ..../OData/Customer   POST
    // {
    //     "Id": "123",
    //     "Category": "Customers", //probably actually a guid
    //     "FullName": "Kendall Frey"
    // }

    [Fact]
    public void Open_NotChain()
    {
        var newPage = Stage.OpenNewEntity("Invoice");
        var dataPage = Stage.SetupEntity("Invoice", data).OpenPage();

    }

    [Fact]
    public void Open_NotAsChained()
    {
        var page = Stage
            .PageAsExisting("Invoice", invoice)
            .Open();

        var page = Stage
            .PageForNew("Invoice")
            .Open();
            
        var page = Stage
            .PageSearchView("Invoice")
            .Open();
    }

    [Fact]
    public void Open_Chain()
    {
        var page = Stage
            .Page("Invoice")
            .Data(invoice)
            .Open();

        var page = Stage
            .Page("Invoice")
            .New()
            .Open();
            
        var page = Stage
            .Page("Invoice")
            .SearchView()
            .Open();
    }

    [Fact]
    public void DefineData_ViaOdata_AndDelete_Test()
    {
        var invoice = new JObject()
        {
            ["SellToCustomer"] = new JObject()
            {
                ["Category"] = "Customers",
                ["Id"] = "ESHJAM99",
                ["PrimaryContact.FullName"] = "Jamers99"
            }
        };

        using var data = Stage.SetupEntity("Invoice", invoice); //not like because it requires thinking
        var page = data.OpenEntityPage();
    }

    [Fact]
    public void DefineData_Invoice_Test()
    {
        var customer = new JObject()
        {
            ["Id"] = DataHelper.GenerateId(),
            ["Category"] = "Customers",
            ["ContactType"] = "Person",
            ["FullName"] = DataHelper.GenerateName(),
        };
        var invoice = new JObject()
        {
            ["SellTo"] = customer["Id"],
            ["Details"] = new JArray()
            [
                new JObject()
                {
                    ["Product"] = "ADJWRE", //generate product???
                    ["Quantity"] = 5
                }
            ],
        };

        Stage.SetupEntity("Customer", data)
             .Open();
        Stage.SetupEntity("Customer")
             .Open();

        var page = Stage.Page.Open("Invoice", data);
        var sellTo = page
            .SelectTab("Details")
            .GetProperty<EntitySearch>("SellTo");

        sellTo.SelectEntity(data["Id"]);
        Assert.Equal(fullName.Value, "James Esh");
    }

    [Fact]
    public void Customer_CreateTest()
    {
        var page = Stage.OpenNewPage("Customer");  //OpenPageForNew("Customer"), OpenPageForExisting("Customer", "DOEJOH")
        page.NavigateTo("General")
            .Select<Text>("Id")
            .EnterText("Blah");

        var otherTab = page.Tab("Other");
        otherTab.Property<Text>("Test")
                .EnterText("Foo");
        otherTab.Property<CheckBox>("Ying")
                .Check();

        otherTab.Property<Text>("Test")
                .AssertText("Foo"); //todo

        page.Tab("General").Select();
        page.Property<Text>("FirstName").EnterText("Kendall");
        Assert.Equal(page.Property<Text>("FullName").Value, "Kendall Frey");

        page.SelectTab("General");
        page.Property<Text>("FirstName")
            .EnterText("Kendall");
    }

    [Fact]
    public void Path_Hyperlink_Test()
    {
        var fullName = Stage
            .OpenNewPage("Customer")
            .Focus<Text>("General>PrimaryContact.FullName");

        fullName.EnterText("James Esh");
        Assert.Equal(fullName.Value, "James Esh");
    }

    [Fact]
    public void Assertion_Test()
    {
        var fullName = Stage
            .OpenNewPage("Customer")
            .Focus<Text>("General>PrimaryContact.FullName");

        fullName.AssertText("James Esh"); //not this (don't build our own assertion)
        Assert.Equal(fullName.Value, "James Esh"); //always this
    }

    [Fact]
    public void James_Test()
    {
        var page = Stage.OpenNewPage("Customer");  //OpenPageForNew("Customer"), OpenPageForExisting("Customer", "DOEJOH")
        page.Tab("General")
            .Property<Text>("Id")
            .Focus() //goes via clicking tab then clicking property
            .EnterText("Blah");

        page.Tab("General")
            .Property<Text>("Id")
            .EnterText("Blah"); //does Focus() via hyperlink under the hood


        var otherTab = page.Tab("Other");
        otherTab.Property<Text>("Test")
                .EnterText("Foo");
        otherTab.Property<CheckBox>("Ying")
                .Check();
    }

    [Fact]
    public void Customer_DisposeTest()
    {
        var firstName;
        // ok crazy idea time
        using (var general = page.SelectTab("General"))
        {
            // PEBCAKException: page.SelectTab("Details"); // You can't select this tab while another one is active
            firstName = page.Property("FirstName");
        }
        firstName.Whatever // error
    }
}
