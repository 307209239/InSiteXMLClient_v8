
namespace Camstar.XMLClient.Interface
{
  public interface ICsiConnection
  {
    ICsiSession CreateSession(string userName, string password, string sessionName);

    ICsiSession CreateSessionWithSessionId(
      string userName,
      string sessionID,
      string sessionName);

    ICsiSession FindSession(string sessionName);

    string Submit(string xml);

    void RemoveSession(string sessionName);

    int SetConnectionTimeout(int timeout);
  }
}
