using System.ComponentModel;

namespace Isi.Utility.ViewModels
{
    public abstract class WindowViewModel : ViewModel, INotifyPropertyChanged
    {
        public string Title
        {
            get => title;
            set
            {
                if (title != value)
                {
                    title = value;
                    NotifyPropertyChanged(nameof(Title));
                }
            }
        }

        public ViewModel Content
        {
            get => content;
            set
            {
                if (content != value)
                {
                    content = value;
                    NotifyPropertyChanged(nameof(Content));
                }
            }
        }

        private string title;
        private ViewModel content;

        protected WindowViewModel() : base()
        { }
    }
}
