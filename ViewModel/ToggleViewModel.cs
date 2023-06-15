using System.ComponentModel;

namespace WPF_LiveChart_MVVM.ViewModel
{
    class ToggleViewModel : INotifyPropertyChanged
    {
        private bool _mainToggle;
        public bool MainToggle
        {
            get { return _mainToggle; }
            set
            {
                _mainToggle = value;
                OnPropertyChanged(nameof(MainToggle));
            }
        }

        private bool _subToggle;
        public bool SubToggle
        {
            get { return _subToggle; }
            set
            {
                _subToggle = value;
                OnPropertyChanged(nameof(SubToggle));
            }
        }

   

        public ToggleViewModel()
        {
            MainToggle = true;
            SubToggle = true;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
