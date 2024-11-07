/////// APPIUM ////////

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

class AppiumControl(AppiumDriver driver) : IControl
{
    ControlQuery Query {get; set;}

    AppiumElement foundElement;
    protected AppiumElement GetElement()
    {
        if (foundElement == null)
            foundElement = driver.Find(query);
        
        return foundElement;
    }

    T GetChild<T>(ControlQuery query) where T : IControl;
    {
        
    }
}

class AppiumDriver()
{
    AppiumElement Find(AppiumElement parent, ControlQuery query)
    {

    }
}