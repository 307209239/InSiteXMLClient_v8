using System;
using System.Collections.Concurrent;

namespace Camstar.Utility
{
    public class ServerConnectionPool
    {
        private readonly ConcurrentStack<ServerConnection> _ConnCache = new ConcurrentStack<ServerConnection>();

        public ServerConnectionPool() => ServerConnectionSettings.ConfigFileChanged += new EventHandler(this.OnConfigFileChanged);

        private void OnConfigFileChanged(object sender, EventArgs e) => this._ConnCache.Clear();

        public ServerConnection GetServerConnection(out bool fromCache)
        {
            ServerConnection result = (ServerConnection)null;
            fromCache = this._ConnCache.TryPop(out result);
            if (!fromCache)
                result = new ServerConnection();
            return result;
        }

        public void ReleaseServerConnection(ServerConnection conn)
        {
            if (ServerConnectionSettings.CurrentChangeNumber != conn.Settings.ChangeNumber)
                return;
            this._ConnCache.Push(conn);
        }
    }
}