using System.ComponentModel;

namespace CodeNames.Logic
{
    public class Word : INotifyPropertyChanged
    {
        public Word(string text)
        {
            Text = text;
        }

        string text;
        public string Text
        {
            get => text;
            set { text = value; OnPropertyChanged(); }
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
}
