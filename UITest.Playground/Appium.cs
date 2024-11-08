using Services;
namespace Appium;

#region Queries

class AppiumQueryBuilder : IQueryBuilder
{
    IControlQuery Id(string id) => new AppiumQuery(MobileBy.Id(id));
    IControlQuery Text(string text) => new AppiumQuery(MobileBy.Text(text));
    IControlQuery Type(string type) => new AppiumQuery(MobileBy.Class(type));
}

record AppiumControlQuery(By By) : IControlQuery
{ }

#endregion

#region Controls

class AppiumWindow(IControlFactory controlFactory) : AppiumControl(controlFactory), IWindow
{
    void Minimize()
    {
        GetElement().Minimize();
    }
}

class AppiumTextEntry(IControlFactory source) : AppiumControl(source), ITextEntry
{
    void EnterText(string text)
    {
        GetElement().EnterText()
    }
}

class AppiumButton(IControlFactory source) : AppiumControl(source), IButton
{
    void Click()
    {
        GetElement().Click()
    }
}

#endregion

#region Element finding

//if we see a way to avoid a base class, lets. 
//It's hard to have more than one ctor dependency as it is, since all controls need to pass it along
class AppiumControl(IControlFactory controlFactory) : IControl 
{
    public ControlQueryExpression Query { get; set; }

    public T Child<T>(ControlQueryExpression query) where T : IControl
    {
        var control = controlFactory.Create<T>(this, query);
        if (control is AppiumControl appiumControl)
            appiumControl.Parent = this; //this feels hacky, probably there's a better way to do this.
        return control;
    }

    internal AppiumControl Parent { get; set; }

    internal void SetUpAsRootNode(AppiumWebElement element)
    {
        if (Parent != null)
            throw new Exception("Parent already set, blah blah helpfulness");

        foundElement = element;
    }

    AppiumWebElement foundElement;
    internal AppiumWebElement GetElement()
    {
        if (foundElement != null)
            return foundElement;

        if (Parent is not AppiumControl parent) //this feels hacky, probably there's a better way to do this.
            throw new Exception("Parent not set, blah blah helpfulness");

        var parentElement = parent.GetElement();
        var queryBuilder = new AppiumQueryBuilder();
        var appiumQuery = query(queryBuilder);
        return foundElement = parentElement.FindElement(appiumQuery);
    }
}

//this may need to be a wrapper, not sure if we can create the WindowsDriver at composition time for it to be in the container
class AppiumDriver(IControlFactory controlFactory) : WindowsDriver<AppiumWebElement>, IAppDriver
{
    public IWindow GetWindow(ControlQueryExpression query) 
    {
        var window = controlFactory.Create<IWindow>(this, query);
        var appiumQuery = query(new AppiumQueryBuilder());
        var windowElement = FindElement(appiumQuery); //WindowsDriver contains the FindElement(By) method
        window.SetUpAsRootNode(windowElement)
    }
}

#endregion