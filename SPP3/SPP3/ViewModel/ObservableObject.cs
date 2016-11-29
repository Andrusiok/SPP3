using System.ComponentModel;

namespace SPP3.ViewModel
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChangedEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void Add(Methods method)
        {

        }

        public virtual int Time
        {
            get; set;
        }

        public ObservableObject Parent = null;
    }
}
