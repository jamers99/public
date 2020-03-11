using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace CodeNames.Uno
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        Game game;
        public Game Game
        {
            get => game;
            set { game = value; OnPropertyChanged(); }
        }

        public MainPage()
        {
            InitializeComponent();

            NewGame();
        }

        void NewGame()
        {
            Game = new Game((Game?.Number ?? 0) + 1);
            if (Game.AddCards())
                uiFrame.Content = new GamePage(Game);
            else
                uiFrame.Content = new DictionaryPage(Game);
        }

        void New_Click(object sender, RoutedEventArgs e) => NewGame();

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
