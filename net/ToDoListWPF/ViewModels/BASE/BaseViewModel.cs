using System.ComponentModel;

/// <summary>
/// Klasa pozwalajaca na wywolanie gdy ktoras z wartosci sie zmieni w tasku
/// </summary>
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
