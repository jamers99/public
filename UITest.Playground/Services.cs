//////// APPIUM-AGNOSTIC ///////

interface IQueryBuilder 
{
    IQuery Id(string id);
    IQuery Text(string text);
    IQuery Type(string type);

    //probably not needed:
    IQuery And(params IQuery[] query);
    IQuery Or(params IQuery[] query);
}

interface IQuery { }

delegate IControl ControlQuery(IQueryBuilder builder);

interface IControl
{
    ControlQuery Query {get;set;}
    T GetChild<T>(ControlQuery query) where T : IControl;
}

interface IWindow : IControl
{
}

interface IText : IControl
{
    void EnterText(string text);
}

interface IButton : IControl
{
    void EnterText(string text);
}

interface ILayout
{
    IControl Frame {get; set;}
}

class PageOpener(
    IUriLauncher launcher, 
    IWindow window)
{
    string Uri { get; set; }
    string ExpectedId { get; set; }
    Page Open()
    {
        launcher.Invoke(Uri);
        var pageControl = window.GetChild<IControl>(q => q.Id(ExpectedId));
        return containerFactory.Create<Page>(pageControl);
    }
}


class LayoutFactory(IServiceProvider services)
{
    T Create<T>(IControl control) where T : IControlContainer
    {
        T container = services.GetRequiredService<T>();
        container.Frame = control;
        return container;
    }
}

class Page(ILogger logger, LayoutFactory layoutFactory) : ILayout
{
    IControl Frame { get; set; }

    Panel SelectTab(string id)
    {
        Frame
            .GetChild<IButton>(q => q.Id($"tab button {id}"))
            .Click();

        var tabControl = Frame.GetChild<IControl>(q => q.Id($"tab {id}"));
        return layoutFactory.Create<Panel>(tabControl);
    }
}

class Panel() : ILayout
{
    IControl Frame { get; set; }

    T GetProperty<T>(string name) where T : IControl
    {
        return Frame.GetChild<T>(q => q.Id(name));
    }

    T GetProperty<T>(string name) where T : ILayout
    {
        return Frame.GetChild<T>(q => q.Id(name));
    }
}