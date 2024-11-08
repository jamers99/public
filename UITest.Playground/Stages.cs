using Services;
namespace Stages;

public interface IStage
{
    void TestSetup();
}

public class LoginStage(IWindow window) : IStage
{
    void Login()
    {
        window
            .Child<Control>(q => q.Id("LoginPopup"))
            .Child<ITextEntry>(q => q.Name("Username"))
            .EnterText("admin");
    }
}

public class RunningStage(LoginStage loginStage, IEntitySetup setup, IServiceProvider services) : IStage
{
    public void TestSetup()
    {
        loginStage.Login();
    }

    public IPageOpener SetupEntity(string entityName, JObject data)
    {
        JObject data = setup.CreateEntity(entityName, data);
        
        var scope = services.CreateScope();
        var page = scope.Services.GetRequiredService<IPageOpener>();
        page.Link = new(/*data["guid"]*/);
        return page;
    }

    public IPageOpener NewEntity(string entityName)
    {
        var scope = services.CreateScope();
        var page = scope.Services.GetRequiredService<IPageOpener>();
        page.Link = new(/*"/new"*/);
        return page;
    }
}
