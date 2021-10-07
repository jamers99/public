using Eagle.Framework.Common;

namespace VisionOnTheWeb
{
    public class WebSettings : LocalSettings
    {
        Dictionary<string, object> settings = new Dictionary<string, object>();

        protected override T GetValue<T>(string name)
        {
            if (settings.TryGetValue(name, out var value))
                return (T)value;

            return default;
        }

        protected override void SetValue<T>(string name, T value)
        {
            settings.AddOrUpdate(name, value);
        }
    }
}
