using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using WPF_LiveChart_MVVM.Model;
using WPF_LiveChart_MVVM.ViewModel.Command;

namespace WPF_LiveChart_MVVM.ViewModel
{
    class UdpViewModel : INotifyPropertyChanged
    {
        UdpClient _udpClient;
        private OxyPlotViewModel _oxyPlotViewModel;
        private ToggleViewModel _toggleViewModel;
        private DataBaseViewModel _databaseViewModel;
        private CsvViewModel _csvViewMdoel;
        private TimerViewModel _timerViewModel;
        private DisplayDataViewModel _displayDataViewModel;
        private DataModel _dataModel;

        private string _ip;
        public string Ip
        {
            get { return _ip; }
            set
            {
                _ip = value;
                OnPropertyChanged(nameof(Ip));
            }
        }

        private string _port;
        public string Port
        {
            get { return _port; }
            set
            {
                _port = value;
                OnPropertyChanged(nameof(Port));
            }
        }

        private string _udpContent;
        public string UdpContent
        {
            get { return _udpContent; }
            set
            {
                _udpContent = value;
                OnPropertyChanged(nameof(UdpContent));
            }
        }
        
        private bool _udpState;
        public bool UdpState
        {
            get { return _udpState; }
            set
            {
                _udpState = value;
                OnPropertyChanged(nameof(UdpState));
            }
        }


        private RelayCommand _udpCommand;
        public RelayCommand UdpCommand
        {
            get { return _udpCommand; }
            set
            {
                _udpCommand = value;
                OnPropertyChanged(nameof(UdpCommand));
            }
        }

        public UdpViewModel(OxyPlotViewModel oxyPlotView, ToggleViewModel toggleViewModel, DataBaseViewModel dataBaseViewModel, CsvViewModel csvViewModel, TimerViewModel timerViewModel, DisplayDataViewModel displayDataViewModel)
        {
            UdpCommand = new RelayCommand(OpenUdp);

            _oxyPlotViewModel = oxyPlotView;
            _toggleViewModel = toggleViewModel;
            _databaseViewModel = dataBaseViewModel;
            _csvViewMdoel = csvViewModel;
            _timerViewModel = timerViewModel;
            _displayDataViewModel = displayDataViewModel;

            _dataModel = new DataModel();

            UdpState = true;
            UdpContent = "Open";
            Ip = "192.168.0.2";
            Port = "4210";
        }


        public void OpenUdp()
        {
            if (_databaseViewModel.AvailableMysql && !_databaseViewModel.MysqlState)
            {
                MessageBox.Show("데이터베이스를 연결해주세요!");
                return;
            } else
            {
                try
                {
                    _udpClient = new UdpClient(int.Parse(Port));
                    byte[] data = Encoding.UTF8.GetBytes("abc");

                    _udpClient.Send(data, data.Length, Ip, int.Parse(Port));
                    UdpState = false;
                    
                    _udpClient.BeginReceive(ReceiveCallback, null);

                    MessageBox.Show(Ip + ", " + Port + " Connect !");
                    _timerViewModel.Start();

                    UdpContent = "Close";
                    UdpCommand = new RelayCommand(CloseUdp);

                    _toggleViewModel.MainToggle = false;
                    _toggleViewModel.SubToggle = false;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            
        }

        public void CloseUdp()
        {
            UdpState = true;
            _udpClient.Close();
            _timerViewModel.Stop();
            
            if (_databaseViewModel.MysqlState)
            {
                _databaseViewModel.CloseDatabase();
            }

            UdpCommand = new RelayCommand(OpenUdp);
            UdpContent = "Open";

            _toggleViewModel.MainToggle = true;
            _toggleViewModel.SubToggle = true;
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            if (!UdpState)
            {
                try
                {
                    IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] receivedBytes = _udpClient.EndReceive(ar, ref ipEndPoint);
                    string ReceivedData = Encoding.UTF8.GetString(receivedBytes); // 바이트 배열을 문자열로 변환
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
                        if (_csvViewMdoel.CsvState)
                        {
                            _csvViewMdoel._csv.AddCsv(
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
                    
                    _udpClient.BeginReceive(ReceiveCallback, null); // 계속해서 데이터 수신 대기
               
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    
        

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
