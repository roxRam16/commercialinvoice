namespace EmpresasApi.Models
{
    public class Settings : ISettings
    {
        public string Name { get; set; }

        public string Host { get; set; }

        public string Port { get; set; }
    }

    //Para inyectar esa configuracion debemos inyectalar a traves de un contrato
    public interface ISettings
    {
        string Name { get; set; }

        string Host { get; set; }

        string Port { get; set; }
    }

}
