namespace CompaniesApi.Settings
{
    //public class MongoDbConfig
    //{
    //    public string Name { get; init; }
    //    public string Host { get; init; }
    //    public int Port { get; init; }
    //    //Si es con otra forma de generar la conexion a mongo
    //    //public string ConnectionString => $"mongodb://{Host}:{Port}";
    //    //SI ES CON CONEXION CON MONGODB CLUD
    //    public string ConnectionString => $"{Host}";
    //}

    public class MongoDbConfig : IMongoDbConfig
    {
        public string DataBase { get; set; }

        public string Host { get; set; }

        public string Port { get; set; }
    }

    //Para inyectar esa configuracion debemos inyectalar a traves de un contrato
    public interface IMongoDbConfig
    {
        string DataBase { get; set; }

        string Host { get; set; }

        string Port { get; set; }
    }
}
