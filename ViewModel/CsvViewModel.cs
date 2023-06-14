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
    class CsvViewModel : INotifyPropertyChanged
    {
        public CsvService _csv { get; set; }

        private bool _availableCsv;
        public bool AvailableCsv
        {
            get { return _availableCsv; }
            set
            {
                _availableCsv = value;
                OnPropertyChanged(nameof(AvailableCsv));
            }
        }

        private bool _csvState;
        public bool CsvState
        {
            get { return _csvState; }
            set
            {
                _csvState = value;
                OnPropertyChanged(nameof(CsvState));
            }
        }


        private RelayCommand _csvCommand;
        public RelayCommand CsvCommand
        {
            get { return _csvCommand; }
            set
            {
                _csvCommand = value;
                OnPropertyChanged(nameof(CsvCommand));
            }
        }

        public CsvViewModel()
        {
            CsvCommand = new RelayCommand(OpenCsv);
            CsvState = false;
        }

        private void OpenCsv()
        {
            if (!CsvState)
            {
                _csv = new CsvService();
                CsvState = _csv.CreateCsv();
            }
            else
            {
                CsvState = false;
                _csv = null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
