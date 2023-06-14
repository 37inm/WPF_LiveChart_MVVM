using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_LiveChart_MVVM.Service;
using WPF_LiveChart_MVVM.ViewModel.Command;

namespace WPF_LiveChart_MVVM.ViewModel
{
    class DataBaseViewModel : INotifyPropertyChanged
    {
        public DataBaseService _database { get; set; }

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

        private bool _mysqlToggle;
        public bool MysqlToggle
        {
            get { return _mysqlToggle; }
            set
            {
                _mysqlToggle = value;
                OnPropertyChanged(nameof(MysqlToggle));
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


        public DataBaseViewModel()
        {
            AvailableMysql = false;
            MysqlState = false;
            MysqlToggle = true;
            MysqlContent = "Connect";
            AvailableMysqlCommand = new RelayCommand(ToggleMysql);
            MysqlCommand = new RelayCommand(OpenDatabase);
        }



        public void ToggleMysql()
        {
            AvailableMysql = !AvailableMysql;
        }

        public void OpenDatabase()
        {
            _database = new DataBaseService();
            MysqlState = _database.OpenDatabase("root", "8546elefjq", "127.0.0.1", "Hello");
            MysqlToggle = !MysqlState;
            if(MysqlState)
            {
                MysqlCommand = new RelayCommand(CloseDatabase);
                MysqlContent = "Close";
            }
        }

        public void CloseDatabase()
        {
            MysqlState = _database.CloseDatabase();
            MysqlToggle = !MysqlState;
            if (!MysqlState)
            {
                MysqlCommand = new RelayCommand(OpenDatabase);
                MysqlContent = "Connect";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
