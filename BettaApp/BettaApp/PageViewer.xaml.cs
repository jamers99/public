using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace BettaApp
{
    public sealed partial class PageViewer : Page
    {
        public ObservableCollection<ColorPage> Pages { get; } = new ObservableCollection<ColorPage>();

        public PageViewer()
        {
            this.InitializeComponent();

            DataContext = this;

            Loaded += PageViewer_Loaded;
        }

        private void PageViewer_Loaded(object sender, RoutedEventArgs e)
        {
            Pages.Add(new ColorPage() { Brush = new SolidColorBrush(Colors.ForestGreen) });
            Pages.Add(new ColorPage() { Brush = new SolidColorBrush(Colors.Goldenrod) });
            Pages.Add(new ColorPage() { Brush = new SolidColorBrush(Colors.Firebrick) });
            Pages.Add(new ColorPage() { Brush = new SolidColorBrush(Colors.DeepSkyBlue) });
            Pages.Add(new ColorPage() { Brush = new SolidColorBrush(Colors.Chocolate) });
            Pages.Add(new ColorPage() { Brush = new SolidColorBrush(Colors.BlueViolet) });
            Pages.Add(new ColorPage() { Brush = new SolidColorBrush(Colors.DarkKhaki) });
            Pages.Add(new ColorPage() { Brush = new SolidColorBrush(Colors.HotPink) });
        }

        DateTime LastAnimateTime;

        private int minimizedCount;
        public int MinimizedCount
        {
            get { return minimizedCount; }
            set { minimizedCount = value; OnPropertyChanged(); }
        }

        public ColorPage LastMinimized
        {
            get { return Pages.LastOrDefault(t => t.State == State.Minimized); }
        }
        public ColorPage FirstMaximized
        {
            get { return Pages.FirstOrDefault(t => t.State == State.Maximized); }
        }
        public ColorPage CurrentIndeterminate
        {
            get { return Pages.FirstOrDefault(t => t.State == State.Indeterminate); }
        }

        private void uiScrollViewer_PointerWheelChanged(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var delta = e.GetCurrentPoint(uiScrollViewer).Properties.MouseWheelDelta;
            var time = 250;

            if (delta > 0)
            {
                if ((LastAnimateTime + TimeSpan.FromMilliseconds(time)) < DateTime.Now)
                {
                    var lastMinimized = Pages.LastOrDefault(t => t.State == State.Minimized);
                    if (lastMinimized != null)
                    {
                        lastMinimized.State = State.Maximized;
                        LastAnimateTime = DateTime.Now;
                    }
                }
            }
            else
            {
                if ((LastAnimateTime + TimeSpan.FromMilliseconds(time)) < DateTime.Now)
                {
                    var firstMaximized = Pages.FirstOrDefault(t => t.State == State.Maximized);
                    if (firstMaximized != null)
                    {
                        firstMaximized.State = State.Minimized;
                        LastAnimateTime = DateTime.Now;
                    }
                }
            }

            UpdateMinimizedCount();
        }

        private void UpdateMinimizedCount()
        {
            int minimizedIndex = 0;
            MinimizedCount = Pages.Count(c => c.State == State.Minimized);
            foreach (var test in Pages.Where(c => c.State == State.Minimized).Reverse())
            {
                test.MinimizedIndex = minimizedIndex;
                minimizedIndex++;
            }

            foreach (var test in Pages.Where(c => c.State == State.Maximized))
            {
                test.MinimizedIndex = null;
            }
        }

        #region Touch

        double totalDelta = 0;

        private void uiScrollViewer_ManipulationDelta(object sender, Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e)
        {
            totalDelta += e.Delta.Translation.X;

            if (CurrentIndeterminate == null)
            {
                UpdateMinimizedCount();

                if (totalDelta > 0)
                {
                    if (LastMinimized != null)
                    {
                        LastMinimized.State = State.Indeterminate;
                        CurrentIndeterminate.Width = ColorPage.minimizedWidth;
                    }
                }
                else
                {
                    if (FirstMaximized != null)
                    {
                        FirstMaximized.State = State.Indeterminate;
                        CurrentIndeterminate.Width = ColorPage.maximizedWidth;
                    }
                }
            }

            if (CurrentIndeterminate != null)
            {
                var totalWidth = CurrentIndeterminate.lastStateWidth + totalDelta;

                if (totalWidth < ColorPage.minimizedWidth)
                {
                    CurrentIndeterminate.State = State.Minimized;
                    totalDelta = 0;
                }
                else if (totalWidth > ColorPage.maximizedWidth)
                {
                    CurrentIndeterminate.State = State.Maximized;
                    totalDelta = 0;
                }
                else
                    CurrentIndeterminate.Width = totalWidth;
            }
        }

        private void uiScrollViewer_ManipulationCompleted(object sender, Windows.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs e)
        {
            if (CurrentIndeterminate != null)
            {
                if (CurrentIndeterminate.lastState == State.Minimized)
                    CurrentIndeterminate.State = (CurrentIndeterminate.Width > ColorPage.minimizedWidth + 150 ? State.Maximized : State.Minimized);
                else if (CurrentIndeterminate.lastState == State.Maximized)
                    CurrentIndeterminate.State = (CurrentIndeterminate.Width > ColorPage.maximizedWidth - 150 ? State.Maximized : State.Minimized);
            }

            totalDelta = 0;
            UpdateMinimizedCount();
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
        public void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    public class ColorPage : INotifyPropertyChanged
    {
        internal const double maximizedWidth = 400;
        public double MaximizedWidth
        {
            get { return maximizedWidth; }
        }

        internal const double minimizedWidth = 50;
        public double MinimizedWidth
        {
            get { return minimizedWidth; }
        }

        private Brush brush;
        public Brush Brush
        {
            get { return brush; }
            set { brush = value; OnPropertyChanged(); }
        }

        private int? minimizedIndex;
        public int? MinimizedIndex
        {
            get { return minimizedIndex; }
            set { minimizedIndex = value; OnPropertyChanged(); }
        }

        private State state = State.Maximized;
        public State State
        {
            get { return state; }
            set { lastState = state; state = value; OnPropertyChanged(); }
        }

        private double width = 0;
        public double Width
        {
            get { return width; }
            set { width = (value < 0 ? 0 : value); OnPropertyChanged(); }
        }

        public State lastState { get; set; }
        public double lastStateWidth { get { return (lastState == State.Minimized ? ColorPage.minimizedWidth : (lastState == State.Maximized ? ColorPage.maximizedWidth : 0)); } }

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

    public class ContainerOverlayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int? index = value as int?;
            if (index.HasValue)
            {
                return new Thickness(0, 0, -((double)index * 10), 0);
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class ContainerOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int? index = value as int?;
            if (index.HasValue)
            {
                return 1 - ((double)index / 10);
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
