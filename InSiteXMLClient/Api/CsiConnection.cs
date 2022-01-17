using Camstar.Utility;
using Camstar.XMLClient.API.Utilities;
using Camstar.XMLClient.Interface;
using System.Collections;

namespace Camstar.XMLClient.API
{
    internal class CsiConnection : ICsiConnection
    {
        private ServerConnection mServerConnection;
        private Hashtable mSessions;
        private string mHost;
        private int mPort;
        private int mTimeout = 0;

        internal CsiConnection(string host, int port)
        {
            this.mServerConnection = new ServerConnection();
            this.mSessions = new Hashtable();
            this.mHost = host;
            this.mPort = port;
            this.mTimeout = 0;
        }

        public int SetConnectionTimeout(int timeout)
        {
            int mTimeout = this.mTimeout;
            this.mTimeout = timeout > 0 ? timeout : 0;
            this.mServerConnection.SendTimeout = this.mServerConnection.ReceiveTimeout = this.mTimeout;
            return mTimeout;
        }

        public ICsiSession CreateSessionWithSessionId(
          string userName,
          string sessionID,
          string sessionName)
        {
            lock (this)
            {
                if (this.FindSession(sessionName) != null)
                    throw new CsiClientException(3014680L, this.GetType().FullName + ".createSessionWithSessionID()");
                ICsiSession csiSession = (ICsiSession)new CsiSession(userName, string.Empty, (ICsiConnection)this);
                csiSession.SessionId = sessionID;
                csiSession.Host = this.mHost;
                this.mSessions[sessionName] = csiSession;
                return csiSession;
            }
        }

        public ICsiSession CreateSession(string userName, string password, string sessionName)
        {
            lock (this)
            {
                if (this.FindSession(sessionName) != null)
                    throw new CsiClientException(3014680L, this.GetType().FullName + ".createSession()");
                ICsiSession csiSession = (ICsiSession)new CsiSession(userName, password, (ICsiConnection)this);
                csiSession.Host = this.mHost;
                csiSession.Port = this.mPort;
                string license = CsiSessionManager.AddOrGetLicense(this.mHost, this.mPort, userName, password);
                csiSession.SessionId = !string.IsNullOrEmpty(license) ? license : throw new CsiClientException(3014683L, this.GetType().FullName + ".createSession()");
                this.mSessions[sessionName] = csiSession;
                return csiSession;
            }
        }

        public ICsiSession FindSession(string sessionName) => this.mSessions[sessionName] as ICsiSession;

        public void RemoveSession(string sessionName)
        {
            lock (this)
            {
                ICsiSession session = this.FindSession(sessionName);
                if (session != null)
                    CsiSessionManager.TryReleaseLicense(this.mHost, this.mPort, session.UserName, session.Password);
                this.mSessions.Remove(sessionName);
            }
        }

        protected internal virtual void ChangeExpiredSessionId(
          string expiredSessionId,
          string newSessionId)
        {
            if (this.mSessions == null)
                return;
            lock (this)
            {
                foreach (string key in (IEnumerable)this.mSessions.Keys)
                {
                    if (this.FindSession(key) is CsiSession session && string.Equals(session.SessionId, expiredSessionId))
                    {
                        session.SessionId = newSessionId;
                        session.ChangeAllDocumentsSessionId(newSessionId);
                    }
                }
            }
        }

        public string Submit(string requestXml) => this.mServerConnection.Submit(this.mHost, this.mPort, requestXml);
    }
}