namespace WPF_LiveChart_MVVM.Model
{
    class DatabaseModel
    {
        public string Server { get; set; }
        public string DatabaseServer { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string TableName { get; set; }

        public bool State { get; set; }
    }
}
