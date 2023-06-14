using System;
using System.ComponentModel;
using WPF_LiveChart_MVVM.Model;
using WPF_LiveChart_MVVM.ViewModel.Command;

namespace WPF_LiveChart_MVVM.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {

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

        public RelayCommand SerialCommand { get; set; }
        public RelayCommand UdpCommand { get; set; }

        

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
            ToggleViewModel = new ToggleViewModel();
            OxyPlotViewModel = new OxyPlotViewModel();
            DataBaseViewModel = new DataBaseViewModel();
            CsvViewModel = new CsvViewModel();
            TimerViewModel = new TimerViewModel();
            UdpViewModel = new UdpViewModel(OxyPlotViewModel, ToggleViewModel, DataBaseViewModel, CsvViewModel, TimerViewModel);

        }

        private void ConnectSerial()
        {
            UdpViewModel = null;
            SerialToggle = true;
            UdpToggle = false;
            ToggleViewModel = new ToggleViewModel();
            OxyPlotViewModel = new OxyPlotViewModel();
            DataBaseViewModel = new DataBaseViewModel();
            CsvViewModel = new CsvViewModel();
            TimerViewModel = new TimerViewModel();
            SerialViewModel = new SerialViewModel(OxyPlotViewModel, ToggleViewModel, DataBaseViewModel, CsvViewModel, TimerViewModel);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
