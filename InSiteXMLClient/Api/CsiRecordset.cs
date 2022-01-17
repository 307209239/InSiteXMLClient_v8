using Camstar.XMLClient.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;

namespace Camstar.XMLClient.API
{
    internal class CsiRecordset : CsiXmlElement, ICsiRecordset, ICsiXmlElement
    {
        private XmlNode mCurrentElement;

        public CsiRecordset(ICsiDocument doc, ICsiXmlElement element)
          : base(doc, "__recordSet", element)
        {
        }

        public CsiRecordset(ICsiDocument doc, XmlElement domElement)
          : base(doc, domElement)
        {
        }

        public virtual long GetRecordCount() => CsiXmlHelper.GetChildCount(this.GetDomElement());

        public void MoveFirst()
        {
            try
            {
                this.mCurrentElement = this.GoToNextElementNode(this.GetDomElement().FirstChild, true);
            }
            catch (Exception ex)
            {
                throw new CsiClientException(-1L, ex, this.GetType().FullName + ".moveFirst()");
            }
        }

        public void MoveLast()
        {
            try
            {
                this.mCurrentElement = this.GoToNextElementNode(this.GetDomElement().LastChild, false);
            }
            catch (Exception ex)
            {
                throw new CsiClientException(-1L, ex, this.GetType().FullName + ".moveLast()");
            }
        }

        public bool MoveNext()
        {
            try
            {
                if (this.mCurrentElement == null)
                {
                    this.MoveFirst();
                }
                else
                {
                    XmlNode nextSibling = this.mCurrentElement.NextSibling;
                    if (nextSibling == null)
                    {
                        this.MoveFirst();
                        return false;
                    }
                    else
                    {
                        this.mCurrentElement = this.GoToNextElementNode(nextSibling, true);
                        if (this.mCurrentElement == null)
                        {
                            this.MoveLast();
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new CsiClientException(-1L, ex.Message, this.GetType().FullName + ".moveNext()");
            }
        }

        public void MovePrevious()
        {
            try
            {
                if (this.mCurrentElement == null)
                {
                    this.MoveFirst();
                }
                else
                {
                    XmlNode previousSibling = this.mCurrentElement.PreviousSibling;
                    if (previousSibling == null)
                    {
                        this.MoveFirst();
                    }
                    else
                    {
                        this.mCurrentElement = this.GoToNextElementNode(previousSibling, false);
                        if (this.mCurrentElement == null)
                            this.MoveFirst();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CsiClientException(-1L, ex, this.GetType().FullName + ".movePrevious()");
            }
        }

        public virtual IEnumerable<ICsiRecordsetField> GetFields()
        {
            try
            {
                var arrayList = new List<CsiRecordsetField>();
                if (this.mCurrentElement != null)
                {
                    XmlNodeList childNodes = this.mCurrentElement.ChildNodes;
                    for (int i = 0; i < childNodes.Count; ++i)
                    {
                        XmlNode xmlNode = childNodes[i];
                        if (xmlNode.NodeType == XmlNodeType.Element)
                            arrayList.Add(new CsiRecordsetField(this.GetOwnerDocument(), xmlNode as XmlElement));
                    }
                }
                return arrayList;
            }
            catch (Exception ex)
            {
                throw new CsiClientException(-1L, ex, this.GetType().FullName + ".getFields()");
            }
        }

        private XmlNode GoToNextElementNode(XmlNode element, bool bForward)
        {
            try
            {
                XmlNode xmlNode = element;
                while (xmlNode != null && (xmlNode.NodeType != XmlNodeType.Element || !xmlNode.Name.Equals("__row")))
                    xmlNode = !bForward ? xmlNode.PreviousSibling : xmlNode.NextSibling;
                return xmlNode;
            }
            catch (Exception ex)
            {
                throw new CsiClientException(-1L, ex, this.GetType().FullName + ".GoToNextElementNode()");
            }
        }

        public System.Data.DataTable GetAsDataTable()
        {
            try
            {
                this.MoveFirst();
                var fs = this.GetFields();
                DataTable dt = new DataTable();
                dt.Columns.AddRange((from f in fs select new DataColumn(f.GetName())).ToArray());
                dt.Rows.Add((from f in fs select f.GetValue()).ToArray());
                while (this.MoveNext())
                {
                    dt.Rows.Add((from f in this.GetFields() select f.GetValue()).ToArray());
                }
                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}