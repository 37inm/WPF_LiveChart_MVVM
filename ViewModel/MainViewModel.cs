using System.ComponentModel;
using WPF_LiveChart_MVVM.Model;

namespace WPF_LiveChart_MVVM.ViewModel
{
    class MainViewModel 
    {

        public SerialViewModel _serialViewModel { get; set; }
        public OxyPlotViewModel _oxyPlotViewModel {get; set;}

       

        public MainViewModel()
        {
            _oxyPlotViewModel = new OxyPlotViewModel();
            _serialViewModel = new SerialViewModel(_oxyPlotViewModel);
        }

        
    }
}
