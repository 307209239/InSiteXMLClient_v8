using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Camstar.Utility
{
    public class ServerConnectionSettings
    {
        private string _Host = "localhost";
        private int _Port = 443;
        private ApplicationProtocol _applicationProtocol = ApplicationProtocol.Auto;
        private string _appServerUriStem = "/camstarappserver/api/";
        private int _SendTimeout = 90000;
        private int _ReceiveTimeout = 90000;
        private string _XMLLogPath = "C:\\Temp";
        private bool _LogXml = false;
        private bool _fIgnoreInvalidCert = false;
        private int _HttpTimeout = 180000;
        private int _HttpReadWriteTimeout = 300000;
        private int _changeNumber = ServerConnectionSettings.s_ChangeNumber;
        private static ServerConnectionSettings s_DefaultSettings = new ServerConnectionSettings();
        private static string s_DefaultConfigurationFile = null;

        private static int s_ChangeNumber = 0;

        public ServerConnectionSettings Clone() => (ServerConnectionSettings)this.MemberwiseClone();

        public string Host
        {
            get => this._Host;
            set
            {
                if (string.IsNullOrEmpty(value))
                    return;
                this._Host = value;
            }
        }

        public int Port
        {
            get => this._Port;
            set
            {
                if (0 > value || value > (int)ushort.MaxValue)
                    return;
                this._Port = value;
            }
        }

        public ApplicationProtocol Protocol
        {
            get => this._applicationProtocol;
            set => this._applicationProtocol = value;
        }

        public string AppServerUriStem
        {
            get => this._appServerUriStem;
            set => this._appServerUriStem = value;
        }

        public int HttpTimeout
        {
            get => this._HttpTimeout;
            set => this._HttpTimeout = value;
        }

        public int HttpReadWriteTimeout
        {
            get => this._HttpReadWriteTimeout;
            set => this._HttpReadWriteTimeout = value;
        }

        public int SendTimeout
        {
            get => this._SendTimeout;
            set => this._SendTimeout = value;
        }

        public int ReceiveTimeout
        {
            get => this._ReceiveTimeout;
            set => this._ReceiveTimeout = value;
        }

        public string XMLLogPath
        {
            get => this._XMLLogPath;
            set
            {
                if (string.IsNullOrEmpty(value))
                    return;
                try
                {
                    if (!Directory.Exists(value))
                        Directory.CreateDirectory(value);
                    this._XMLLogPath = value;
                }
                catch (Exception ex)
                {
                    string message = string.Format("Error checking log directory\r\n{0}", ex.ToString());
                    Trace.TraceWarning(message);
                }
            }
        }

        public bool LogXml
        {
            get => this._LogXml;
            set => this._LogXml = value;
        }

        public bool IgnoreInvalidCert
        {
            get => this._fIgnoreInvalidCert;
            set => this._fIgnoreInvalidCert = value;
        }

        public int ChangeNumber
        {
            get => this._changeNumber;
            protected set => this._changeNumber = value;
        }

        private static bool LoadConfigSettings(string settingsFile)
        {
            return false;
            //if (string.IsNullOrWhiteSpace(settingsFile) || !File.Exists(settingsFile))
            //    return false;
            //XmlDocument xmlDocument = (XmlDocument)null;
            //FileStream fileStream = (FileStream)null;
            //bool flag1;
            //try
            //{
            //    bool flag2 = false;
            //    int num = 0;
            //    string message1 = string.Empty;
            //    do
            //    {
            //        try
            //        {
            //            xmlDocument = new XmlDocument();
            //            fileStream = new FileStream(settingsFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
            //            xmlDocument.Load((Stream)fileStream);
            //        }
            //        catch (IOException ex)
            //        {
            //            xmlDocument = (XmlDocument)null;
            //            message1 = string.Format("Error reading cconfiguration file\r\n{0}\r\n{1}", ex.HResult, ex.ToString());
            //            Trace.TraceWarning(message1);
            //            if (-2147024864 == ex.HResult)
            //                flag2 = true;
            //        }
            //        catch (Exception ex)
            //        {
            //            xmlDocument = (XmlDocument)null;
            //            message1 = string.Format("Error reading cconfiguration file\r\n{0}", ex.ToString());
            //            Trace.TraceWarning(message1);
            //        }
            //        finally
            //        {
            //            if (fileStream != null)
            //            {
            //                fileStream.Dispose();
            //                fileStream = (FileStream)null;
            //            }
            //        }
            //    }
            //    while (flag2 && 10 > num++);
            //    if (xmlDocument == null)
            //    {
            //        if (ServerConnectionSettings.s_EventLog != null)
            //            ServerConnectionSettings.s_EventLog.WriteEntry(string.IsNullOrEmpty(message1) ? "Unknown error reading config file" : message1, EventLogEntryType.Warning);
            //        return false;
            //    }
            //    ServerConnectionSettings defaultSettings = ServerConnectionSettings.DefaultSettings;
            //    XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("ServerConnection");
            //    if (elementsByTagName != null && elementsByTagName.Count > 0)
            //    {
            //        foreach (XmlNode xmlNode in elementsByTagName[0])
            //        {
            //            if (!string.IsNullOrWhiteSpace(xmlNode.InnerText))
            //            {
            //                string str = xmlNode.InnerText.Trim();
            //                string name = xmlNode.Name;
            //                // ISSUE: reference to a compiler-generated method
            //                switch ()
            //  {
            //    case 102951349:
            //      if (name == "ApplicationProtocol")
            //                    {
            //                        int result = 0;
            //                        if (int.TryParse(str, out result))
            //                        {
            //                            defaultSettings._applicationProtocol = (ApplicationProtocol)result;
            //                            break;
            //                        }
            //                        break;
            //                    }
            //                    continue;
            //    case 269898621:
            //      if (name == "IgnoreInvalidCerts")
            //                    {
            //                        bool result = false;
            //                        if (bool.TryParse(str, out result))
            //                        {
            //                            defaultSettings._fIgnoreInvalidCert = result;
            //                            break;
            //                        }
            //                        break;
            //                    }
            //                    continue;
            //    case 808120719:
            //      if (name == "Host")
            //                    {
            //                        defaultSettings._Host = str;
            //                        break;
            //                    }
            //                    continue;
            //    case 1331647074:
            //      if (name == "SendTimeout")
            //                    {
            //                        int result = 0;
            //                        if (int.TryParse(str, out result))
            //                        {
            //                            defaultSettings._SendTimeout = result;
            //                            break;
            //                        }
            //                        break;
            //                    }
            //                    continue;
            //    case 1606619994:
            //      if (name == "LogXML")
            //                    {
            //                        bool result = false;
            //                        if (bool.TryParse(str, out result))
            //                        {
            //                            defaultSettings._LogXml = result;
            //                            break;
            //                        }
            //                        break;
            //                    }
            //                    continue;
            //    case 2378346075:
            //      if (name == "XMLLogPath")
            //                    {
            //                        try
            //                        {
            //                            if (!Directory.Exists(str))
            //                                Directory.CreateDirectory(str);
            //                            defaultSettings._XMLLogPath = str;
            //                            break;
            //                        }
            //                        catch (Exception ex)
            //                        {
            //                            string message2 = string.Format("Unable to validate log path\r\n{0}", ex.ToString());
            //                            Trace.TraceWarning(message2);
            //                            if (ServerConnectionSettings.s_EventLog != null)
            //                            {
            //                                ServerConnectionSettings.s_EventLog.WriteEntry(message2, EventLogEntryType.Warning);
            //                                break;
            //                            }
            //                            break;
            //                        }
            //                    }
            //                    else
            //                        continue;
            //    case 2629024471:
            //      if (name == "HttpReadWriteTimeout")
            //                    {
            //                        int result = 0;
            //                        if (int.TryParse(str, out result))
            //                        {
            //                            defaultSettings._HttpReadWriteTimeout = result;
            //                            break;
            //                        }
            //                        break;
            //                    }
            //                    continue;
            //    case 2778009854:
            //      if (name == "AppServerUriStem")
            //                    {
            //                        defaultSettings._appServerUriStem = string.IsNullOrWhiteSpace(str) ? string.Empty : str.Trim();
            //                        break;
            //                    }
            //                    continue;
            //    case 3159403304:
            //      if (name == "HttpTimeout")
            //                    {
            //                        int result = 0;
            //                        if (int.TryParse(str, out result))
            //                        {
            //                            defaultSettings._HttpTimeout = result;
            //                            break;
            //                        }
            //                        break;
            //                    }
            //                    continue;
            //    case 3725526197:
            //      if (name == "ReceiveTimeout")
            //                    {
            //                        int result = 0;
            //                        if (int.TryParse(str, out result))
            //                        {
            //                            defaultSettings._ReceiveTimeout = result;
            //                            break;
            //                        }
            //                        break;
            //                    }
            //                    continue;
            //    case 3804576966:
            //      if (name == "Port")
            //                    {
            //                        int result = 0;
            //                        if (int.TryParse(str, out result) && (0 <= result && result <= (int)ushort.MaxValue))
            //                        {
            //                            defaultSettings._Port = result;
            //                            break;
            //                        }
            //                        break;
            //                    }
            //                    continue;
            //                    default:
            //      continue;
            //                }
            //            }
            //        }
            //    }
            //    defaultSettings._changeNumber = Interlocked.Increment(ref ServerConnectionSettings.s_ChangeNumber);
            //    Interlocked.Exchange<ServerConnectionSettings>(ref ServerConnectionSettings.s_DefaultSettings, defaultSettings);
            //    flag1 = true;
            //}
            //catch (Exception ex)
            //{
            //    flag1 = false;
            //    string message = string.Format("Error reading cconfiguration file\r\n{0}", ex.ToString());
            //    Trace.TraceWarning(message);
            //    if (ServerConnectionSettings.s_EventLog != null)
            //        ServerConnectionSettings.s_EventLog.WriteEntry(message, EventLogEntryType.Warning);
            //}
            //return flag1;
        }

        public static string DefaultConfigurationFile
        {
            get => ServerConnectionSettings.s_DefaultConfigurationFile;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !File.Exists(value))
                    return;
                Interlocked.Exchange<string>(ref ServerConnectionSettings.s_DefaultConfigurationFile, value);
            }
        }

        public static int CurrentChangeNumber => ServerConnectionSettings.s_ChangeNumber;

        public static ServerConnectionSettings DefaultSettings
        {
            get => ServerConnectionSettings.s_DefaultSettings.Clone();
            set
            {
                if (value == null)
                    return;
                Interlocked.Exchange<ServerConnectionSettings>(ref ServerConnectionSettings.s_DefaultSettings, value);
            }
        }

      
    }
}