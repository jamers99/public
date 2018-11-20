using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace BettaApp
{
    public sealed partial class HamburgerMenu : UserControl, INotifyPropertyChanged
    {
        public HamburgerMenu()
        {
            this.InitializeComponent();

            MainPage.InitializeDropShadow(uiShadowHost, new Vector3(0, 0, 0));
        }

        public void ToggleMenu()
        {
            State = (State == State.Maximized ? State.Minimized : State.Maximized);
        }

        private double menuActualWidth = 0;
        internal double MenuActualWidth
        {
            get { return menuActualWidth; }
            set
            {
                if (value < 0)
                    menuActualWidth = 0;
                else if (value > MenuOpenWidth + 20)
                    menuActualWidth = MenuOpenWidth + 20;
                else
                    menuActualWidth = value;

                OnPropertyChanged();
            }
        }

        private double menuOpenWidth = 300;
        public double MenuOpenWidth
        {
            get { return menuOpenWidth; }
            set { menuOpenWidth = value; OnPropertyChanged(); }
        }

        private State state = State.Minimized;
        internal State State
        {
            get { return state; }
            set
            {
                if (state != State.Indeterminate && value == State.Indeterminate)
                    BeginWidth = MenuActualWidth;

                state = value;
                OnPropertyChanged();

                switch (state)
                {
                    case State.Maximized:
                        MenuActualWidth = MenuOpenWidth;
                        break;
                    case State.Minimized:
                        MenuActualWidth = 0;
                        break;
                }
            }
        }

        private double beginWidth;
        public double BeginWidth
        {
            get { return beginWidth; }
            set { beginWidth = value; OnPropertyChanged(); }
        }

        private double gripWidth = 50;
        public double GripWidth
        {
            get { return gripWidth; }
            set { gripWidth = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The list of menu items
        /// </summary>
        public ObservableCollection<MenuItem> MenuItems
        {
            get { return (ObservableCollection<MenuItem>)GetValue(MenuItemsProperty); }
            set { SetValue(MenuItemsProperty, value); }
        }
        public static readonly DependencyProperty MenuItemsProperty =
            DependencyProperty.Register("MenuItems", typeof(ObservableCollection<MenuItem>), typeof(HamburgerMenu), new PropertyMetadata(new ObservableCollection<MenuItem>()));

        #region Notify

        /// <summary>
        /// Property Changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Fire the PropertyChanged event
        /// </summary>
        /// <param name="propertyName">Name of the property that changed (defaults from CallerMemberName)</param>
        public void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void Grid_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (State == State.Indeterminate)
                MenuActualWidth = BeginWidth + e.Cumulative.Translation.X;
        }

        private void Grid_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            var x = e.Position.X;

            if (State == State.Maximized)
                State = State.Indeterminate;

            if (State == State.Minimized && x < GripWidth)
            {
                MenuActualWidth = e.Position.X;
                State = State.Indeterminate;
            }
        }

        private void Grid_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            State = (MenuActualWidth > (MenuOpenWidth / 2) ? State.Maximized : State.Minimized);
        }

        private void OpenedExtraSpace_Tapped(object sender, TappedRoutedEventArgs e)
        {
            State = State.Minimized;
        }

        private void MenuItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var itemTapped = (MenuItem)((FrameworkElement)sender).DataContext;

            foreach (var item in MenuItems)
            {
                item.IsSelected = item == itemTapped;
            }

            ToggleMenu();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ToggleMenu();
        }
    }

    public class MenuItem : INotifyPropertyChanged
    {
        private string label;
        public string Label
        {
            get { return label; }
            set { label = value; OnPropertyChanged(); }
        }

        private Symbol symbol;
        public Symbol Symbol
        {
            get { return symbol; }
            set { symbol = value; OnPropertyChanged(); }
        }

        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (value != isSelected)
                {
                    isSelected = value;

                    if (isSelected)
                        OnSelected();
                }

                OnPropertyChanged();
            }
        }

        #region Selected Event

        public event EventHandler Selected;
        internal virtual void OnSelected()
        {
            Selected?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Notify

        /// <summary>
        /// Property Changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Fire the PropertyChanged event
        /// </summary>
        /// <param name="propertyName">Name of the property that changed (defaults from CallerMemberName)</param>
        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
