/////// APPIUM ////////

class AppiumQueryBuilder : IQueryBuilder
{
    IQuery Id(string id) => new AppiumQuery(MobileBy.Id(id));
    IQuery Text(string text) => new AppiumQuery(MobileBy.Text(text));
    IQuery Type(string type) => new AppiumQuery(MobileBy.Class(type));
}

record AppiumQuery(By By) : IQuery
{ }




class AppiumControlFactory(
    IServiceProvider services) 
{
    T Create<T>(ControlQuery query) where T : IControl
    {
        IControl control = services.GetRequiredService<T>();
        control.Init(query);
        return control;
    }
}

class AppiumText() : AppiumControl, IText
{
    void EnterText(string text)
    {
        Element.EnterText()
    }
}

class AppiumButton() : AppiumControl, IButton
{
    void EnterText(string text)
    {
        Element.EnterText()
    }
}

class AppiumControl(AppiumControlFactory controlFactory, IQueryBuilder queryBuilder) : IControl
{
    ControlQuery Query {get; set;}

    AppiumWebElement foundElement;
    AppiumWebElement GetElement()
    {
        if (foundElement == null)
            foundElement = driver.Find(query);
        
        return foundElement;
    }

    T GetChild<T>(ControlQuery query) where T : IControl
    {
        var appiumQuery = (AppiumQuery)query(queryBuilder);
        var element = GetElement();
        return controlFactory.Create<T>(element);
    }
}

class Window(AppiumDriver driver) : IWindow
{
    ControlQuery Query {get; set;}

    T GetChild<T>(ControlQuery query) where T : IControl
    {
        var appiumQuery = (AppiumQuery)query(queryBuilder);
        var element = driver.FindElement(appiumQuery);
        return controlFactory.Create<T>(element);
    }
}

class AppiumDriver() : WindowsDriver<AppiumWebElement>
{
    AppiumWebElement Find(ControlQuery query)
    {
        var parent = 
        var appiumQuery = parser.ToBy(query);
        return parent.FindElement(appiumQuery);
    }
}