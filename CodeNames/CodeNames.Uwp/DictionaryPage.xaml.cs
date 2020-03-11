using CodeNames.Logic;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CodeNames
{
    public sealed partial class DictionaryPage : ContentControl
    {
        public Game Game { get; }

        public DictionaryPage(Game game)
        {
            Game = game;

            InitializeComponent();

            KeyDown += DictionaryPage_KeyDown;
        }

        void DictionaryPage_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
                NewWord();
        }

        void NewWord_Click(object sender, RoutedEventArgs e) => NewWord();

        void Done_Click(object sender, RoutedEventArgs e)
        {
            if (Game.HasEnoughWords())
                ((ContentControl)Parent).Content = new GamePage(Game);
        }

        void NewWord() => Game.Dictionary.NewWord();

        void Word_Loaded(object sender, RoutedEventArgs e) => ((Control)sender).Focus(FocusState.Keyboard);

        void AddDefaultWords_Click(object sender, RoutedEventArgs e) => Game.Dictionary.AddDefaultWords();

        void Remove_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Game.Dictionary.Remove(((FrameworkElement)sender).DataContext as Word);
        }
    }
}
