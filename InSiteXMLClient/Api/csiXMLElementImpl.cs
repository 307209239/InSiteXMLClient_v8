using Camstar.Util;
using System;
using System.Collections;
using System.Xml;
using Camstar.XMLClient.Interface;

namespace Camstar.XMLClient.API
{
  internal class CsiXmlElement : ICsiXmlElement
  {
    private ICsiDocument mDocument = (ICsiDocument) null;
    private XmlElement mDOMElement;

    public CsiXmlElement(ICsiDocument doc, XmlElement domElement)
    {
      this.mDocument = doc;
      this.mDOMElement = domElement;
    }

    public CsiXmlElement(ICsiDocument document, string tagName, ICsiXmlElement parentElement)
    {
      if (StringUtil.IsEmptyString(tagName))
        throw new CsiClientException(3014678L, this.GetType().FullName + ".csiXMLElement()");
      this.mDocument = document;
      string str = tagName;
      ICsiXmlElement csiXmlElement = parentElement;
      while (str != null)
      {
        int length = str.IndexOf(".");
        if (length > 0)
        {
          tagName = str.Substring(0, length);
          str = str.Substring(length + 1);
        }
        else
        {
          tagName = str;
          str = (string) null;
        }
        this.mDOMElement = (document as CsiDocument).CreateDomElement(tagName);
        CsiXmlElement csiXmlElementImpl = new CsiXmlElement(document, this.mDOMElement);
        csiXmlElement.AppendChild((ICsiXmlElement) csiXmlElementImpl);
        csiXmlElement = (ICsiXmlElement) csiXmlElementImpl;
      }
    }

    public virtual ICsiDocument GetOwnerDocument() => this.mDocument;

    public virtual ICsiXmlElement GetParentElement()
    {
      XmlNode parentNode = this.mDOMElement.ParentNode;
      return parentNode != null && parentNode.NodeType == XmlNodeType.Element ? CsiXmlHelper.CreateCsiElement(this.GetOwnerDocument(), parentNode as XmlElement) : (ICsiXmlElement) null;
    }

    public virtual bool IsField() => false;

    public virtual bool IsObject() => false;

    public virtual bool IsContainer() => false;

    public virtual bool IsList() => false;

    public virtual bool IsNamedObject() => false;

    public virtual bool IsService() => false;

    public virtual bool IsRevisionedObject() => false;

    public virtual bool IsRequestData() => false;

    public virtual bool IsDataField() => false;

    public virtual bool IsSubentity() => false;

    public virtual bool IsDataList() => false;

    public virtual bool IsContainerList() => false;

    public virtual bool IsNamedObjectList() => false;

    public virtual bool IsRevisionedObjectList() => false;

    public virtual bool IsSubentityList() => false;

    public virtual bool IsNamedSubentityList() => false;

    public virtual bool IsNamedSubentity() => false;

    public virtual bool IsObjectList() => false;

    public ICsiField AsField() => this as ICsiField;

    public ICsiDataField AsDataField() => this as ICsiDataField;

    public ICsiList AsList() => this as ICsiList;

    public ICsiObject AsObject() => this as ICsiObject;

    public ICsiService AsService() => this as ICsiService;

    public ICsiContainer AsContainer() => this as ICsiContainer;

    public ICsiRevisionedObject AsRevisionedObject() => this as ICsiRevisionedObject;

    public ICsiNamedObject AsNamedObject() => this as ICsiNamedObject;

    public ICsiSubentity AsSubentity() => this as ICsiSubentity;

    public ICsiRequestData AsRequestData() => this as ICsiRequestData;

    public ICsiDataList AsDataList() => this as ICsiDataList;

    public ICsiObjectList AsObjectList() => this as ICsiObjectList;

    public ICsiContainerList AsContainerList() => this as ICsiContainerList;

    public ICsiNamedObjectList AsNamedObjectList() => this as ICsiNamedObjectList;

    public ICsiSubentityList AsSubentityList() => this as ICsiSubentityList;

    public ICsiRevisionedObjectList AsRevisionedObjectList() => this as ICsiRevisionedObjectList;

    public ICsiNamedSubentityList AsNamedSubentityList() => this as ICsiNamedSubentityList;

    public ICsiNamedSubentity AsNamedSubentity() => this as ICsiNamedSubentity;

    public virtual string GetElementName() => this.mDOMElement.Name;

    public void SetAttribute(string key, string val)
    {
      try
      {
        this.mDOMElement.SetAttribute(key, val);
      }
      catch (XmlException ex)
      {
        throw new CsiClientException(-1L, (Exception) ex, this.GetType().FullName + ".setAttribute()");
      }
    }

    public string GetAttribute(string key) => this.mDOMElement.GetAttribute(key);

    public ICsiXmlElement FindChildByName(string tagName)
    {
        ICsiXmlElement csiXmlElement = (ICsiXmlElement) null;
      string[] stringList = StringUtil.GetStringList(tagName, '.');
      XmlElement element = this.RecursiveGetElement(this.GetDomElement(), stringList);
      if (element != null)
        csiXmlElement = CsiXmlHelper.CreateCsiElement(this.GetOwnerDocument(), element);
      return csiXmlElement;
    }

    public ICsiXmlElement AppendChild(ICsiXmlElement child)
    {
      CsiXmlElement csiXmlElementImpl = (CsiXmlElement) child;
      try
      {
        if (this.mDOMElement.OwnerDocument == csiXmlElementImpl.GetDomElement().OwnerDocument)
          this.mDOMElement.AppendChild((XmlNode) csiXmlElementImpl.GetDomElement());
      }
      catch (Exception ex)
      {
        throw new CsiClientException(-1L, ex, this.GetType().FullName + ".appendChild()");
      }
      return child;
    }

    public ICsiXmlElement RemoveChild(ICsiXmlElement child)
    {
      CsiXmlElement csiXmlElementImpl = (CsiXmlElement) child;
      this.mDOMElement.RemoveChild((XmlNode) csiXmlElementImpl.GetDomElement());
      return (ICsiXmlElement) csiXmlElementImpl;
    }

    public void RemoveAllChildren()
    {
      try
      {
        while (this.mDOMElement.HasChildNodes)
          this.mDOMElement.RemoveChild(this.mDOMElement.FirstChild);
      }
      catch (Exception ex)
      {
        throw new CsiClientException(-1L, ex, this.GetType().FullName + ".removeAllChildren()");
      }
    }

    public bool HasChildren()
    {
      if (this.GetDomElement().HasChildNodes)
      {
        for (XmlNode xmlNode = this.GetDomElement().FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
        {
          if (xmlNode.NodeType != XmlNodeType.Text && xmlNode.NodeType != XmlNodeType.CDATA)
            return true;
        }
      }
      return false;
    }

    public Array GetChildrenByName(string name) => CsiXmlHelper.GetChildrenByName(this.GetOwnerDocument(), this.GetDomElement(), name);

    public virtual Array GetAllChildren() => this.GetAllChildren(false);

    protected internal XmlElement GetDomElement() => this.mDOMElement;

    protected internal Array GetAllChildren(bool bDoNotIncludeTagsWith__)
    {
      ArrayList arrayList = new ArrayList();
      for (XmlNode xmlNode = this.mDOMElement.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
      {
        if (xmlNode.NodeType == XmlNodeType.Element && (!bDoNotIncludeTagsWith__ || !xmlNode.Name.StartsWith("__")))
          arrayList.Add((object) CsiXmlHelper.CreateCsiElement(this.GetOwnerDocument(), (XmlElement) xmlNode));
      }
      return (Array) arrayList.ToArray();
    }

    protected internal virtual string GetElementValue()
    {
      for (XmlNode xmlNode = this.GetDomElement().FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
      {
        if (xmlNode.NodeType == XmlNodeType.Text || xmlNode.NodeType == XmlNodeType.CDATA)
          return xmlNode.Value;
      }
      return (string) null;
    }

    protected virtual void RemoveChildByName(ICsiXmlElement element, string name)
    {
      if (StringUtil.IsEmptyString(name))
      {
        element.RemoveAllChildren();
      }
      else
      {
          ICsiXmlElement childByName = element.FindChildByName(name);
        if (childByName != null)
          element.RemoveChild(childByName);
      }
    }

    protected virtual ICsiXmlElement FindChildByName(
      string firstLevelChildTagName,
      string secondLevelChildTagName,
      string nameText)
    {
      foreach (object obj in CsiXmlHelper.GetChildrenByName(this.GetOwnerDocument(), this.GetDomElement(), firstLevelChildTagName))
      {
          ICsiXmlElement csiXmlElement = obj as ICsiXmlElement;
        if (csiXmlElement.FindChildByName(secondLevelChildTagName) is CsiXmlElement childByName)
        {
          string str = childByName.GetDomElement().Value ?? childByName.GetElementValue();
          if (nameText.Equals(str))
            return csiXmlElement;
        }
      }
      return null;
    }

    protected virtual ICsiExceptionData GetChildExceptionData() => this.FindChildByName("__responseData.__exceptionData") as ICsiExceptionData;

    protected ICsiXmlElement ClearXmlElementChildByName(string parent, string child)
    {
        ICsiXmlElement element = this.FindChildByName(parent);
      if (element == null)
        element = (ICsiXmlElement) new CsiXmlElement(this.GetOwnerDocument(), parent, (ICsiXmlElement) this);
      else
        this.RemoveChildByName(element, child);
      return element;
    }

    protected ICsiXmlElement ClearXmlElementChildByName(string parent) => this.ClearXmlElementChildByName(parent, (string) null);

    protected ICsiRequestField RequestForField(string fieldName)
    {
      CsiRequestField requestFieldImpl = (CsiRequestField) null;
      string[] stringList = StringUtil.GetStringList(fieldName, '.');
      CsiXmlElement csiXmlElementImpl = this;
      for (int index = 0; index < stringList.Length; ++index)
      {
        XmlElement element = this.GetElement(csiXmlElementImpl.GetDomElement(), stringList[index]);
        if (element != null)
        {
          requestFieldImpl = new CsiRequestField(this.GetOwnerDocument(), element);
        }
        else
        {
          requestFieldImpl = new CsiRequestField(this.GetOwnerDocument(), stringList[index], (ICsiXmlElement) csiXmlElementImpl);
          csiXmlElementImpl.AppendChild((ICsiXmlElement) requestFieldImpl);
        }
        csiXmlElementImpl = (CsiXmlElement) requestFieldImpl;
      }
      return (ICsiRequestField) requestFieldImpl;
    }

    private ArrayList GetImmediateChildren(XmlElement element)
    {
      ArrayList arrayList = new ArrayList();
      for (XmlNode xmlNode = element.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
        arrayList.Add((object) xmlNode);
      return arrayList;
    }

    private XmlElement GetElement(XmlElement parent, string tag)
    {
      ArrayList immediateChildren = this.GetImmediateChildren(parent);
      int length = tag.IndexOf('[');
      if (length != -1)
      {
        tag.Substring(0, length);
        int index = int.Parse(tag.Substring(length + 1, tag.IndexOf(']') - (length + 1)));
        if (tag.Equals(((XmlNode) immediateChildren[index]).Name) && immediateChildren[index] is XmlElement)
          return (XmlElement) immediateChildren[index];
      }
      else
      {
        for (int index = 0; index < immediateChildren.Count; ++index)
        {
          if (immediateChildren[index] is XmlElement)
          {
            XmlElement xmlElement = (XmlElement) immediateChildren[index];
            if (tag.Equals(xmlElement.Name))
              return xmlElement;
          }
        }
      }
      return (XmlElement) null;
    }

    private XmlElement RecursiveGetElement(XmlElement sourceElement, string[] tagsList)
    {
      XmlElement parent = sourceElement;
      if (tagsList.Length == 0)
        return (XmlElement) null;
      for (int index = 0; index < tagsList.Length; ++index)
      {
        parent = this.GetElement(parent, tagsList[index]);
        if (parent == null || index == tagsList.Length - 1)
          return parent;
      }
      return (XmlElement) null;
    }
  }
}
