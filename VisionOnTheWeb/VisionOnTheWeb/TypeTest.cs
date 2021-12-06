using System.ComponentModel;

namespace VisionOnTheWeb
{
    public class TypeTest : INotifyPropertyChanged
    {
        object? myProperty;
        public object? MyProperty
        {
            get => myProperty;
            set
            {
                myProperty = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(MyPropertyBindable));
            }
        }

        public BindablePropertyValue? MyPropertyBindable
        {
            get { return new BindablePropertyValue(MyProperty); }
            set { MyProperty = value?.Value; OnPropertyChanged(); }
        }

        #region Notify

        /// <summary>
        /// Property Changed event
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

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
