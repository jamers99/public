//////// APPIUM-AGNOSTIC ///////

record ControlQuery(string Id, /*etc, etc...*/)
{ }

interface IControl
{
    ControlQuery Query {get;set;}
    T GetChild<T>(ControlQuery query) where T : IControl;
}

interface IText : IControl
{
    void EnterText(string text);
}

interface IButton : IControl
{
    void EnterText(string text);
}

interface IControlContainer
{
    IControl Control {get; set;}
}

class Page() : IControlContainer
{
    IControl Control { get; set; }

    Panel SelectTab(string id)
    {
        Control.GetChild<IButton>(new ControlQuery("tab button id")).Click();
        return Control.GetChild<ITab>(new ControlQuery("tab id"));
    }
}

class Panel() : IControlContainer
{
    IControl Control { get; set; }

    T GetProperty<T>(string name) where T : IProperty
    {
        var Control = Control.GetChild<T>(new ControlQuery(name));
        return propertyFactory.Create<T>(control);
    }
}

interface IProperty
{
     
}

class TextProperty : IProperty
{
    void EnterText()
}

