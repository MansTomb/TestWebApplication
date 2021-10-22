namespace TestWebApplication.Settings
{
    public class MongoDbSettings
    {
        public string Host { get; set; }
        public string Port { get; set; }

        public string ConnectionSring
        {
            get
            {
                return $"mongodb://{Host}:{Port}";
            }
        }
    }
}