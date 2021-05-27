using System;
using System.Collections;
using System.Xml;
using Camstar.XMLClient.Interface;

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

    public void MoveNext()
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
          }
          else
          {
            this.mCurrentElement = this.GoToNextElementNode(nextSibling, true);
            if (this.mCurrentElement == null)
              this.MoveLast();
          }
        }
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

    public virtual Array GetFields()
    {
      try
      {
        ArrayList arrayList = new ArrayList();
        if (this.mCurrentElement != null)
        {
          XmlNodeList childNodes = this.mCurrentElement.ChildNodes;
          for (int i = 0; i < childNodes.Count; ++i)
          {
            XmlNode xmlNode = childNodes[i];
            if (xmlNode.NodeType == XmlNodeType.Element)
              arrayList.Add((object) new CsiRecordsetField(this.GetOwnerDocument(), xmlNode as XmlElement));
          }
        }
        return (Array) arrayList.ToArray();
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
  }
}
