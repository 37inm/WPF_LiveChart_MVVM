using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF_LiveChart_MVVM.Service
{
    class CsvService
    {
        private string csvName;
        private string csvFilePath; // CSV 파일 경로
        private StreamWriter writer; // CSV 파일 작성자

        public bool CreateCsv()
        {
            string defaultCsvName = DateTime.Now.ToString("yyMMdd_HHmm");
            csvName = Interaction.InputBox("저장할 csv파일의 이름을 입력하세요:", "사용할 csv파일 이름 입력", defaultCsvName);
            if(csvName == "")
            {
                return false; 
            } else
            {
                string projectPath = Directory.GetCurrentDirectory();
                csvFilePath = System.IO.Path.Combine(projectPath, csvName + ".csv");
                writer = new StreamWriter(csvFilePath, true, Encoding.UTF8);
                string line = $"{"Time"},{"Temperature"},{"Humidity"}, {"PM1.0"}, {"PM2.5"}, {"PM10"}, {"PID"}, {"MICS"}, {"CJMCU"}, {"MQ"}, {"HCHO"}";
                writer.WriteLine(line);
                writer.Close();
                MessageBox.Show(csvName + ".csv 저장 완료");
                return true;
            }
        }

        public void AddCsv(string timer ,double humidity, double temperature, double pm1_0, double pm2_5, double pm10, double pid, double mics, double cjmcu, double mq, double hcho)
        {
            writer = new StreamWriter(csvFilePath, true, Encoding.UTF8);
            string line = $"{timer},{humidity},{temperature},{pm1_0},{pm2_5},{pm10}, {pid}, {mics}, {cjmcu}, {mq}, {hcho}";
            writer.WriteLine(line);
            writer.Close();
        }
    }
}
