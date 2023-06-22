using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Windows;
using WPF_LiveChart_MVVM.Model;
using WPF_LiveChart_MVVM.ViewModel.Command;

namespace WPF_LiveChart_MVVM.ViewModel
{
    class SerialViewModel : INotifyPropertyChanged
    {
        private SerialPort _serialCommunication;
        private OxyPlotViewModel _oxyPlotViewModel;
        private ToggleViewModel _toggleViewModel;
        private DataBaseViewModel _databaseViewModel;
        private CsvViewModel _csvViewModel;
        private TimerViewModel _timerViewModel;
        private DisplayDataViewModel _displayDataViewModel;
        private DataModel _dataModel;

        private string _serialContent;
        public string SerialContent
        {
            get { return _serialContent; }
            set
            {
                _serialContent = value;
                OnPropertyChanged(nameof(SerialContent));
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


        public ObservableCollection<string> SerialPorts { get; set; }

        public ObservableCollection<int> SerialBaudRate { get; set; }

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


        public SerialViewModel(OxyPlotViewModel oxyPlotView, ToggleViewModel toggleViewModel, DataBaseViewModel dataBaseViewModel, CsvViewModel csvViewModel, TimerViewModel timerViewModel, DisplayDataViewModel displayDataViewModel)
        {

            LoadSerialPorts();
            SerialBaudRate = new ObservableCollection<int> { 9600, 14400, 19200, 38400, 57600, 115200 };

            SerialCommand = new RelayCommand(OpenSerial);

            _oxyPlotViewModel = oxyPlotView;
            _toggleViewModel = toggleViewModel;
            _databaseViewModel = dataBaseViewModel;
            _csvViewModel = csvViewModel;
            _timerViewModel = timerViewModel;
            _displayDataViewModel = displayDataViewModel;

            _dataModel = new DataModel();

            SerialContent = "Open";

            SerialState = true;
        }

        private void OpenSerial()
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
                    _timerViewModel.Start();
                    SerialCommand = new RelayCommand(CloseSerial);
                    SerialContent = "Close";
                    SerialState = false;
                    _toggleViewModel.MainToggle = false;
                    _toggleViewModel.SubToggle = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void CloseSerial()
        {
            try
            {
                _serialCommunication.DataReceived -= SerialPort_DataReceived;
                _serialCommunication.DiscardInBuffer();
                _serialCommunication.Close();
                if (_databaseViewModel.MysqlState)
                {
                    _databaseViewModel.CloseDatabase();
                }
                if (_csvViewModel.CsvState)
                {
                    _csvViewModel.Close();
                }
                if (!_serialCommunication.IsOpen)
                {
                    SerialCommand = new RelayCommand(OpenSerial);
                    SerialContent = "Open";
                    SerialState = true;
                    _timerViewModel.Stop();
                    _toggleViewModel.MainToggle = true;
                    _toggleViewModel.SubToggle = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string ReceivedData = _serialCommunication.ReadLine();
                string[] splitData = ReceivedData.Split('/');
                bool bl = double.TryParse(splitData[0], out double result);
                if ((double.Parse(splitData[3]) < 1000) && bl)
                {
                    SerialContent = "Close";

                    _dataModel.Humidity = double.Parse(splitData[0]);
                    _dataModel.Temperature = double.Parse(splitData[1]);
                    _dataModel.Pm1_0 = double.Parse(splitData[2]);
                    _dataModel.Pm2_5 = double.Parse(splitData[3]);
                    _dataModel.Pm10 = double.Parse(splitData[4]);
                    _dataModel.Pid = double.Parse(splitData[5]);
                    _dataModel.Mics = double.Parse(splitData[6]);
                    _dataModel.Cjmcu = double.Parse(splitData[7]);
                    _dataModel.Mq = double.Parse(splitData[8]);
                    _dataModel.Hcho = double.Parse(splitData[9]);

                    _displayDataViewModel.Humidity = _dataModel.Humidity;
                    _displayDataViewModel.Temperature = _dataModel.Temperature;
                    _displayDataViewModel.Pm1_0 = _dataModel.Pm1_0;
                    _displayDataViewModel.Pm2_5 = _dataModel.Pm2_5;
                    _displayDataViewModel.Pm10 = _dataModel.Pm10;
                    _displayDataViewModel.Pid = _dataModel.Pid;
                    _displayDataViewModel.Mics = _dataModel.Mics;
                    _displayDataViewModel.Cjmcu = _dataModel.Cjmcu;
                    _displayDataViewModel.Mq = _dataModel.Mq;
                    _displayDataViewModel.Hcho = _dataModel.Hcho;

                    _oxyPlotViewModel.GraphHumidity(_dataModel.Humidity);
                    _oxyPlotViewModel.GraphTemperature(_dataModel.Temperature);
                    _oxyPlotViewModel.GraphPm1_0(_dataModel.Pm1_0);
                    _oxyPlotViewModel.GraphPm2_5(_dataModel.Pm2_5);
                    _oxyPlotViewModel.GraphPm10(_dataModel.Pm10);
                    _oxyPlotViewModel.GraphPid(_dataModel.Pid);
                    _oxyPlotViewModel.GraphMics(_dataModel.Mics);
                    _oxyPlotViewModel.GraphCjmcu(_dataModel.Cjmcu);
                    _oxyPlotViewModel.GraphMq(_dataModel.Mq);
                    _oxyPlotViewModel.GraphHcho(_dataModel.Hcho);

                    _oxyPlotViewModel.UpdateCount();
                    _oxyPlotViewModel.UpdataGrpah(_oxyPlotViewModel.LiveState);

                    if (_databaseViewModel.MysqlState)
                    {
                        _databaseViewModel._database.AddData(
                            _timerViewModel.TimerContent,
                            _dataModel.Humidity,
                            _dataModel.Temperature,
                            _dataModel.Pm1_0,
                            _dataModel.Pm2_5,
                            _dataModel.Pm10,
                            _dataModel.Pid,
                            _dataModel.Mics,
                            _dataModel.Cjmcu,
                            _dataModel.Mq,
                            _dataModel.Hcho
                            );

                    }
                    if (_csvViewModel.CsvState)
                    {
                        _csvViewModel._csv.AddCsv(
                            _timerViewModel.TimerContent,
                            _dataModel.Humidity,
                            _dataModel.Temperature,
                            _dataModel.Pm1_0,
                            _dataModel.Pm2_5,
                            _dataModel.Pm10,
                            _dataModel.Pid,
                            _dataModel.Mics,
                            _dataModel.Cjmcu,
                            _dataModel.Mq,
                            _dataModel.Hcho
                            );
                    }

                }
                else
                {
                    SerialContent = "형식 오류";
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
