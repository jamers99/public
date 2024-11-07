
class RunningStage(IEntitySetup setup, IServiceProvider services)
{
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

class PageOpener(
    IUriLauncher launcher, 
    IWindow window)
{
    string Uri { get; set; }
    IPage Open()
    {
        launcher.Invoke(Uri);
        return window.GetChild<IPage>(new ControlQuery("page id or whatever"))
    }
}