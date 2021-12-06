using System.ComponentModel;
using System.Globalization;

namespace VisionOnTheWeb
{
    [TypeConverter(typeof(PropertyValueConverter))]
    public class BindablePropertyValue
    {
        public BindablePropertyValue(object? value)
        {
            Value = value;
        }
        public object? Value { get; }
    }

    public class PropertyValueConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            return true;
        }

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            return new BindablePropertyValue(value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        {
            return true;
        }

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            return ((BindablePropertyValue?)value)?.Value?.ToString();
        }
    }
}
