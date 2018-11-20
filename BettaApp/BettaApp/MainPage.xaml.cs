using System.Collections.ObjectModel;
using System.Numerics;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BettaApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            var pagesMenuItem = new MenuItem() { Label = "Pages", Symbol = Symbol.DockRight };
            pagesMenuItem.Selected += (_, __) => uiFrame.Navigate(typeof(PageViewer));
            MenuItems.Add(pagesMenuItem);

            var acrylicMenuItem = new MenuItem() { Label = "Acrylic", Symbol = Symbol.FontColor };
            acrylicMenuItem.Selected += (_, __) => uiFrame.Navigate(typeof(AcrylicTest));
            MenuItems.Add(acrylicMenuItem);

            var objectDraggingMenuItem = new MenuItem() { Label = "Dragging", Symbol = Symbol.Directions };
            objectDraggingMenuItem.Selected += (_, __) => uiFrame.Navigate(typeof(ObjectDragging));
            MenuItems.Add(objectDraggingMenuItem);

            objectDraggingMenuItem.IsSelected = true;

            Window.Current.SetTitleBar(uiTitleBar);
        }

        public ObservableCollection<MenuItem> MenuItems { get; set; } = new ObservableCollection<MenuItem>();

        private void HamburgerMenu_HamburgerButtonToggled(object sender, System.EventArgs e)
        {
            uiHamburger.ToggleMenu();
        }

        public static void InitializeDropShadow(UIElement shadowHost, Vector3 offset, Color? color = null)
        {
            Visual hostVisual = ElementCompositionPreview.GetElementVisual(shadowHost);
            Compositor compositor = hostVisual.Compositor;

            // Create a drop shadow
            var dropShadow = compositor.CreateDropShadow();
            dropShadow.Color = color ?? Color.FromArgb(150, 0, 0, 0);
            dropShadow.BlurRadius = 15;
            dropShadow.Offset = offset;

            // Create a Visual to hold the shadow
            var shadowVisual = compositor.CreateSpriteVisual();
            shadowVisual.Shadow = dropShadow;

            // Add the shadow as a child of the host in the visual tree
            ElementCompositionPreview.SetElementChildVisual(shadowHost, shadowVisual);

            // Make sure size of shadow host and shadow visual always stay in sync
            var bindSizeAnimation = compositor.CreateExpressionAnimation("hostVisual.Size");
            bindSizeAnimation.SetReferenceParameter("hostVisual", hostVisual);

            shadowVisual.StartAnimation("Size", bindSizeAnimation);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            uiFrame.Navigate(typeof(AcrylicTest));
        }
    }

    public enum State
    {
        Maximized,
        Minimized,
        Indeterminate
    }
}
