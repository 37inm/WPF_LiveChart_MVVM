using System.ComponentModel;
using System.Windows;
using WPF_LiveChart_MVVM.Model;
using WPF_LiveChart_MVVM.ViewModel.Command;

namespace WPF_LiveChart_MVVM.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        DatabaseModel databaseModel;

        private SerialViewModel _serialViewModel;
        public SerialViewModel SerialViewModel
        { 
            get { return _serialViewModel; }
            set 
            {
                _serialViewModel = value;
                OnPropertyChanged(nameof(SerialViewModel));
            }
        }
        private UdpViewModel _udpViewModel;
        public UdpViewModel UdpViewModel
        {
            get { return _udpViewModel; }
            set
            {
                _udpViewModel = value;
                OnPropertyChanged(nameof(UdpViewModel));
            }
        }

        private OxyPlotViewModel _oxyPlotViewModel; 
        public OxyPlotViewModel OxyPlotViewModel
        {
            get { return _oxyPlotViewModel; } 
            set
            {
                _oxyPlotViewModel = value;
                OnPropertyChanged(nameof(OxyPlotViewModel));
            }
        }

        private ToggleViewModel _toggleViewModel;
        public ToggleViewModel ToggleViewModel
        {
            get { return _toggleViewModel; }
            set
            {
                _toggleViewModel = value;
                OnPropertyChanged(nameof(ToggleViewModel));
            }
        }

        private DataBaseViewModel _databaseViewModel;
        public DataBaseViewModel DataBaseViewModel
        {
            get { return _databaseViewModel; }
            set
            {
                _databaseViewModel = value;
                OnPropertyChanged(nameof(DataBaseViewModel));
            }
        }

        private CsvViewModel _csvViewModel;
        public CsvViewModel CsvViewModel
        {
            get { return _csvViewModel; }
            set
            {
                _csvViewModel = value;
                OnPropertyChanged(nameof(CsvViewModel));
            }
        }

        private TimerViewModel _timerViewModel;
        public TimerViewModel TimerViewModel
        {
            get { return _timerViewModel; }
            set 
            { 
                _timerViewModel = value;
                OnPropertyChanged(nameof(TimerViewModel));
            }
        }

        private DisplayDataViewModel _displayDataViewModel;
        public DisplayDataViewModel DisplayDataViewModel
        {
            get { return _displayDataViewModel; }
            set
            {
                _displayDataViewModel = value;
                OnPropertyChanged(nameof(DisplayDataViewModel));
            }
        }

        public RelayCommand SerialCommand { get; set; }
        public RelayCommand UdpCommand { get; set; }

        private Visibility _serialVisibility;
        public Visibility SerialVisibility
        {
            get { return _serialVisibility; }
            set
            {
                _serialVisibility = value;
                OnPropertyChanged(nameof(SerialVisibility));
            }
        }

        private Visibility _udpVisibility;
        public Visibility UdpVisibility
        {
            get { return _udpVisibility; }
            set
            {
                _udpVisibility = value;
                OnPropertyChanged(nameof(UdpVisibility));
            }
        }

        private bool _serialToggle;
        public bool SerialToggle
        {
            get { return _serialToggle; }
            set
            {
                _serialToggle = value;
                OnPropertyChanged(nameof(SerialToggle));
            }
        }

        private bool _udpToggle;
        public bool UdpToggle
        {
            get { return _udpToggle; }
            set
            {
                _udpToggle = value;
                OnPropertyChanged(nameof(UdpToggle));
            }
        }



        public MainViewModel()
        {
            SerialCommand = new RelayCommand(ConnectSerial);
            UdpCommand = new RelayCommand(ConnectUdp);
            ConnectSerial();
        }

       
        private void ConnectUdp()
        {
            SerialViewModel = null;
            SerialToggle = false;
            UdpToggle = true;
            SerialVisibility = Visibility.Collapsed;
            UdpVisibility = Visibility.Visible;
            ToggleViewModel = new ToggleViewModel();
            OxyPlotViewModel = new OxyPlotViewModel();
            databaseModel = new DatabaseModel();
            DataBaseViewModel = new DataBaseViewModel(databaseModel);
            CsvViewModel = new CsvViewModel();
            TimerViewModel = new TimerViewModel();
            DisplayDataViewModel = new DisplayDataViewModel();
            UdpViewModel = new UdpViewModel(OxyPlotViewModel, ToggleViewModel, DataBaseViewModel, CsvViewModel, TimerViewModel, DisplayDataViewModel);

        }

        private void ConnectSerial()
        {
            UdpViewModel = null;
            SerialToggle = true;
            UdpToggle = false;
            SerialVisibility = Visibility.Visible;
            UdpVisibility = Visibility.Collapsed;
            ToggleViewModel = new ToggleViewModel();
            OxyPlotViewModel = new OxyPlotViewModel();
            databaseModel = new DatabaseModel();
            DataBaseViewModel = new DataBaseViewModel(databaseModel);
            CsvViewModel = new CsvViewModel();
            TimerViewModel = new TimerViewModel();
            DisplayDataViewModel = new DisplayDataViewModel();
            SerialViewModel = new SerialViewModel(OxyPlotViewModel, ToggleViewModel, DataBaseViewModel, CsvViewModel, TimerViewModel, DisplayDataViewModel);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
