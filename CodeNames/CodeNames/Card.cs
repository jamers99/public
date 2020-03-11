using System.ComponentModel;

namespace CodeNames
{
    public class Card : INotifyPropertyChanged
    {
        public Card(string word, CardType type)
        {
            Word = word;
            Type = type;
        }

        public string Word { get; }

        public CardType Type { get; }

        bool isPreviewing;
        public bool IsPreviewing
        {
            get => isPreviewing;
            set { isPreviewing = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsReveald)); }
        }

        bool tapped;
        public bool Tapped
        {
            get => tapped;
            set { tapped = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsReveald)); }
        }

        public bool IsReveald => Tapped || IsPreviewing;

        public void Tap()
        {
            Tapped = true;
        }

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

    public enum CardType
    {
        Neutral,
        TeamBlue,
        TeamRed,
        EndGame,
    }
}