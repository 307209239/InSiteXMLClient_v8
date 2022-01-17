using Camstar.XMLClient.Interface;
using System.Collections;

namespace Camstar.XMLClient.API
{
    public sealed class CsiClient
    {
        public static int CamstarServerIdleSessionTimeout = 30;
        private Hashtable mConnections;

        public CsiClient() => this.mConnections = new Hashtable();

        public ICsiConnection CreateConnection(string host, int port)
        {
            lock (this)
            {
                ICsiConnection csiConnection = this.FindConnection(host, port) == null ? (ICsiConnection)new CsiConnection(host, port) : throw new CsiClientException(3014681L, this.GetType().FullName + ".createConnection()");
                this.mConnections[(host + "_" + port)] = csiConnection;
                return csiConnection;
            }
        }

        public ICsiConnection FindConnection(string host, int port) => this.mConnections[(host + "_" + port)] as ICsiConnection;

        public void RemoveConnection(string host, int port)
        {
            lock (this)
                this.mConnections.Remove((host + "_" + port));
        }
    }
}