using System;
using CodeNames.Logic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace CodeNames
{
    public sealed partial class CardTile : UserControl
    {
        public CardTile()
        {
            InitializeComponent();
        }

        public Card Card
        {
            get { return (Card)GetValue(CardProperty); }
            set { SetValue(CardProperty, value); }
        }
        public static readonly DependencyProperty CardProperty =
            DependencyProperty.Register(nameof(Card), typeof(Card), typeof(CardTile), new PropertyMetadata(null));

        void Tile_Click(object sender, RoutedEventArgs e)
        {
            var card = (Card)((FrameworkElement)sender).DataContext;
            card.Tap();
        }
    }

    public class CardTypeToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is CardType type)
            {
                switch (type)
                {
                    case CardType.Neutral:
                        return new SolidColorBrush(Colors.AntiqueWhite);
                    case CardType.TeamBlue:
                        return new SolidColorBrush(Colors.DodgerBlue);
                    case CardType.TeamRed:
                        return new SolidColorBrush(Colors.Crimson);
                    case CardType.EndGame:
                        return new SolidColorBrush(new Color() { A = 255, R = 50, G = 50, B = 50 });
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class CardTypeToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is CardType type)
            {
                switch (type)
                {
                    case CardType.Neutral:
                        return new SolidColorBrush(Colors.Black);
                    case CardType.TeamBlue:
                        return new SolidColorBrush(Colors.White);
                    case CardType.TeamRed:
                        return new SolidColorBrush(Colors.White);
                    case CardType.EndGame:
                        return new SolidColorBrush(Colors.White);
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
