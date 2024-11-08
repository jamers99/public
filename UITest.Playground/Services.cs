namespace Services;

#region Control definitions (need implementations in Appium)

public interface IQueryBuilder 
{
    IControlQuery Id(string id);
    IControlQuery Text(string text);
    IControlQuery Type(string type);

    //probably not needed:
    IControlQuery And(params IQuery[] query);
    IControlQuery Or(params IQuery[] query);
}

public interface IControlQuery { }

delegate IControl ControlQueryExpression(IQueryBuilder builder);

public interface IControl
{
    ControlQueryExpression Query { get; set; }
    T Child<T>(ControlQueryExpression query) where T : IControl;
}

public interface IControlFactory
{
    T Create<T>(ControlQueryExpression query) where T : IControl;
}

class ControlFactory(IServiceProvider services) : IControlFactory
{
    T Create<T>(ControlQueryExpression query) where T : IControl
    {
        IControl control = services.GetRequiredService<T>();
        control.Query = query;
        return control;
    }
}

public interface IWindow : IControl
{
    void Minimize();
}

public interface ITextEntry : IControl
{
    void EnterText(string text);
}

public interface IButton : IControl
{
    void EnterText(string text);
}

#endregion

#region Window and page management

interface IAppDriver
{
    IWindow GetWindow();
}

class MainWindow(IAppDriver driver) //singleton
{
    IWindow foundWindow;
    IWindow FindWindow()
    {
        if (foundWindow != null)
            return foundWindow;

        var windowControl = driver.GetWindow();
        return foundWindow = layoutFactory.Create<MainWindow>(windowControl);
    }

    public Page GetPage(string id)
    {
        driver.
        var pageFrame = Frame.Child<IControl>(q => q.Id(id));
        return layoutFactory.Create<Page>(pageFrame);
    }
}

record PageLink(string Entity, string Identifier, string ExpectedTitle)

interface IPageOpener
{
    Page Open();
    PageLink Link { get; set; }
}
    
class PageOpener(MainWindow window)
{
    public PageLink Link { get; set; }
    
    public Page Open()
    {
        var uri = GetUri();
        InvokeUri(uri);

        return window.GetPage(Link.ExpectedTitle);
    }

    void InvokeUri(string uri)
    {
        ProcessInfo.Start(uri);
    }

    string GetUri()
    {
        //todo get the protocal from the info singleton
        return $"kobleerp:{Link.Entity}?{Link.Identifier}";
    }
}

#endregion

#region Layouts (stay the same even if we swap out Appium)

public interface ILayout
{
    IControl Frame {get; set;}
}

class LayoutFactory(IServiceProvider services)
{
    T Create<T>(IControl control) where T : ILayout
    {
        T container = services.GetRequiredService<T>();
        container.Frame = control;
        return container;
    }
}

class Page(LayoutFactory layoutFactory) : ILayout
{
    IControl Frame { get; set; }

    Panel SelectTab(string id)
    {
        Frame
            .Child<IButton>(q => q.Id($"tab button {id}"))
            .Click();

        var tabControl = Frame.Child<IControl>(q => q.Id($"tab {id}"));
        return layoutFactory.Create<Panel>(tabControl);
    }
}

class Panel() : ILayout
{
    IControl Frame { get; set; }

    T GetProperty<T>(string name) where T : IControl
    {
        return Frame.Child<T>(q => q.Id(name));
    }

    T GetProperty<T>(string name) where T : ILayout
    {
        return Frame.Child<T>(q => q.Id(name));
    }
}

#endregion
