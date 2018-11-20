using MoeBetta;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BettaApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AcrylicTest : Page
    {
        public AcrylicTest()
        {
            this.InitializeComponent();
            Loaded += AcrylicTest_Loaded;
        }

        private void AcrylicTest_Loaded(object sender, RoutedEventArgs e)
        {
            uiBackground.Children.Add(new TextBlock() { Text = new CoolThing().MyThing });
        }
    }
}
