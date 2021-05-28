using Camstar.Util;
using System;
using System.Collections.Concurrent;

namespace Camstar.XMLClient.API.Utilities
{
  internal class CsiSessionManager
  {
    private static readonly ConcurrentDictionary<string, CsiSessionManager.SessionIdPair> ConnectionsHashtable = new ConcurrentDictionary<string, CsiSessionManager.SessionIdPair>();
    private static readonly ConcurrentDictionary<string, object> Lockers = new ConcurrentDictionary<string, object>();

    public static string AddOrGetLicense(string host, int port, string userName, string password)
    {
      string str1 = CryptUtil.Encrypt(password);
      string key = CsiSessionManager.GetKey(host, port, userName, str1);
      if (!CsiSessionManager.Lockers.ContainsKey(key))
        CsiSessionManager.Lockers.TryAdd(key, new object());
      lock (CsiSessionManager.Lockers[key])
      {
        string str2 = string.Empty;
        if (!CsiSessionManager.ConnectionsHashtable.ContainsKey(key))
        {
          string sessionId=String.Empty;
          str2 = csiWCFUtilities.LogIn(userName,str1, out sessionId, host,port==443);
          if (string.IsNullOrEmpty(str2))
            CsiSessionManager.ConnectionsHashtable.TryAdd(key, new CsiSessionManager.SessionIdPair(sessionId)
            {
              LastUsageUtcTime = DateTime.UtcNow
            });
        }
        if (!string.IsNullOrEmpty(str2))
          return string.Empty;
        CsiSessionManager.SessionIdPair sessionIdPair = CsiSessionManager.ConnectionsHashtable[key];
        ++sessionIdPair.LicenseHolders;
        return sessionIdPair.SessionId;
      }
    }

    public static string CheckSessionKey(string host, int port, string userName, string password)
    {
      string sessionId = string.Empty;
      int idleSessionTimeout = CsiClient.CamstarServerIdleSessionTimeout;
      if (idleSessionTimeout > 0)
      {
        string str = CryptUtil.Encrypt(password);
        string key = CsiSessionManager.GetKey(host, port, userName, str);
        if (CsiSessionManager.Lockers.ContainsKey(key))
        {
          lock (CsiSessionManager.Lockers[key])
          {
            CsiSessionManager.SessionIdPair sessionIdPair;
            if (CsiSessionManager.ConnectionsHashtable.TryGetValue(key, out sessionIdPair))
            {
              if (sessionIdPair != null)
              {
                if (DateTime.UtcNow - sessionIdPair.LastUsageUtcTime >= new TimeSpan(0, idleSessionTimeout, 0))
                {
                  int licenseHolders = sessionIdPair.LicenseHolders;
                  csiWCFUtilities.Logout(sessionIdPair.SessionId, host,port==443);
                  CsiSessionManager.ConnectionsHashtable.TryRemove(key, out sessionIdPair);
                  if (string.IsNullOrEmpty(csiWCFUtilities.LogIn(userName, str, out sessionId, host,port==443)))
                    CsiSessionManager.ConnectionsHashtable.TryAdd(key, new CsiSessionManager.SessionIdPair(sessionId)
                    {
                      LastUsageUtcTime = DateTime.UtcNow,
                      LicenseHolders = licenseHolders
                    });
                }
                else
                  sessionIdPair.LastUsageUtcTime = DateTime.UtcNow;
              }
            }
          }
        }
      }
      return sessionId;
    }

    public static void TryReleaseLicense(string host, int port, string userName, string password)
    {
      string password1 = CryptUtil.Encrypt(password);
      string key = CsiSessionManager.GetKey(host, port, userName, password1);
      if (!CsiSessionManager.Lockers.ContainsKey(key))
        return;
      lock (CsiSessionManager.Lockers[key])
      {
        CsiSessionManager.SessionIdPair sessionIdPair;
        if (CsiSessionManager.ConnectionsHashtable.TryGetValue(key, out sessionIdPair) && sessionIdPair != null)
        {
          --sessionIdPair.LicenseHolders;
          if (sessionIdPair.LicenseHolders == 0)
          {
            csiWCFUtilities.Logout(sessionIdPair.SessionId, host,port==443);
            CsiSessionManager.ConnectionsHashtable.TryRemove(key, out sessionIdPair);
          }
          else if (sessionIdPair.LicenseHolders < 0)
            CsiSessionManager.ConnectionsHashtable.TryRemove(key, out sessionIdPair);
        }
      }
    }

    private static string GetKey(string host, int port, string userName, string password) => string.Join("_", (object) host, (object) port, (object) userName, (object) password);

    private class SessionIdPair
    {
      public SessionIdPair(string sessionId) => this.SessionId = sessionId;

      public string SessionId { get; set; }

      public int LicenseHolders { get; set; }

      public DateTime LastUsageUtcTime { get; set; }
    }
  }
}
