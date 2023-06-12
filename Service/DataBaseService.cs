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
                    "`Time` INT NULL, " +
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

        public void AddData()
        {
            string insertDataQuery = "INSERT INTO " + tableName + " (Time, Humidity, Temperature, PM1_0, PM2_5, PM10, PID, MiCS, CJMCU, MQ, HCHO) " +
                        "VALUES (@Time, @Humidity, @Temperature, @PM1_0, @PM2_5, @PM10, @PID, @MiCS, @CJMCU, @MQ, @HCHO);";

            MySqlCommand insertDataCommand = new MySqlCommand(insertDataQuery, connection);
            //insertDataCommand.Parameters.AddWithValue("@Time", seconds);
            //insertDataCommand.Parameters.AddWithValue("@Humidity", splitData[0]);
            //insertDataCommand.Parameters.AddWithValue("@Temperature", splitData[1]);
            //insertDataCommand.Parameters.AddWithValue("@PM1_0", splitData[2]);
            //insertDataCommand.Parameters.AddWithValue("@PM2_5", splitData[3]);
            //insertDataCommand.Parameters.AddWithValue("@PM10", splitData[4]);
            //insertDataCommand.Parameters.AddWithValue("@PID", splitData[5]);
            //insertDataCommand.Parameters.AddWithValue("@MiCS", splitData[6]);
            //insertDataCommand.Parameters.AddWithValue("@CJMCU", splitData[7]);
            //insertDataCommand.Parameters.AddWithValue("@MQ", splitData[8]);
            //insertDataCommand.Parameters.AddWithValue("@HCHO", splitData[9]);
            //insertDataCommand.ExecuteNonQuery();

            insertDataCommand.Parameters.AddWithValue("@Time", 1);
            insertDataCommand.Parameters.AddWithValue("@Humidity", 1);
            insertDataCommand.Parameters.AddWithValue("@Temperature", 1);
            insertDataCommand.Parameters.AddWithValue("@PM1_0", 1);
            insertDataCommand.Parameters.AddWithValue("@PM2_5", 1);
            insertDataCommand.Parameters.AddWithValue("@PM10", 1);
            insertDataCommand.Parameters.AddWithValue("@PID", 1);
            insertDataCommand.Parameters.AddWithValue("@MiCS", 1);
            insertDataCommand.Parameters.AddWithValue("@CJMCU", 1);
            insertDataCommand.Parameters.AddWithValue("@MQ", 1);
            insertDataCommand.Parameters.AddWithValue("@HCHO", 1);
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
