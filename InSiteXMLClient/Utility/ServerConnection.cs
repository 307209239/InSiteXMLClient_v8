using Camstar.Util;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Xml;

namespace Camstar.Utility
{
    public class ServerConnection
    {
        public ServerConnectionSettings Settings = ServerConnectionSettings.DefaultSettings;
        private TcpClient _TcpClient = (TcpClient)null;
        private NetworkStream _Stream = (NetworkStream)null;
        private string _InboundXML = string.Empty;
        private string _InboundXMLFile = string.Empty;
        ~ServerConnection() => this.Disconnect();

        public virtual string Host
        {
            get => this.Settings.Host;
            set => this.Settings.Host = value;
        }

        public virtual int Port
        {
            get => this.Settings.Port;
            set => this.Settings.Port = value;
        }

        public virtual int SendTimeout
        {
            get => this.Settings.SendTimeout;
            set => this.Settings.SendTimeout = value;
        }

        public virtual int ReceiveTimeout
        {
            get => this.Settings.ReceiveTimeout;
            set => this.Settings.ReceiveTimeout = value;
        }

        public virtual string XMLLogPath
        {
            get => this.Settings.XMLLogPath;
            set => this.Settings.XMLLogPath = value;
        }

        public virtual bool LogXML
        {
            get => this.Settings.LogXml;
            set => this.Settings.LogXml = value;
        }

        public virtual string InboundXML
        {
            get => this._InboundXML;
            set
            {
                if (StringUtil.IsEmptyString(value))
                    return;
                this._InboundXML = value;
            }
        }

        public virtual string InboundXMLFile
        {
            get => this._InboundXMLFile;
            set
            {
                char[] chArray = new char[3]
                {
          '\n',
          char.MinValue,
          ' '
                };
                if (!StringUtil.IsEmptyString(value) && value.Trim(chArray).Length > 1)
                {
                    this._InboundXMLFile = value;
                    using (StreamReader streamReader = new StreamReader(this._InboundXMLFile))
                    {
                        this._InboundXML = streamReader.ReadToEnd();
                        streamReader.Close();
                    }
                }
                else
                    this._InboundXMLFile = "";
            }
        }

        public virtual string ConfigurationFile
        {
            get => ServerConnectionSettings.DefaultConfigurationFile;
            set
            {
                if (StringUtil.IsEmptyString(value))
                    return;
                ServerConnectionSettings.DefaultConfigurationFile = value;
                this.Settings = ServerConnectionSettings.DefaultSettings;
            }
        }

        public virtual bool Connect()
        {
            if (this.Settings.Host == null)
                throw new ArgumentNullException();
            if (this.Settings.Protocol == ApplicationProtocol.Auto)
            {
                switch (this.Settings.Port)
                {
                    case 80:
                        this.Settings.Protocol = ApplicationProtocol.Http;
                        break;
                    case 443:
                        this.Settings.Protocol = ApplicationProtocol.Https;
                        break;
                    case 2150:
                        this.Settings.Protocol = ApplicationProtocol.InSiteSockets;
                        break;
                    case 2881:
                        this.Settings.Protocol = ApplicationProtocol.InSiteSockets;
                        break;
                    case 8080:
                        this.Settings.Protocol = ApplicationProtocol.Http;
                        break;
                    default:
                        this.Settings.Protocol = ApplicationProtocol.Https;
                        break;
                }
            }
            if (!this.Settings.Protocol.Equals((object)ApplicationProtocol.InSiteSockets))
                return true;
            bool flag;
            try
            {
                this._TcpClient = new TcpClient();
                this._TcpClient.ReceiveBufferSize = 75000;
                this._TcpClient.SendTimeout = this.Settings.SendTimeout;
                this._TcpClient.ReceiveTimeout = this.Settings.ReceiveTimeout;
                this._TcpClient.Connect(this.Settings.Host, this.Settings.Port);
                this._Stream = this._TcpClient.GetStream();
                flag = true;
            }
            catch (SocketException ex)
            {
                this._TcpClient = (TcpClient)null;
               
                throw ex;
            }
            return flag;
        }

        public virtual bool Connect(string HostName, int PortNo)
        {
            this.Settings.Host = HostName;
            this.Settings.Port = PortNo;
            return this.Connect();
        }

        public virtual void Disconnect()
        {
            if (this._TcpClient != null)
            {
                this._TcpClient.Close();
                this._TcpClient = (TcpClient)null;
            }
            if (this._Stream != null)
            {
                this._Stream.Close();
                this._Stream = (NetworkStream)null;
            }
            this._InboundXML = string.Empty;
            this._InboundXMLFile = string.Empty;
        }

        public virtual bool Send()
        {
            bool flag = false;
            if (this._TcpClient != null)
            {
                try
                {
                    this.SendRequest();
                    flag = true;
                }
                catch (Exception ex)
                {
                    
                    throw ex;
                }
            }
            return flag;
        }

        public virtual bool Send(string inputXML)
        {
            this.InboundXML = inputXML;
            return this.Send();
        }

        public virtual bool Receive(out string outputXML)
        {
            bool flag = false;
            outputXML = string.Empty;
            if (this._TcpClient != null)
            {
                try
                {
                    outputXML = this.ReceiveResponse();
                    flag = true;
                    if (this.Settings.LogXml)
                        this.LogDocument("XmlResponse", outputXML);
                }
                catch (IOException ex)
                {
                   
                    if (this.Settings.LogXml)
                        this.LogDocument("XmlResponse", outputXML);
                    throw ex;
                }
                catch (Exception ex)
                {
                  
                    throw ex;
                }
            }
            return flag;
        }

        public virtual string Submit(string host, int port) => this.Submit(host, port, (string)null, (string)null, this.Settings.LogXml, (string)null);

        public virtual string Submit(string host, int port, string inboundXML) => this.Submit(host, port, inboundXML, (string)null, this.Settings.LogXml, (string)null);

        public virtual string Submit(string host, int port, string inboundXML, bool logXML) => this.Submit(host, port, inboundXML, (string)null, logXML, (string)null);

        public virtual string Submit(
          string host,
          int port,
          string inboundXML,
          bool logXML,
          string xmlLogPath)
        {
            return this.Submit(host, port, inboundXML, (string)null, logXML, xmlLogPath);
        }

        public virtual string Submit(
          string host,
          int port,
          string inboundXML,
          string inboundXMLFilePath)
        {
            return this.Submit(host, port, inboundXML, inboundXMLFilePath, this.Settings.LogXml, (string)null);
        }

        public virtual string Submit(
          string host,
          int port,
          string inboundXML,
          string inboundXMLFilePath,
          bool logXML)
        {
            return this.Submit(host, port, inboundXML, inboundXMLFilePath, logXML, (string)null);
        }

        public virtual string Submit(
          string host,
          int port,
          string inboundXML,
          string inboundXMLFilePath,
          bool logXML,
          string xmlLogPath)
        {
            if (!StringUtil.IsEmptyString(host))
                this.Settings.Host = host;
            if (port != -1)
                this.Settings.Port = port;
            if (!StringUtil.IsEmptyString(inboundXML))
                this._InboundXML = inboundXML;
            this.Settings.LogXml = logXML;
            if (!StringUtil.IsEmptyString(inboundXMLFilePath))
                this.InboundXMLFile = inboundXMLFilePath;
            if (!StringUtil.IsEmptyString(xmlLogPath))
                this.Settings.XMLLogPath = xmlLogPath;
            return this.Submit();
        }

        public virtual string Submit(string host) => this.Submit(host, -1, (string)null, (string)null, this.Settings.LogXml, (string)null);

        public virtual string Submit(string host, string inboundXML) => this.Submit(host, -1, inboundXML, (string)null, this.Settings.LogXml, (string)null);

        public virtual string Submit(string host, string inboundXML, bool logXML) => this.Submit(host, -1, inboundXML, (string)null, logXML, (string)null);

        public virtual string Submit(string host, string inboundXML, bool logXML, string xmlLogPath) => this.Submit(host, -1, inboundXML, (string)null, logXML, xmlLogPath);

        public virtual string Submit(string host, string inboundXML, string inboundXMLFilePath) => this.Submit(host, -1, inboundXML, inboundXMLFilePath, this.Settings.LogXml, (string)null);

        public virtual string Submit(
          string host,
          string inboundXML,
          string inboundXMLFilePath,
          bool logXML)
        {
            return this.Submit(host, -1, inboundXML, inboundXMLFilePath, logXML, (string)null);
        }

        public virtual string Submit(
          string host,
          string inboundXML,
          string inboundXMLFilePath,
          bool logXML,
          string xmlLogPath)
        {
            return this.Submit(host, -1, inboundXML, inboundXMLFilePath, logXML, xmlLogPath);
        }

        public virtual string Submit()
        {
            lock (this)
            {
                string outputXML = string.Empty;
                try
                {
                    if (this.Connect())
                    {
                        switch (this.Settings.Protocol)
                        {
                            case ApplicationProtocol.Http:
                                outputXML = this.SendReceivetHttp();
                                break;
                            case ApplicationProtocol.Https:
                                outputXML = this.SendReceivetHttp();
                                break;
                            default:
                                if (this.Send())
                                {
                                    this.Receive(out outputXML);
                                    break;
                                }
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    
                    throw ex;
                }
                finally
                {
                    this.Disconnect();
                }
                return outputXML;
            }
        }

        protected virtual void SendRequest()
        {
            string str = this.EncryptPassword(this.InboundXML) + "\0";
            if (this.Settings.LogXml)
                this.LogDocument("XmlRequest", str);
            byte[] bytes = Encoding.Unicode.GetBytes(str);
            this._Stream.Write(bytes, 0, bytes.Length);
        }

        protected virtual string ReceiveResponse()
        {
            string empty = string.Empty;
            byte num = 0;
            bool flag = false;
            byte[] numArray = new byte[this._TcpClient.ReceiveBufferSize + 1];
            try
            {
                int count;
                do
                {
                    int offset;
                    if (flag)
                    {
                        offset = 1;
                        numArray[0] = num;
                    }
                    else
                        offset = 0;
                    count = this._Stream.Read(numArray, offset, this._TcpClient.ReceiveBufferSize);
                    if (flag)
                        ++count;
                    if (count > 0)
                    {
                        flag = count % 2 == 1;
                        if (flag)
                        {
                            --count;
                            num = numArray[count];
                        }
                        empty += Encoding.Unicode.GetString(numArray, 0, count);
                    }
                }
                while (count > 0 || this._Stream.DataAvailable);
            }
            catch (IOException ex)
            {
                
                empty.TrimEnd(new char[1]);
                throw ex;
            }
            return empty.TrimEnd(new char[1]);
        }

        protected virtual string SendReceivetHttp()
        {
            string str1 = this.EncryptPassword(this.InboundXML);
            if (this.Settings.LogXml)
                this.LogDocument("XmlRequest", str1);
            byte[] bytes = Encoding.Unicode.GetBytes(str1);
            bool flag = ApplicationProtocol.Https == this.Settings.Protocol;
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Host = this.Settings.Host;
            uriBuilder.Path = this.Settings.AppServerUriStem;
            if (!string.IsNullOrWhiteSpace(uriBuilder.Path))
            {
                if (!uriBuilder.Path.EndsWith("/"))
                    uriBuilder.Path += "/";
            }
            else
                uriBuilder.Path = "/";
            uriBuilder.Scheme = flag ? "https" : "http";
            if (flag && 443 != this.Settings.Port || !flag && 80 != this.Settings.Port)
                uriBuilder.Port = this.Settings.Port;
            if (-1 != this.InboundXML.IndexOf("<__service"))
            {
                uriBuilder.Path += "service/";
                int startIndex = this.InboundXML.IndexOf("__serviceType=\"") + "__serviceType =\"".Length - 1;
                string str2 = this.InboundXML.Substring(startIndex, this.InboundXML.IndexOf("\">", startIndex) - startIndex);
                uriBuilder.Path += str2;
            }
            else if (-1 != this.InboundXML.IndexOf("<__query"))
                uriBuilder.Path += "query/";
            else if (-1 != this.InboundXML.IndexOf("<__requestData"))
            {
                if (-1 != this.InboundXML.IndexOf("<__metadata"))
                {
                    if (-1 != this.InboundXML.IndexOf("<__label"))
                        uriBuilder.Path += "label/";
                    else
                        uriBuilder.Path += "metadata/";
                }
                else
                    uriBuilder.Path += "requestdata/";
            }
            HttpWebRequest http = WebRequest.CreateHttp(uriBuilder.Uri);
            http.Method = "POST";
            http.ContentType = "application/xml; charset=utf-16le";
            http.AllowWriteStreamBuffering = true;
            http.AllowAutoRedirect = false;
            http.PreAuthenticate = false;
            http.CookieContainer = new CookieContainer();
            http.KeepAlive = true;
            http.SendChunked = false;
            http.Accept = "application/xml; charset=utf-16le";
            http.Headers.Add(HttpRequestHeader.AcceptCharset, "utf-16le");
            http.Timeout = this.Settings.HttpTimeout;
            http.ReadWriteTimeout = this.Settings.HttpReadWriteTimeout;
            http.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            if (flag)
                http.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(this.RemoteCertificateValidationCallback);
            http.GetRequestStream().Write(bytes, 0, bytes.Length);
            HttpWebResponse response = (HttpWebResponse)http.GetResponse();
            string document = string.Empty;
            if ((ulong)response.ContentLength > 0UL)
            {
                Encoding encoding = Encoding.Unicode;
                try
                {
                    encoding = Encoding.GetEncoding(response.CharacterSet);
                }
                catch
                {
                }
                document = new StreamReader(response.GetResponseStream(), encoding).ReadToEnd();
            }
            if (this.Settings.LogXml)
                this.LogDocument("XmlResponse", document);
            return document;
        }

     
      

        protected virtual bool CheckLogPath(string path)
        {
            bool flag = false;
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                flag = true;
            }
            catch (Exception ex)
            {
               
            }
            return flag;
        }

        protected virtual string GetLogFile(string suffix)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(this.Settings.XMLLogPath);
            if (!this.Settings.XMLLogPath.EndsWith("\\"))
                stringBuilder.Append("\\");
            stringBuilder.Append(DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"));
            try
            {
                stringBuilder.Append(Thread.CurrentThread.ExecutionContext.GetHashCode().ToString("d2"));
            }
            catch (Exception ex)
            {
                
            }
            stringBuilder.Append('_');
            stringBuilder.Append(suffix);
            stringBuilder.Append('.');
            stringBuilder.Append("xml");
            return stringBuilder.ToString();
        }

        protected virtual string MaskPassword(string document)
        {
            string str = document;
            int startIndex = document.IndexOf("<password");
            int num = document.IndexOf("</password>");
            if (startIndex >= 0 && num >= 0)
            {
                int length = num + "</password>".Length - startIndex;
                string oldValue = document.Substring(startIndex, length);
                string newValue = "<password>" + new string('*', 8) + "</password>";
                str = document.Replace(oldValue, newValue);
            }
            return str;
        }

        protected virtual string EncryptPassword(string document)
        {
            string str1 = document;
            XmlWriter xmlWriter = (XmlWriter)null;
            XmlReader reader = (XmlReader)null;
            bool flag1 = false;
            try
            {
                StringBuilder sb = new StringBuilder();
                xmlWriter = (XmlWriter)new XmlTextWriter((TextWriter)new StringWriter(sb));
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreWhitespace = true;
                reader = XmlReader.Create((TextReader)new StringReader(document), settings);
                if (reader.Read())
                {
                    do
                    {
                        if (reader.NodeType == XmlNodeType.Element && (reader.LocalName == "__session" || reader.LocalName == "__connect"))
                            reader.Read();
                        else if (reader.NodeType == XmlNodeType.Element && (reader.LocalName == "password" || reader.LocalName == "sessionId"))
                        {
                            for (bool flag2 = reader.MoveToFirstAttribute(); flag2; flag2 = reader.MoveToNextAttribute())
                            {
                                if (reader.LocalName == "__encrypted" && string.Compare(reader.Value, "yes", true) == 0)
                                    flag1 = true;
                            }
                            reader.Close();
                        }
                        else
                            reader.Read();
                    }
                    while (!reader.EOF && reader.ReadState != System.Xml.ReadState.Closed);
                }
                reader = XmlReader.Create((TextReader)new StringReader(document), settings);
                if (reader.Read())
                {
                    do
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "__InSite")
                        {
                            string str2 = (string)null;
                            xmlWriter.WriteStartElement(reader.LocalName);
                            for (bool flag2 = reader.MoveToFirstAttribute(); flag2; flag2 = reader.MoveToNextAttribute())
                            {
                                if (reader.LocalName == "__encryption")
                                    str2 = reader.Value;
                                else
                                    xmlWriter.WriteAttributeString(reader.LocalName, reader.Value);
                            }
                            if (!flag1)
                                str2 = "2";
                            if (!string.IsNullOrEmpty(str2))
                                xmlWriter.WriteAttributeString("__encryption", str2);
                            reader.Read();
                        }
                        else if (reader.NodeType == XmlNodeType.Element && (reader.LocalName == "__session" || reader.LocalName == "__connect"))
                        {
                            xmlWriter.WriteStartElement(reader.LocalName);
                            xmlWriter.WriteAttributes(reader, true);
                            reader.Read();
                        }
                        else if (reader.NodeType == XmlNodeType.EndElement && (reader.LocalName == "__session" || reader.LocalName == "__connect"))
                        {
                            xmlWriter.WriteEndElement();
                            reader.Read();
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "password")
                        {
                            if (!flag1)
                            {
                                xmlWriter.WriteStartElement(reader.LocalName);
                                for (bool flag2 = reader.MoveToFirstAttribute(); flag2; flag2 = reader.MoveToNextAttribute())
                                {
                                    if (reader.LocalName != "__encrypted")
                                        xmlWriter.WriteAttributeString(reader.LocalName, reader.Value);
                                }
                                xmlWriter.WriteAttributeString("__encrypted", "yes");
                                int content = (int)reader.MoveToContent();
                                string fieldData = reader.ReadElementContentAsString();
                                xmlWriter.WriteValue(CryptUtil.Encrypt(fieldData));
                                xmlWriter.WriteEndElement();
                            }
                            else
                                xmlWriter.WriteNode(reader, true);
                        }
                        else
                            xmlWriter.WriteNode(reader, true);
                    }
                    while (!reader.EOF);
                    str1 = sb.ToString();
                }
            }
            catch (Exception ex)
            {
                str1 = document;
                
            }
            finally
            {
                reader?.Close();
                xmlWriter?.Close();
            }
            return str1;
        }

        protected virtual void LogDocument(string operation, string document)
        {
            try
            {
                if (!this.CheckLogPath(this.Settings.XMLLogPath))
                    return;
                string document1 = document.Replace("encoding=\"utf-16\"", "").Replace("encoding='UTF-16LE'", "").Replace("encoding=\"UTF-16LE\"", "");
                if (document1[document1.Length - 1] == char.MinValue)
                    document1 = document1.Remove(document1.Length - 1, 1);
                StreamWriter streamWriter = new StreamWriter(this.GetLogFile(operation), false, Encoding.Default);
                streamWriter.WriteLine(this.MaskPassword(document1));
                streamWriter.Close();
            }
            catch (Exception ex)
            {
                
            }
        }

        private bool RemoteCertificateValidationCallback(
          object sender,
          X509Certificate cert,
          X509Chain chain,
          SslPolicyErrors errors)
        {
            return (uint)errors <= 0U || (this.Settings.IgnoreInvalidCert || this.IsHostLocalMachine(this.Settings.Host));
        }

        private bool IsHostLocalMachine(string hostName)
        {
            if (!string.IsNullOrWhiteSpace(hostName))
            {
                string machineName = Environment.MachineName;
                if (hostName.Equals("localhost", StringComparison.InvariantCultureIgnoreCase) || hostName.Equals("127.0.0.1", StringComparison.InvariantCultureIgnoreCase) || hostName.Equals("::1", StringComparison.InvariantCultureIgnoreCase) || hostName.Equals(machineName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
                IPHostEntry hostEntry = Dns.GetHostEntry(machineName);
                foreach (string alias in hostEntry.Aliases)
                {
                    if (hostName.Equals(alias, StringComparison.InvariantCultureIgnoreCase))
                        return true;
                }
                foreach (IPAddress address in hostEntry.AddressList)
                {
                    if (hostName.Equals(address.ToString(), StringComparison.InvariantCultureIgnoreCase))
                        return true;
                }
            }
            return false;
        }

        private string XmlNodeToString(XmlNode node)
        {
            string empty = string.Empty;
            if (node != null)
            {
                StringBuilder sb = new StringBuilder();
                XmlTextWriter xmlTextWriter = new XmlTextWriter((TextWriter)new StringWriter(sb));
                node.WriteTo((XmlWriter)xmlTextWriter);
                empty = sb.ToString();
            }
            return empty;
        }
    }
}
