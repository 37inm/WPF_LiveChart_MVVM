using WPF_LiveChart_MVVM.ViewModel.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Windows;
using WPF_LiveChart_MVVM.Model;
using WPF_LiveChart_MVVM.Service;

namespace WPF_LiveChart_MVVM.ViewModel
{
    class SerialViewModel : INotifyPropertyChanged
    {
        private SerialPort _serialCommunication;
        private OxyPlotViewModel _oxyPlotViewModel;
        private DataModel _dataModel;
        private DataBase _database;
        int i = 0;

        private string _data;
        public string Data
        {
            get { return _data; }
            set
            {
                _data = value;
                OnPropertyChanged(nameof(Data));
            }
        }

        private string _serialAvailable;
        public string SerialAvailable
        {
            get { return _serialAvailable; }
            set
            {
                _serialAvailable = value;
                OnPropertyChanged(nameof(SerialAvailable));
            }
        }

        private bool _serialState;
        public bool SerialState
        {
            get { return _serialState; }
            set
            {
                _serialState = value;
                OnPropertyChanged(nameof(SerialState));
            }
        }

        private ObservableCollection<string> _serialPorts;
        public ObservableCollection<string> SerialPorts
        {
            get { return _serialPorts; }
            set
            {
                _serialPorts = value;
            }
        }

        private string _selectedSerialPort;
        public string SelectedSerialPort
        {
            get { return _selectedSerialPort; }
            set
            {
                _selectedSerialPort = value;
                OnPropertyChanged(nameof(SelectedSerialPort));
            }
        }

        public ObservableCollection<int> SerialBaudRate { get; set; }

        private int _selectedSerialBaudRate;
        public int SelectedSerialBaudRate
        {
            get { return _selectedSerialBaudRate; }
            set
            {
                _selectedSerialBaudRate = value;
                OnPropertyChanged(nameof(SelectedSerialBaudRate));
            }
        }

        private RelayCommand _serialCommand;
        public RelayCommand SerialCommand
        {
            get { return _serialCommand; }
            set
            {
                _serialCommand = value;
                OnPropertyChanged(nameof(SerialCommand));
            }
        }

        private RelayCommand _mysqlCommand;
        public RelayCommand MysqlCommand
        {
            get { return _mysqlCommand; ; }
            set
            {
                _mysqlCommand = value;
                OnPropertyChanged(nameof(MysqlCommand));
            }
        }


        public SerialViewModel(OxyPlotViewModel oxyPlotView)
        {
            LoadSerialPorts();
            SerialBaudRate = new ObservableCollection<int> { 9600, 14400, 19200, 38400, 57600, 115200 };
            _database = new DataBase();
            SerialCommand = new RelayCommand(OpenSerial);
            //MysqlCommand = new RelayCommand(_database.OpenDatabase);
            _oxyPlotViewModel = oxyPlotView;
            _dataModel = new DataModel();
            SerialAvailable = "Open";
        }

        private void OpenSerial(string a)
        {
            _serialCommunication = new SerialPort();

            try
            {
                _serialCommunication.PortName = SelectedSerialPort;
                _serialCommunication.BaudRate = SelectedSerialBaudRate;
                _serialCommunication.DataReceived += SerialPort_DataReceived;
                _serialCommunication.Open();
                if (_serialCommunication.IsOpen)
                {
                    //SerialCommand = new RelayCommand(CloseSerial);
                    SerialAvailable = "Close";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string ReceivedData = _serialCommunication.ReadLine();

                Data = ReceivedData;
                _dataModel.Time = i;
                i++;
                _oxyPlotViewModel.GraphUpdate(_dataModel.Time);
                _database.AddData();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CloseSerial()
        {
            try
            {
                _serialCommunication.DataReceived -= SerialPort_DataReceived;
                _serialCommunication.DiscardInBuffer();
                _serialCommunication.Close();
                if (!_serialCommunication.IsOpen)
                {
                    SerialCommand = new RelayCommand(OpenSerial);
                    SerialAvailable = "Open";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadSerialPorts()
        {

            var ports = new List<string>(SerialPort.GetPortNames());
            SerialPorts = new ObservableCollection<string>(ports);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
