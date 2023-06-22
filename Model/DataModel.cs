namespace WPF_LiveChart_MVVM.Model
{
    class DataModel
    {
        public int Time { get; set; }
        public double Humidity { get; set; }
        public double Temperature { get; set; }
        public double Pm1_0 { get; set; }
        public double Pm2_5 { get; set; }
        public double Pm10 { get; set; }
        public double Pid { get; set; }
        public double Mics { get; set; }

        public double Cjmcu { get; set; }
        public double Mq { get; set; }
        public double Hcho { get; set; }
    }
}
