namespace Camstar.XMLClient.Interface
{
    public interface ICsiSession
    {
        ICsiDocument CreateDocument(string name);

        ICsiDocument FindDocument(string name);

        void RemoveDocument(string name);

        string SessionId { get; set; }

        string UserName { get; }

        string Password { get; }

        string Host { get; set; }

        int Port { get; set; }
    }
}