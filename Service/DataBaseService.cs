using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using System;
using System.Windows;

namespace WPF_LiveChart_MVVM.Service
{
    class DataBaseService
    {
        private MySqlConnection connection;
        private string tableName;
        public bool OpenDatabase(string _userName, string _pw, string _server, string _database)
        {
            try
            {
                string uid = _userName;
                string password = _pw;
                string server = _server;
                string database = _database;
                string defaultTableName = DateTime.Now.ToString("yyMMdd_HHmm");
                tableName = Interaction.InputBox("저장할 테이블의 이름을 입력하세요:", "사용할 테이블 이름 입력", defaultTableName);
                string connectionString = $"server={server};database={database};uid={uid};password={password};";
                connection = new MySqlConnection(connectionString);
                string createTableQuery = "CREATE TABLE IF NOT EXISTS `" + tableName + "` (`Pk` INT NOT NULL AUTO_INCREMENT, " +
                    "`Time` VARCHAR(45) NULL, " +
                    "`Humidity` VARCHAR(45) NULL, " +
                    "`Temperature` VARCHAR(45) NULL, " +
                    "`PM1_0` VARCHAR(45) NULL, " +
                    "`PM2_5` VARCHAR(45) NULL, " +
                    "`PM10` VARCHAR(45) NULL, " +
                    "`PID` VARCHAR(45) NULL, " +
                    "`MiCS` VARCHAR(45) NULL, " +
                    "`CJMCU` VARCHAR(45) NULL, " +
                    "`MQ` VARCHAR(45) NULL, " +
                    "`HCHO` VARCHAR(45) NULL, " +
                    "PRIMARY KEY (`Pk`));";

                connection.Open();
                MySqlCommand createTableCommand = new MySqlCommand(createTableQuery, connection);
                createTableCommand.ExecuteNonQuery();

                MessageBox.Show(tableName + " Connect !");
                return true;

            } catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }

        public void AddData(string timer, double humidity, double temperature, double pm1_0, double pm2_5, double pm10, double pid, double mics, double cjmcu, double mq, double hcho)
        {
            string insertDataQuery = "INSERT INTO " + tableName + " (Time, Humidity, Temperature, PM1_0, PM2_5, PM10, PID, MiCS, CJMCU, MQ, HCHO) " +
                        "VALUES (@Time, @Humidity, @Temperature, @PM1_0, @PM2_5, @PM10, @PID, @MiCS, @CJMCU, @MQ, @HCHO);";

            MySqlCommand insertDataCommand = new MySqlCommand(insertDataQuery, connection);

            insertDataCommand.Parameters.AddWithValue("@Time", timer);
            insertDataCommand.Parameters.AddWithValue("@Humidity", humidity);
            insertDataCommand.Parameters.AddWithValue("@Temperature", temperature);
            insertDataCommand.Parameters.AddWithValue("@PM1_0", pm1_0);
            insertDataCommand.Parameters.AddWithValue("@PM2_5", pm2_5);
            insertDataCommand.Parameters.AddWithValue("@PM10", pm10);
            insertDataCommand.Parameters.AddWithValue("@PID", pid);
            insertDataCommand.Parameters.AddWithValue("@MiCS", mics);
            insertDataCommand.Parameters.AddWithValue("@CJMCU", cjmcu);
            insertDataCommand.Parameters.AddWithValue("@MQ", mq);
            insertDataCommand.Parameters.AddWithValue("@HCHO", hcho);
            insertDataCommand.ExecuteNonQuery();
        }


        public bool CloseDatabase()
        {
            try
            {
                connection.Close();
                MessageBox.Show(tableName + " Disconnect !");
                return false;
            } catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return true;
            }
            
        }
    }
}
