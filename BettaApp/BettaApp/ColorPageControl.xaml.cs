using System;
using System.ComponentModel;
using System.Numerics;
using Windows.UI.Composition;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace BettaApp
{
    public sealed partial class ColorPageControl : UserControl
    {
        public ColorPageControl()
        {
            this.InitializeComponent();
            MainPage.InitializeDropShadow(uiShadowHost, new Vector3(-10, 0, 0));
        }
    }
}
