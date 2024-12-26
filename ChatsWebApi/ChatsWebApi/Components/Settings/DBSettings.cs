namespace ChatsWebApi.Components.Settings
{
    public class DBSettings
    {
        public string ConnectionString { get; private set; }

        public DBSettings(string connStr)
        {
            ConnectionString = connStr;
        }
    }
}
