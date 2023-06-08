using OxyPlot;
using OxyPlot.Series;
using System.ComponentModel;

namespace WPF_LiveChart_MVVM.ViewModel
{
    class OxyPlotViewModel : INotifyPropertyChanged
    {
        private int _dataCount;

        
        public PlotModel _plotHumidityModel { get; set;}

        private LineSeries lineHumidty;

        public OxyPlotViewModel()
        {
            _plotHumidityModel = new PlotModel();
            lineHumidty = new LineSeries();
            _plotHumidityModel.Series.Add(lineHumidty);
        }

        public void GraphUpdate(double a)
        {
            double x = _dataCount;
            lineHumidty.Points.Add(new DataPoint(x, a));
            _plotHumidityModel.InvalidatePlot(true);
            _dataCount++;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
