using System.ComponentModel;
using CodeNames.Logic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace CodeNames
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        Game game = new Game();
        public Game Game
        {
            get => game;
            set { game = value; OnPropertyChanged(); }
        }

        int gameCount = 1;
        public int GameCount
        {
            get => gameCount;
            set { gameCount = value; OnPropertyChanged(); }
        }

        public MainPage()
        {
            InitializeComponent();
        }

        void New_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Game = new Game();
            GameCount++;
        }

        void SymbolIcon_PointerPressed(object sender, PointerRoutedEventArgs e) => Game.IsPreviewing = true;

        void SymbolIcon_PointerReleased(object sender, PointerRoutedEventArgs e) => Game.IsPreviewing = false;

        #region Notify

        /// <summary>
        /// Property Changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Fire the PropertyChanged event
        /// </summary>
        /// <param name="propertyName">Name of the property that changed (defaults from CallerMemberName)</param>
        void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
