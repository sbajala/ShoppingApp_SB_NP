using System.ComponentModel;

namespace Isi.Utility.ViewModels
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool Visible
        {
            get => visible;
            set
            {
                visible = value;
                NotifyPropertyChanged(nameof(Visible));
            }
        }

        private bool visible;

        protected ViewModel()
        {
            visible = true;
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(sender: this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
