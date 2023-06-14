using OxyPlot;
using OxyPlot.Series;
using System.ComponentModel;

namespace WPF_LiveChart_MVVM.ViewModel
{
    class OxyPlotViewModel
    {
        private int _dataCount;
        
        public PlotModel _plotHumidityModel { get; set;}
        public PlotModel _plotTemperatureModel { get; set; }
        public PlotModel _plotPm1_0Model { get; set; }
        public PlotModel _plotPm2_5Model { get; set; }
        public PlotModel _plotPm10Model { get; set; }
        public PlotModel _plotPidModel { get; set; }
        public PlotModel _plotMicsModel { get; set; }
        public PlotModel _plotCjmcuModel { get; set; }
        public PlotModel _plotMqModel { get; set; }
        public PlotModel _plotHchoModel { get; set; }

        private LineSeries lineHumidty;
        private LineSeries lineTemperature;
        private LineSeries linePm1_0;
        private LineSeries linePm2_5;
        private LineSeries linePm10;
        private LineSeries linePid;
        private LineSeries lineMics;
        private LineSeries lineCjmcu;
        private LineSeries lineMq;
        private LineSeries lineHcho;

        public OxyPlotViewModel()
        {
            _plotHumidityModel = new PlotModel();
            lineHumidty = new LineSeries();

            _plotTemperatureModel = new PlotModel();
            lineTemperature = new LineSeries();

            _plotPm1_0Model = new PlotModel();
            linePm1_0 = new LineSeries();

            _plotPm2_5Model = new PlotModel();
            linePm2_5 = new LineSeries();

            _plotPm10Model = new PlotModel();
            linePm10 = new LineSeries();

            _plotPidModel = new PlotModel();
            linePid = new LineSeries();

            _plotMicsModel = new PlotModel();
            lineMics = new LineSeries();

            _plotCjmcuModel = new PlotModel();
            lineCjmcu = new LineSeries();

            _plotMqModel = new PlotModel();
            lineMq = new LineSeries();

            _plotHchoModel = new PlotModel();
            lineHcho = new LineSeries();

            _plotHumidityModel.Series.Add(lineHumidty);
            _plotTemperatureModel.Series.Add(lineTemperature);
            _plotPm1_0Model.Series.Add(linePm1_0);
            _plotPm2_5Model.Series.Add(linePm2_5);
            _plotPm10Model.Series.Add(linePm10);
            _plotPidModel.Series.Add(linePid);
            _plotMicsModel.Series.Add(lineMics);
            _plotCjmcuModel.Series.Add(lineCjmcu);
            _plotMqModel.Series.Add(lineMq);
            _plotHchoModel.Series.Add(lineHcho);

        }

        public void GraphHumidity(double value)
        {
            double x = _dataCount;
            lineHumidty.Points.Add(new DataPoint(x, value));
        }

        public void GraphTemperature(double value)
        {
            double x = _dataCount;
            lineTemperature.Points.Add(new DataPoint(x, value));
        }
        public void GraphPm1_0(double value)
        {
            double x = _dataCount;
            linePm1_0.Points.Add(new DataPoint(x, value));
        }
        public void GraphPm2_5(double value)
        {
            double x = _dataCount;
            linePm2_5.Points.Add(new DataPoint(x, value));
        }
        public void GraphPm10(double value)
        {
            double x = _dataCount;
            linePm10.Points.Add(new DataPoint(x, value));
        }
        public void GraphPid(double value)
        {
            double x = _dataCount;
            linePid.Points.Add(new DataPoint(x, value));
        }
        public void GraphMics(double value)
        {
            double x = _dataCount;
            lineMics.Points.Add(new DataPoint(x, value));
        }

        public void GraphCjmcu(double value)
        {
            double x = _dataCount;
            lineCjmcu.Points.Add(new DataPoint(x, value));
        }

        public void GraphMq(double value)
        {
            double x = _dataCount;
            lineMq.Points.Add(new DataPoint(x, value));
        }

        public void GraphHcho(double value)
        {
            double x = _dataCount;
            lineHcho.Points.Add(new DataPoint(x, value));
        }

        public void UpdateCount()
        {
            _dataCount++;
        }

        public void UpdataGrpah(bool state)
        {
            _plotHumidityModel.InvalidatePlot(state);
            _plotTemperatureModel.InvalidatePlot(state);
            _plotPm1_0Model.InvalidatePlot(state);
            _plotPm2_5Model.InvalidatePlot(state);
            _plotPm10Model.InvalidatePlot(state);
            _plotPidModel.InvalidatePlot(state);
            _plotMicsModel.InvalidatePlot(state);
            _plotCjmcuModel.InvalidatePlot(state);
            _plotMqModel.InvalidatePlot(state);
            _plotHchoModel.InvalidatePlot(state);
        }

        public void ClearGraph()
        {
            _dataCount = 0;
            lineHumidty.Points.Clear();
            lineTemperature.Points.Clear();
            linePm1_0.Points.Clear();
            linePm2_5.Points.Clear();
            linePm10.Points.Clear();
            linePid.Points.Clear();
            lineMics.Points.Clear();
            lineCjmcu.Points.Clear();
            lineMq.Points.Clear();
            lineHcho.Points.Clear();
            UpdataGrpah(true);
        }

    }
}
