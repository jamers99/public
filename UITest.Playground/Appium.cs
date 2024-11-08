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

class AppiumText(IControlFactory source) : AppiumControl(source), IText
{
    void EnterText(string text)
    {
        Element.EnterText()
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

interface IAppiumControlTreeNode //this may be better as a base class, but would like to avoid lots of inheritance if possible
{
    IAppiumControlTreeNode Parent { get; set; }
    AppiumWebElement GetElement();
}

class AppiumControl(IControlFactory controlFactory) : IControl, IAppiumControlTreeNode
{
    AppiumControl Parent { get; set; }
    ControlQueryExpression Query { get; set; }

    AppiumWebElement foundElement;
    AppiumWebElement GetElement()
    {
        if (foundElement != null)
            return foundElement;

        if (Parent is not IAppiumControlTreeNode parent) //this feels hacky, probably there's a better way to do this.
            throw new Exception("Parent not set, blah blah helpfulness");

        var parentElement = parent.GetElement();
        var queryBuilder = new AppiumQueryBuilder();
        var appiumQuery = query(queryBuilder);
        return foundElement = parentElement.FindElement(appiumQuery);
    }

    T Child<T>(ControlQueryExpression query) where T : IControl
    {
        var control = controlFactory.Create<T>(this, query);
        if (control is IAppiumControlTreeNode appiumControl)
            appiumControl.Parent = this; //this feels hacky, probably there's a better way to do this.
        return control;
    }
}

class Window(
    AppiumDriver driver, 
    IControlFactory controlFactory) : IWindow, IAppiumControlTreeNode
{
    AppiumControl Parent => null; //top of the tree
    ControlQueryExpression Query { get; set; }

    AppiumWebElement foundElement;
    AppiumWebElement GetElement()
    {
        if (foundElement != null)
            return foundElement;

        var queryBuilder = new AppiumQueryBuilder();
        var appiumQuery = query(queryBuilder);
        return foundElement = driver.FindElement(appiumQuery);
    }

    T Child<T>(ControlQueryExpression query) where T : IControl
    {
        var control = controlFactory.Create<T>(this, query);
        if (control is IAppiumControlTreeNode appiumControl)
            appiumControl.Parent = this; //this feels hacky, probably there's a better way to do this.
        return control;
    }
}

class AppiumDriver() : WindowsDriver<AppiumWebElement> //this may need to be a wrapper, not sure if we can create the WindowsDriver at composition time for it to be in the container
{
    //windowsdriver contains the FindElement(By) method
}

#endregion