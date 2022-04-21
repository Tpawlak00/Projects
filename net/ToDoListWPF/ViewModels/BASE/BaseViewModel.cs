using System.ComponentModel;

namespace ToDoListWPF
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (s,e) => { };

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(name)));
        }
    }
}
