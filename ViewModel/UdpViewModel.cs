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

        public UdpViewModel(OxyPlotViewModel oxyPlotView, ToggleViewModel toggleViewModel, DataBaseViewModel dataBaseViewModel, CsvViewModel csvViewModel, TimerViewModel timerViewModel)
        {
            _oxyPlotViewModel = oxyPlotView;
            _toggleViewModel = toggleViewModel;
            _databaseViewModel = dataBaseViewModel;
            _csvViewMdoel = csvViewModel;
            _timerViewModel = timerViewModel;

            UdpCommand = new RelayCommand(OpenUdp);

            _dataModel = new DataModel();

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
                    UdpState = true;
                    _udpClient.BeginReceive(ReceiveCallback, null);

                    MessageBox.Show(Ip + ", " + Port + " Connect !");
                    _timerViewModel.Start();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            
        }

        public void CloseUdp()
        {
            UdpState = false;
            _timerViewModel.Stop();
            if (_databaseViewModel.MysqlState)
            {
                _databaseViewModel.CloseDatabase();
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            if (UdpState)
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
