using System;
using System.Threading;
using System.Xml;

namespace _20_20_20_rule
{
    class Program
    {
        static void Main(string[] args)
        {
            string xml =
@"
<toast>
    <visual>
        <binding template=""ToastText02"">
            <text id=""1"">It's been 20 minutes!</text>
            <text id=""2"">Look 20ft away for 20 seconds, stand up, drink some water and be happy!</text>
        </binding>
    </visual>
</toast> 
";

            var doc = new XmlDocument();
            doc.LoadXml(xml);

            while (true)
            {
                Thread.Sleep(TimeSpan.FromMinutes(20));
                Toast(doc);
            }
        }

        private static void Toast(XmlDocument doc)
        {
            // And create the toast notification
            var toast = new ToastNotification(doc);

            // And then show it
            DesktopNotificationManagerCompat.CreateToastNotifier().Show(toast);
        }
    }
}
