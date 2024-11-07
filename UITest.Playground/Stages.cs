class LoginStage(IWindow window)
{
    void Login()
    {
        window
            .GetChild<Control>(q => q.Id("LoginPopup"))
            .GetChild<IText>(q => q.Name("Username"))
            .EnterText("admin");
    }
}




class RunningStage(LoginStage loginStage, IEntitySetup setup, IServiceProvider services)
{
    void TestSetup()
    {
        loginStage.Login();
    }

    public PageOpener SetupData(string entityName, JObject data)
    {
        JObject data = setup.CreateEntity(entityName, data);
        
        var scope = services.CreateScope();
        var page = scope.Services.GetRequiredService<PageOpener>();
        page.Uri = data["guid"]; //bad but will be better
        return page;
    }

    public PageOpener NewEntityPage(string entityName)
    {
        var scope = services.CreateScope();
        var page = scope.Services.GetRequiredService<PageOpener>();
        page.Uri = "/new"; //bad but will be better
        return page;
    }
}