using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace CodeNames.Uno
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

        void DictionaryPage_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
                NewWord();
        }

        void NewWord_Click(object sender, RoutedEventArgs e) => NewWord();

        void Done_Click(object sender, RoutedEventArgs e)
        {
            if (Game.HasEnoughWords)
                ((ContentControl)Parent).Content = new GamePage(Game);
        }

        void NewWord() => Game.Dictionary.NewWord();

        void Word_Loaded(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (string.IsNullOrEmpty(textBox.Text))
                textBox.Focus(FocusState.Keyboard);
        }

        void AddRandomWords_Click(object sender, RoutedEventArgs e) => Game.Dictionary.AddRandomWords();

        void Remove_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Game.Dictionary.Remove(((FrameworkElement)sender).DataContext as Word);
        }
    }
}
