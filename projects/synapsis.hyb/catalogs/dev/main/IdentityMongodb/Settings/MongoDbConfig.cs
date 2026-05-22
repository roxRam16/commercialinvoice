namespace IdentityMongodb.Settings
{
    public class MongoDbConfig : IMongoDbConfig
    {
        public string? DataBase {get; set; }

        public string? Host { get; set; }

        public string? Port { get; set; }
    }

    public interface IMongoDbConfig
    {
        string? DataBase { get; set; }

        string? Host { get; set; }

        string? Port { get; set; }

    }

}
