using System.ComponentModel;
using System.Windows;
using WPF_LiveChart_MVVM.Model;
using WPF_LiveChart_MVVM.Service;
using WPF_LiveChart_MVVM.View;
using WPF_LiveChart_MVVM.ViewModel.Command;
using WPF_LiveChart_MVVM.ViewModel.PopViewModel;

namespace WPF_LiveChart_MVVM.ViewModel
{
    class DataBaseViewModel : INotifyPropertyChanged
    {
        DatabasePopView _databasePopView;
        DatabasePopViewModel _databasePopViewModel;
        DatabaseModel _databaseModel;
        public DataBaseService _database { get; set; }


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

        public DataBaseViewModel(DatabaseModel databaseModel)
        {
            MysqlState = false;
            MysqlCommand = new RelayCommand(ToggleMysql);
            _databaseModel = databaseModel;
        }

        public void ToggleMysql()
        {
            _databasePopView = new DatabasePopView();
            _databasePopView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _databasePopViewModel = new DatabasePopViewModel(_databaseModel, this);
            _databasePopView.DataContext = _databasePopViewModel;
            _databaseModel.State = false;
            _databasePopView.ShowDialog();
            if (_databaseModel.State == false) return;
            OpenDatabase();
        }

        public void Close()
        {
            _databasePopView.Close();
        }

        public void OpenDatabase()
        {
            _database = new DataBaseService();
            MysqlState = _database.OpenDatabase(_databaseModel.UserName, _databaseModel.Password, _databaseModel.Server, _databaseModel.DatabaseServer, _databaseModel.TableName);
            if(MysqlState)
            {
                MysqlCommand = new RelayCommand(CloseDatabase);
            }
        }

        public void CloseDatabase()
        {
            MysqlState = _database.CloseDatabase();
            if (!MysqlState)
            {
                MysqlCommand = new RelayCommand(ToggleMysql);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
