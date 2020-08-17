using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WPF.OCR.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName()] string name = null)
        {
            if (name is null) return;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
