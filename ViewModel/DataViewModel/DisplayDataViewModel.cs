

using System.ComponentModel;

namespace WPF_LiveChart_MVVM.ViewModel
{
    class DisplayDataViewModel : INotifyPropertyChanged
    {
        private double _humidity;
        public double Humidity
        {
            get { return _humidity; }
            set
            {
                _humidity = value;
                OnPropertyChanged(nameof(Humidity));
            }
        }
        private double _temperature; 
        public double Temperature
        {
            get { return _temperature; }
            set
            {
                _temperature = value;
                OnPropertyChanged(nameof(Temperature));
            }
        }
        private double _pm1_0;
        public double Pm1_0
        {
            get { return _pm1_0; }
            set
            {
                _pm1_0 = value;
                OnPropertyChanged(nameof(Pm1_0));
            }
        }
        private double _pm2_5;
        public double Pm2_5
        {
            get { return _pm2_5; }
            set
            {
                _pm2_5 = value;
                OnPropertyChanged(nameof(Pm2_5));
            }
        }
        private double _pm10;
        public double Pm10
        {
            get { return _pm10; }
            set
            {
                _pm10 = value;
                OnPropertyChanged(nameof(Pm10));
            }
        }
        private double _pid;
        public double Pid
        {
            get { return _pid; }
            set
            {
                _pid = value;
                OnPropertyChanged(nameof(Pid));
            }
        }
        private double _mics;
        public double Mics
        {
            get { return _mics; }
            set
            {
                _mics = value;
                OnPropertyChanged(nameof(Mics));
            }
        }
        private double _cjmcu;
        public double Cjmcu
        {
            get { return _cjmcu; }
            set
            {
                _cjmcu = value;
                OnPropertyChanged(nameof(Cjmcu));
            }
        }
        private double _mq;
        public double Mq
        {
            get { return _mq; }
            set
            {
                _mq = value;
                OnPropertyChanged(nameof(Mq));
            }
        }
        private double _hcho;
        public double Hcho
        {
            get { return _hcho; }
            set
            {
                _hcho = value;
                OnPropertyChanged(nameof(Hcho));
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
