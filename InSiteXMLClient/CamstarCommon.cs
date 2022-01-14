using Camstar.XMLClient.API;
using Camstar.XMLClient.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace InSiteXMLClient
{
    public class CamstarHelper
    {
        private CsiClient _client;
        private ICsiConnection _connection;
        private ICsiSession _session;
        private ICsiDocument _document;
        private ICsiService _service;
        private Guid _sessionId;
        public Guid SessionId => _sessionId;

        /// <summary>
        ///
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public CamstarHelper(string host, int port, string userName, string password)
        {
            _session = null;
            _client = null;
            _client = new CsiClient();
            _connection = null;
            _sessionId = Guid.NewGuid();
            _connection = _client.CreateConnection(host, port);
            _session = _connection.CreateSession(userName, password, _sessionId.ToString());
        }

        #region Document操作

        /// <summary>
        /// 打印Document文档
        /// </summary>

        public void Print()
        {
            _document.Print(true);
        }

        /// <summary>
        /// 提交文档
        /// </summary>
        /// <returns></returns>
        public ICsiDocument Submit()
        {
            return _document.Submit();
        }

        /// <summary>
        /// 提交并返回实体
        /// </summary>
        /// <typeparam name="T">泛型实体类</typeparam>
        /// <returns></returns>
        public T SubmitAndRequestModel<T>() where T : class, new()
        {
            var model = new T();
            var properties = typeof(T).GetProperties();
            var requestData = RequestData();
            foreach (var p in properties)
            {
                requestData.RequestField(p.Name);
            }
            var requestDoc = Submit();

            if (requestDoc.CheckErrors()) return null;
            foreach (var field in requestDoc.GetService().ResponseData().GetResponseFields())
            {
                var fieldType = field.GetType().Name;
                var name = ((ICsiXmlElement)field).GetElementName();
                var p = properties.FirstOrDefault(m => m.Name.ToLower() == name.ToLower());
                if (p == null)
                {
                    continue;
                }
                switch (fieldType)
                {
                    case "CsiDataField":
                        var data1 = (ICsiDataField)field;
                        p.SetValue(model, Convert.ChangeType(data1.GetValue(), p.PropertyType));
                        break;

                    case "CsiDataList":
                        var data2 = (ICsiDataList)field;
                        if (data2.HasChildren())
                        {
                        }
                        break;

                    case "CsiNamedObject":
                        var data3 = (ICsiNamedObject)field;
                        p.SetValue(model, Convert.ChangeType(data3.GetRef(), p.PropertyType));
                        break;

                    case "CsiNamedObjectList":
                        var data4 = (ICsiNamedObjectList)field;
                        if (data4.HasChildren())
                        {
                            p.GetSetMethod().Invoke(model, new[] { (from ICsiNamedObject item in data4.GetListItems() select item.GetRef()).ToList() });
                        }
                        break;

                    case "CsiRevisionedObject":
                        var data5 = (ICsiRevisionedObject)field;
                        p.SetValue(model, Convert.ChangeType(data5.GetName(), p.PropertyType));
                        break;

                    case "CsiRevisionedObjectList":
                        var data6 = (ICsiRevisionedObjectList)field;
                        if (data6.HasChildren())
                        {
                            p.GetSetMethod().Invoke(model, new[] { (from ICsiRevisionedObject item in data6.GetListItems() select item.GetName()).ToList() });
                        }
                        break;

                    case "CsiContainer":
                        var data7 = (ICsiContainer)field;
                        p.SetValue(model, Convert.ChangeType(data7.GetName(), p.PropertyType));
                        break;

                    case "CsiContainerList":
                        var data8 = (ICsiContainerList)field;
                        if (data8.HasChildren())
                        {
                            p.GetSetMethod().Invoke(model, new[] { (from ICsiContainer item in data8.GetListItems() select item.GetName()).ToList() });
                        }
                        break;
                }
            }

            return model;
        }

        /// <summary>
        /// 创建文档和服务
        /// </summary>
        /// <param name="documentName">文档名称</param>
        /// <param name="serviceName">服务名称</param>
        private void CreateDocumentAndService(string documentName, string serviceName)
        {
            if (!string.IsNullOrEmpty(documentName.Trim()))
            {
                _session.RemoveDocument(documentName);
                if (_service != null)
                {
                    _service = null;
                }
                _document = _session.CreateDocument(documentName);
                if (!string.IsNullOrEmpty(serviceName.Trim()))
                {
                    _service = _document.CreateService(serviceName);
                }
            }
        }

        /// <summary>
        /// 建立查询
        /// </summary>
        /// <returns></returns>
        public ICsiQuery CreateQuery()
        {
            var inputDoc = this._session.CreateDocument("Query");
            return inputDoc.CreateQuery();
        }

        #endregion Document操作

        #region Service操作封装

        /// <summary>
        /// 执行service的setExecute
        /// </summary>
        public void Execute()
        {
            _service.SetExecute();
        }

        /// <summary>
        /// 执行service的setExecute，并提交文档到服务器，获取返回结果和信息
        /// </summary>
        /// <param name="action">获取信息的方法</param>
        /// <returns></returns>
        private bool Execute(Action<string> action)
        {
            Execute();
            RequestData().RequestField("CompletionMsg");
            var responseDocument = Submit();
            return !responseDocument.CheckErrors();
        }

        /// <summary>
        /// 执行service的setExecute，并提交文档到服务器，获取返回结果信息
        /// </summary>
        /// <returns></returns>
        public (bool Status, string Message) ExecuteResult()
        {
            try
            {
                Execute();
                RequestData().RequestField("CompletionMsg");
                var responseDocument = Submit();
                var b = false;
                var msg = "";
                b = !responseDocument.CheckErrors(s => msg = s);
                return (Status: b, Message: msg);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + ":request:" + this._document.AsXml() + "\nresponse:" +
                                    this._document.ResponseData()?.GetOwnerDocument()?.AsXml());
                ;
            }
        }

        /// <summary>
        /// 执行service的setExecute，并提交文档到服务器，获取返回需要返回的字段结果信息
        /// </summary>
        /// <param name="requestFields">返回的字段 多节点用.分隔</param>
        /// <returns></returns>
        public (bool Status, string Message, ICsiDocument Document) ExecuteResult(IEnumerable<string> requestFields)
        {
            try
            {
                Execute();
                var req = RequestData();
                req.RequestField("CompletionMsg");
                foreach (var item in requestFields)
                {
                    req.RequestField(item);
                }
                var responseDocument = Submit();
                var b = false;
                var msg = "";
                b = !responseDocument.CheckErrors(s => msg = s);
                return (Status: b, Message: msg, responseDocument);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + ":request:" + this._document.AsXml() + "\nresponse:" +
                                    this._document.ResponseData()?.GetOwnerDocument()?.AsXml());
                ;
            }
        }

        /// <summary>
        /// 指定service执行的事件
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <returns></returns>
        public ICsiPerform Perform(PerformType type)
        {
            switch (type)
            {
                case PerformType.Load:
                    return _service.Perform("Load");

                case PerformType.New:
                    return _service.Perform("New");

                case PerformType.Change:
                    return _service.Perform("Load");

                case PerformType.Delete:
                    return _service.Perform("delete");

                case PerformType.NewRev:
                    return _service.Perform("NewRev");

                default:
                    return null;
            }
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="evenName">事件名称</param>
        /// <returns></returns>
        public ICsiPerform Perform(string evenName)
        {
            return _service.Perform(evenName);
        }

        /// <summary>
        /// 返回service的requestData
        /// </summary>
        /// <returns></returns>
        public ICsiRequestData RequestData()
        {
            return _service.RequestData();
        }

        /// <summary>
        /// 创建InputData
        /// </summary>
        /// <returns></returns>
        public ICsiObject InputData()
        {
            return _service.InputData();
        }

        /// <summary>
        /// 创建服务，默认文档名称为服务名加Doc
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        public void CreateService(string serviceName)
        {
            CreateDocumentAndService(serviceName + "Doc", serviceName);
        }

        /// <summary>
        /// 更改命名对象
        /// </summary>
        /// <typeparam name="name"></typeparam>
        /// <returns></returns>

        public ICsiNamedObject Changes(string name)
        {
            this.InputData().NamedObjectField("ObjectToChange").SetRef(name);
            this.Perform(PerformType.Load);
            return this.InputData().NamedObjectField("ObjectChanges");
        }

        /// <summary>
        /// 删除命名对象
        /// </summary>
        /// <typeparam name="name"></typeparam>
        /// <returns></returns>

        public (bool Status, string Message) Delete(string name)
        {
            this.InputData().NamedObjectField("ObjectToChange").SetRef(name);
            this.Perform(PerformType.Delete);
            return this.ExecuteResult();
        }

        /// <summary>
        /// 更改版本对象
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="rev">版本</param>
        /// <param name="useRor">是否使用默认版本</param>

        public ICsiRevisionedObject Changes(string name, string rev, bool useRor)
        {
            this.InputData().RevisionedObjectField("ObjectToChange").SetRef(name, rev, useRor);
            this.Perform(PerformType.Load);
            return this.InputData().RevisionedObjectField("ObjectChanges");
        }

        /// <summary>
        /// 删除版本对象
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="rev">版本</param>
        /// <param name="deleteAllRev">删除所有</param>

        public (bool Status, string Message) Delete(string name, string rev, bool deleteAllRev = false)
        {
            this.InputData().RevisionedObjectField("ObjectToChange").SetRef(name, rev, false);
            this.Perform(PerformType.Delete);
            return this.ExecuteResult();
        }

        /// <summary>
        /// 新建ObjectChanges对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ICsiNamedObject New(string name)
        {
            this.Perform(PerformType.New);
            var objectChanges = this.InputData().NamedObjectField("ObjectChanges");
            objectChanges.DataField("Name").SetValue(name);
            return objectChanges;
        }

        /// <summary>
        /// 新建ObjectChanges对象
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="rev">版本</param>
        /// <param name="useRor">是否设置为默认版本</param>
        public ICsiRevisionedObject New(string name, string rev, bool useRor)
        {
            this.Perform(PerformType.New);
            var objectChanges = this.InputData().RevisionedObjectField("ObjectChanges");
            objectChanges.DataField("Name").SetValue(name);
            objectChanges.DataField("Revision").SetValue(rev);
            objectChanges.DataField("IsRevofRcd").SetValue(useRor);
            return objectChanges;
        }

        /// <summary>
        /// 执行sql 参数名在SQL语句中中以?开头
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public DataTable QueryTable(string sql, Dictionary<string, string> parameter = null)
        {
            var inputDoc = this._session.CreateDocument("AdHocQuery");

            var query = inputDoc.CreateQuery();
            query.SetSqlText(sql);
            if (parameter != null)
                foreach (var pa in parameter?.Keys)
                {
                    query.SetParameter(pa, parameter[pa]);
                }
            var responseDoc = inputDoc.Submit();

            if (null != responseDoc)
            {
                query = responseDoc.GetQuery();
                if (null != query)
                {
                    var recordset = query.GetRecordset();
                    if (null != recordset)
                    {
                        return recordset.GetAsDataTable();
                        //var data = new DataTable();
                        //for (var i = 0; i < recordset.GetRecordCount(); i++)
                        //{
                        //    recordset.MoveNext();
                        //    var arrOfQueryFields = recordset.GetFields();
                        //    var values = new List<string>();
                        //    foreach (CsiRecordsetField recordSetField in arrOfQueryFields)
                        //    {
                        //        if (i == 0)
                        //        {
                        //            data.Columns.Add(recordSetField.GetName());
                        //        }
                        //        values.Add(recordSetField.GetValue());
                        //    }

                        //    data.Rows.Add(values.ToArray());
                        //}

                        //return data;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 获取Model 参数名在SQL语句中中以?开头
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql, Dictionary<string, string> parameter = null)
        {
            try
            {
                var inputDoc = this._session.CreateDocument("AdHocQuery");
                var query = inputDoc.CreateQuery();
                query.SetSqlText(sql);
                if (parameter != null)
                    foreach (var pa in parameter?.Keys)
                    {
                        query.SetParameter(pa, parameter[pa]);
                    }

                var properties = typeof(T).GetProperties();

                var responseDoc = inputDoc.Submit();
                if (null != responseDoc)
                {
                    query = responseDoc.GetQuery();
                    if (null != query)
                    {
                        var recordset = query.GetRecordset();
                        if (null != recordset)
                        {
                            var data = new List<T>();
                            for (var i = 0; i < recordset.GetRecordCount(); i++)
                            {
                                recordset.MoveNext();
                                var arrOfQueryFields = recordset.GetFields();
                                var model = Activator.CreateInstance(typeof(T));
                                int x = 0;
                                foreach (CsiRecordsetField recordSetField in arrOfQueryFields)
                                {
                                    var p = properties.FirstOrDefault(m => m.Name.ToLower() == recordSetField.GetName().ToLower());
                                    var v = recordSetField.GetValue();
                                    if (p != null)
                                    {
                                        switch (p.PropertyType.Name)
                                        {
                                            case "Int32":
                                            case "Int16":
                                                p.SetValue(model, int.Parse(string.IsNullOrEmpty(v) ? "0" : v));
                                                break;

                                            case "Int64":
                                                p.SetValue(model, long.Parse(string.IsNullOrEmpty(v) ? "0" : v));
                                                break;

                                            case "Double":
                                            case "Float":
                                                p.SetValue(model, long.Parse(string.IsNullOrEmpty(v) ? "0" : v));
                                                break;

                                            case "Boolean":
                                                p.SetValue(model, bool.Parse(string.IsNullOrEmpty(v) ? "0" : v));
                                                break;

                                            case "String":
                                                p.SetValue(model, recordSetField.GetValue());
                                                break;

                                            case "DateTime":
                                                p.SetValue(model, DateTime.Parse(string.IsNullOrEmpty(v) ? DateTime.MinValue.ToString() : v));
                                                break;

                                            case "Decimal":
                                                p.SetValue(model, Decimal.Parse(string.IsNullOrEmpty(v) ? "0" : v));
                                                break;

                                            default:
                                                p.SetValue(model, null);
                                                break;
                                        }
                                        // p.SetValue(model, recordSetField.GetValue());
                                        x++;
                                    }
                                }

                                if (x > 0)
                                {
                                    data.Add((T)model);
                                }
                            }

                            return data;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return null;
        }

        #endregion Service操作封装

        public enum PerformType
        {
            /// <summary>
            /// 载入
            /// </summary>
            Load,

            /// <summary>
            /// 新建
            /// </summary>
            New,

            /// <summary>
            /// 修改
            /// </summary>
            Change,

            /// <summary>
            /// 删除
            /// </summary>
            Delete,

            /// <summary>
            /// 添加新版本
            /// </summary>
            NewRev
        }
    }

    /// <summary>
    /// CamstarCommon 扩展类
    /// </summary>
    public static class CamstarCommonEx
    {
        /// <summary>
        /// 打印文档
        /// </summary>
        /// <param name="document">文档</param>
        /// <param name="isInput">是否输入文档</param>
        public static void Print(this ICsiDocument document, bool isInput = false)
        {
            PrintDocument(document.AsXml(), isInput);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="isInput"></param>
        private static void PrintDocument(string doc, bool isInput)
        {
            string filePath = Path.GetTempPath();
            filePath = Path.Combine(filePath, isInput ? "request.xml" : "response.xml");
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                StreamWriter writer = new StreamWriter(fs);
                writer.Write(doc);
                writer.Flush();
                writer.Close();
                fs.Close();
            }
        }

        /// <summary>
        /// 检查错误，并返回错误信息,存在错误返回true
        /// </summary>
        /// <param name="document">文档</param>
        /// <param name="errorMsg">错误信息</param>
        /// <returns>存在错误返回true</returns>
        public static bool CheckErrors(this ICsiDocument document, ref string errorMsg)
        {
            ICsiExceptionData exceptionData;
            ICsiDataField completionMessage;
            ICsiService gService;

            if (document.CheckErrors())
            {
                exceptionData = document.ExceptionData();
                errorMsg = exceptionData.GetDescription();
                return true;
            }
            else
            {
                gService = document.GetService();
                if (gService != null)
                {
                    completionMessage = (ICsiDataField)gService.ResponseData().GetResponseFieldByName("CompletionMsg");
                    errorMsg = completionMessage.GetValue();
                }

                return false;
            }
        }

        /// <summary>
        /// 检查文档错误，并处理错误信息,存在错误返回true
        /// </summary>
        /// <param name="document">文档</param>
        /// <param name="action">处理方法</param>
        /// <returns>存在错误返回true</returns>
        public static bool CheckErrors(this ICsiDocument document, Action<string> action)
        {
            string msg = string.Empty;
            bool result = document.CheckErrors(ref msg);
            action(msg);
            return result;
        }

        /// <summary>
        /// 赋值
        /// </summary>
        /// <param name="csiDataField"></param>
        /// <param name="value"></param>
        public static void SetValue(this ICsiDataField csiDataField, object value)
        {
            if (value is bool b)
            {
                csiDataField.SetValue(b ? 1 : 0);
            }
            else if (value is DateTime)
            {
                csiDataField.SetValue(((DateTime)value).ToString("O"));
            }
            else
            {
                csiDataField.SetValue(value.ToString());
            }
        }

        /// <summary>
        /// 执行sql 参数名在SQL语句中中以?开头
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static DataTable ExecuteTable(this ICsiQuery query, Dictionary<string, string> parameter = null)
        {
            if (parameter != null)
                foreach (var pa in parameter.Keys)
                {
                    query.SetParameter(pa, parameter[pa]);
                }
            var responseDoc = query.GetOwnerDocument().Submit();

            if (null != responseDoc)
            {
                query = responseDoc.GetQuery();
                if (null != query)
                {
                    var recordset = query.GetRecordset();
                   return recordset.GetAsDataTable();
                    //if (null != recordset)
                    //{
                    //    var data = new DataTable();
                    //    for (var i = 0; i < recordset.GetRecordCount(); i++)
                    //    {
                    //        recordset.MoveNext();
                    //        var arrOfQueryFields = recordset.GetFields();
                    //        var values = new List<string>();
                    //        foreach (CsiRecordsetField recordSetField in arrOfQueryFields)
                    //        {
                    //            if (i == 0)
                    //            {
                    //                data.Columns.Add(recordSetField.GetName());
                    //            }
                    //            values.Add(recordSetField.GetValue());
                    //        }

                    //        data.Rows.Add(values.ToArray());
                    //    }

                    //    return data;
                    //}
                }
            }

            return null;
        }

        /// <summary>
        /// 获取Model 参数名在SQL语句中中以?开头
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static IEnumerable<T> Execute<T>(this ICsiQuery query, Dictionary<string, string> parameter = null)
        {
            try
            {
                if (parameter != null)
                    foreach (var pa in parameter?.Keys)
                    {
                        query.SetParameter(pa, parameter[pa]);
                    }
                var properties = typeof(T).GetProperties();
                var responseDoc = query.GetOwnerDocument().Submit();
                if (null != responseDoc)
                {
                    query = responseDoc.GetQuery();
                    if (null != query)
                    {
                        var recordset = query.GetRecordset();
                        if (null != recordset)
                        {
                            var data = new List<T>();
                            for (var i = 0; i < recordset.GetRecordCount(); i++)
                            {
                                recordset.MoveNext();
                                var arrOfQueryFields = recordset.GetFields();
                                var model = Activator.CreateInstance(typeof(T));
                                int x = 0;
                                foreach (CsiRecordsetField recordSetField in arrOfQueryFields)
                                {
                                    var p = properties.FirstOrDefault(m => m.Name.ToLower() == recordSetField.GetName().ToLower());
                                    var v = recordSetField.GetValue();
                                    if (p != null)
                                    {
                                        switch (p.PropertyType.Name)
                                        {
                                            case "Int32":
                                            case "Int16":
                                                p.SetValue(model, int.Parse(string.IsNullOrEmpty(v) ? "0" : v));
                                                break;

                                            case "Int64":
                                                p.SetValue(model, long.Parse(string.IsNullOrEmpty(v) ? "0" : v));
                                                break;

                                            case "Double":
                                            case "Float":
                                                p.SetValue(model, long.Parse(string.IsNullOrEmpty(v) ? "0" : v));
                                                break;

                                            case "Boolean":
                                                p.SetValue(model, bool.Parse(string.IsNullOrEmpty(v) ? "0" : v));
                                                break;

                                            case "String":
                                                p.SetValue(model, recordSetField.GetValue());
                                                break;

                                            case "DateTime":
                                                p.SetValue(model, DateTime.Parse(string.IsNullOrEmpty(v) ? DateTime.MinValue.ToString() : v));
                                                break;

                                            case "Decimal":
                                                p.SetValue(model, Decimal.Parse(string.IsNullOrEmpty(v) ? "0" : v));
                                                break;

                                            default:
                                                p.SetValue(model, null);
                                                break;
                                        }
                                        // p.SetValue(model, recordSetField.GetValue());
                                        x++;
                                    }
                                }

                                if (x > 0)
                                {
                                    data.Add((T)model);
                                }
                            }

                            return data;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return null;
        }
    }
}