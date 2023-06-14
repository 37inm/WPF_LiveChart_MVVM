using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Windows;
using WPF_LiveChart_MVVM.Model;
using WPF_LiveChart_MVVM.Service;
using WPF_LiveChart_MVVM.ViewModel.Command;

namespace WPF_LiveChart_MVVM.ViewModel
{
    class SerialViewModel : INotifyPropertyChanged
    {
        private SerialPort _serialCommunication;
        private OxyPlotViewModel _oxyPlotViewModel;
        private DataModel _dataModel;
        private DataBaseService _database;

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

        private string _mysqlContent;
        public string MysqlContent
        {
            get { return _mysqlContent; }
            set
            {
                _mysqlContent = value;
                OnPropertyChanged(nameof(MysqlContent));
            }
        }

        private string _liveContent;
        public string LiveContent
        {
            get { return _liveContent; }
            set
            {
                _liveContent = value;
                OnPropertyChanged(nameof(LiveContent));
            }
        }


        private bool _availableMysql;
        public bool AvailableMysql
        {
            get { return _availableMysql; }
            set
            {
                _availableMysql = value;
                OnPropertyChanged(nameof(AvailableMysql));
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

        private bool _mysqlState;
        public bool MysqlState
        {
            get { return _mysqlState; }
            set
            {
                _mysqlState = value;
                OnPropertyChanged(nameof(MysqlState));
            }
        }

        private bool _toggleState;
        public bool ToggleState
        {
            get { return _toggleState; }
            set
            {
                _toggleState = value;
                OnPropertyChanged(nameof(ToggleState));
            }
        }

        private bool _liveState;
        public bool LiveState
        {
            get { return _liveState; }
            set
            {
                _liveState = value;
                OnPropertyChanged(nameof(LiveState));
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

        private RelayCommand _availableMysqlCommand;
        public RelayCommand AvailableMysqlCommand
        {
            get { return _availableMysqlCommand; }
            set
            {
                _availableMysqlCommand = value;
                OnPropertyChanged(nameof(AvailableMysqlCommand));
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

        private RelayCommand _stopCommand;
        public RelayCommand StopCommand
        {
            get { return _stopCommand; }
            set
            {
                _stopCommand = value;
                OnPropertyChanged(nameof(StopCommand));
            }
        }

        private RelayCommand _claerCommand;
        public RelayCommand ClearCommand
        {
            get { return _claerCommand; }
            set
            {
                _claerCommand = value;
                OnPropertyChanged(nameof(ClearCommand));
            }
        }



        public SerialViewModel(OxyPlotViewModel oxyPlotView)
        {
            LoadSerialPorts();
            SerialBaudRate = new ObservableCollection<int> { 9600, 14400, 19200, 38400, 57600, 115200 };
            _database = new DataBaseService();

            SerialCommand = new RelayCommand(OpenSerial);
            MysqlCommand = new RelayCommand(OpenDatabase);
            AvailableMysqlCommand = new RelayCommand(ToggleMysql);
            StopCommand = new RelayCommand(ToggleLive);
            ClearCommand = new RelayCommand(GrahpClear);

            _oxyPlotViewModel = oxyPlotView;
            _dataModel = new DataModel();


            SerialContent = "Open";
            MysqlContent = "Conenct";
            LiveContent = "Stop";
            SerialState = true;
            MysqlState = false;
            AvailableMysql = false;
            ToggleState = true;
            LiveState = true;
        }



        private void OpenSerial()
        {
            if (AvailableMysql && !MysqlState)
            {
                MessageBox.Show("데이터베이스를 연결해주세요!");
                return;
            }
            else
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
                        SerialCommand = new RelayCommand(CloseSerial);
                        SerialContent = "Close";
                        SerialState = false;
                        ToggleState = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }




        }

        private void CloseSerial()
        {
            try
            {
                _serialCommunication.DataReceived -= SerialPort_DataReceived;
                _serialCommunication.DiscardInBuffer();
                _serialCommunication.Close();
                if (MysqlState)
                {
                    CloseDatabase();
                }

                if (!_serialCommunication.IsOpen)
                {
                    SerialCommand = new RelayCommand(OpenSerial);
                    SerialContent = "Open";
                    SerialState = true;
                    ToggleState = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void OpenDatabase()
        {
            MysqlState = _database.OpenDatabase("root", "8546elefjq", "127.0.0.1", "Hello");
            ToggleState = !MysqlState;
            if (MysqlState)
            {
                MysqlCommand = new RelayCommand(CloseDatabase);
                MysqlContent = "Close";
            }
        }

        private void CloseDatabase()
        {
            MysqlState = _database.CloseDatabase();
            ToggleState = !MysqlState;
            if (!MysqlState)
            {
                MysqlCommand = new RelayCommand(OpenDatabase);
                MysqlContent = "Connect";
            }
        }

        public void ToggleMysql()
        {
            AvailableMysql = !AvailableMysql;
        }
        private void ToggleLive()
        {
            if (LiveState)
            {
                LiveState = !LiveState;
                LiveContent = "Stop";
            }
            else
            {
                LiveState = !LiveState;
                LiveContent = "Live";
            }
        }
        private void GrahpClear()
        {
            _oxyPlotViewModel.ClearGraph();
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
                    _oxyPlotViewModel.UpdataGrpah(LiveState);

                    if (MysqlState)
                    {
                        _database.AddData(
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



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
