using Windows.UI.Xaml.Controls;

namespace CodeNames.Uwp
{
    public sealed partial class GamePage : ContentControl
    {
        public GamePage(Game game)
        {
            Game = game;
            Game.AddCards();

            InitializeComponent();
        }

        public Game Game { get; }
    }
}
