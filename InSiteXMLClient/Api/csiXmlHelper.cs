using Camstar.Util;
using System;
using System.Collections;
using System.Reflection;
using System.Security;
using System.Xml;
using Camstar.XMLClient.Interface;

namespace Camstar.XMLClient.API
{
  internal sealed class CsiXmlHelper
  {
    private const string mkFailToCreateElement = "Failed to create a <{0}> element.\n";
    private const string mkElementDoesNotExist = "Element <{0}> does not exist.\n";
    private const string mkIndexIsOutOfBound = "Index {0} is out of bound\n";
    private const string mkInvalidElement = "The element <{0}> is invalid.";

    private CsiXmlHelper()
    {
    }

    public static string GetCreateFailed(string sObject) => $"Failed to create a <{(object) sObject}> element.\n";

    public static string GetNotExists(string sObject) => $"Element <{(object) sObject}> does not exist.\n";

    public static string GetIndexOufOfBound(int index) => $"Index {(object) index} is out of bound\n";

    public static string GetInvalidElement(string sObject) => $"The element <{(object) sObject}> is invalid.";

    public static string GenerateGuid() => Guid.NewGuid().ToString();

    public static ICsiXmlElement CreateCsiElement(
      ICsiDocument document,
      XmlElement element)
    {
      string typeName = (string) null;
      string str = (string) null;
      try
      {
        str = element.Name;
        string data = element.GetAttribute("__nodeType");
        if (StringUtil.IsEmptyString(data))
          data = element.GetAttribute("__type");
        if (StringUtil.IsEmptyString(data))
        {
          XmlElement parentNode = (XmlElement) element.ParentNode;
          if (parentNode != null)
          {
            string attribute = parentNode.GetAttribute("__nodeType");
            if (!StringUtil.IsEmptyString(attribute))
            {
              if (str.Equals("__listItem"))
              {
                if (attribute.EndsWith("List"))
                  data = attribute.Substring(0, attribute.Length - "List".Length);
              }
              else if (str.Equals("__defaultValue") && "__label".Equals(parentNode.Name))
                data = attribute;
            }
          }
        }
        if (!StringUtil.IsEmptyString(data))
          typeName = CsiConstants.NodeToClassMapping[(object) data] as string;
        if (typeName == null)
          typeName = CsiConstants.NodeToClassMapping[(object) str] as string;
        if (typeName == null)
          typeName = "Camstar.XMLClient.API.csiXMLElementImpl";
        return (ICsiXmlElement) Type.GetType(typeName).GetConstructor(new Type[2]
        {
          typeof (ICsiDocument),
          typeof (XmlElement)
        }).Invoke(new object[2]
        {
          (object) document,
          (object) element
        });
      }
      catch (TypeLoadException ex)
      {
        throw new CsiClientException(-1L, (Exception) ex, string.Format("csiXmlHelper.createcsiElementment() - TypeLoadException: Tag = {0}, className = {1}", (object) str, (object) typeName));
      }
      catch (MethodAccessException ex)
      {
        throw new CsiClientException(-1L, (Exception) ex, string.Format("csiXmlHelper.createcsiElementment() - MethodAccessException: Tag = {0}, className = {1}", (object) str, (object) typeName));
      }
      catch (SecurityException ex)
      {
        throw new CsiClientException(-1L, (Exception) ex, string.Format("csiXmlHelper.createcsiElementment() - SecurityException: Tag = {0}, className = {1}", (object) str, (object) typeName));
      }
      catch (UnauthorizedAccessException ex)
      {
        throw new CsiClientException(-1L, (Exception) ex, string.Format("csiXmlHelper.createcsiElementment() - UnauthorizedAccessException: Tag = {0}, className = {1}", (object) str, (object) typeName));
      }
      catch (ArgumentException ex)
      {
        throw new CsiClientException(-1L, (Exception) ex, string.Format("csiXmlHelper.createcsiElementment() - ArgumentException: Tag = {0}, className = {1}", (object) str, (object) typeName));
      }
      catch (TargetInvocationException ex)
      {
        throw new CsiClientException(-1L, (Exception) ex, string.Format("csiXmlHelper.createcsiElementment() - TargetInvocationException: Tag = {0}, className = {1}", (object) str, (object) typeName));
      }
      catch (Exception ex)
      {
        throw new CsiClientException(-1L, ex, string.Format("csiXmlHelper.createcsiElementment(): Tag = {0}, className = {1}", (object) str, (object) typeName));
      }
    }

    public static Array GetChildrenByName(
      ICsiDocument document,
      XmlElement element,
      string name)
    {
      try
      {
        ArrayList arrayList = new ArrayList();
        for (XmlNode xmlNode = element.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
        {
          if (xmlNode.NodeType == XmlNodeType.Element && xmlNode.Name.Equals(name))
            arrayList.Add((object) CsiXmlHelper.CreateCsiElement(document, (XmlElement) xmlNode));
        }
        return (Array) arrayList.ToArray();
      }
      catch (Exception ex)
      {
        throw new CsiClientException(-1L, ex, "csiXmlHelper.getChildrenByName()");
      }
    }

    public static ICsiXmlElement FindCreateSetValue(
        ICsiXmlElement sourceElement,
      string tagName,
      string val,
      bool isCDATA)
    {
      CsiXmlElement csiXmlElementImpl = (CsiXmlElement) sourceElement.FindChildByName(tagName);
      if (csiXmlElementImpl == null)
      {
        csiXmlElementImpl = new CsiXmlElement(sourceElement.GetOwnerDocument(), tagName, sourceElement);
        if (csiXmlElementImpl == null)
        {
          string src = "csiXmlHelper.findCreateSetName()";
          throw new CsiClientException(-1L, CsiXmlHelper.GetCreateFailed(tagName), src);
        }
      }
      if (isCDATA && !StringUtil.IsEmptyString(val))
        CsiXmlHelper.SetCdataNode(csiXmlElementImpl.GetDomElement(), val);
      else
        CsiXmlHelper.SetTextNode(csiXmlElementImpl.GetDomElement(), val);
      return (ICsiXmlElement) csiXmlElementImpl;
    }

    public static void SetTextNode(XmlElement target, string data)
    {
      try
      {
        if (target == null)
          return;
        XmlNode nextSibling;
        for (XmlNode oldChild = target.FirstChild; oldChild != null; oldChild = nextSibling)
        {
          nextSibling = oldChild.NextSibling;
          if (oldChild.NodeType == XmlNodeType.Text || oldChild.NodeType == XmlNodeType.CDATA)
            target.RemoveChild(oldChild);
        }
        if (!StringUtil.IsEmptyString(data))
        {
          target.AppendChild((XmlNode) target.OwnerDocument.CreateTextNode(data));
          target.RemoveAttribute("__empty");
        }
        else
          target.RemoveAttribute("__empty");
      }
      catch (Exception ex)
      {
        throw new CsiClientException(-1L, ex, "csiXmlHelper.setTextNode()");
      }
    }

    public static void SetCdataNode(XmlElement target, string data)
    {
      try
      {
        if (target == null || StringUtil.IsEmptyString(data))
          return;
        XmlNode nextSibling;
        for (XmlNode oldChild = target.FirstChild; oldChild != null; oldChild = nextSibling)
        {
          nextSibling = oldChild.NextSibling;
          if (oldChild.NodeType == XmlNodeType.Text || oldChild.NodeType == XmlNodeType.CDATA)
            target.RemoveChild(oldChild);
        }
        target.AppendChild((XmlNode) target.OwnerDocument.CreateCDataSection(data));
      }
      catch (Exception ex)
      {
        throw new CsiClientException(-1L, ex, "csiXmlHelper.setTextNode()");
      }
    }

    public static ICsiXmlElement FindCreateSetValue(
        ICsiXmlElement sourceElement,
      string tagName,
      string val)
    {
      return CsiXmlHelper.FindCreateSetValue(sourceElement, tagName, val, false);
    }

    public static ICsiXmlElement FindCreateSetValue2(
        ICsiXmlElement sourceElement,
      string firstLevelTag,
      string secondLevelTag,
      string val,
      bool isCDATA)
    {
        ICsiXmlElement sourceElement1 = sourceElement.FindChildByName(firstLevelTag);
      if (sourceElement1 == null)
      {
        sourceElement1 = (ICsiXmlElement) new CsiXmlElement(sourceElement.GetOwnerDocument(), firstLevelTag, sourceElement);
        if (sourceElement1 == null)
        {
          string src = "csiXmlHelper.findCreateSetName2()";
          throw new CsiClientException(-1L, CsiXmlHelper.GetCreateFailed(firstLevelTag), src);
        }
      }
      return CsiXmlHelper.FindCreateSetValue(sourceElement1, secondLevelTag, val, isCDATA);
    }

    public static ICsiXmlElement FindCreateSetValue2(
        ICsiXmlElement sourceElement,
      string firstLevelTag,
      string secondLevelTag,
      string val)
    {
      return CsiXmlHelper.FindCreateSetValue2(sourceElement, firstLevelTag, secondLevelTag, val, false);
    }

    public static ICsiXmlElement FindCreateSetValue3(
        ICsiXmlElement sourceElement,
      string firstLevelTag,
      string secondLevelTag,
      string thirdLevelTag,
      string val,
      bool isCDATA)
    {
      return CsiXmlHelper.FindCreateSetValue(CsiXmlHelper.FindCreateSetValue2(sourceElement, firstLevelTag, secondLevelTag, (string) null, false), thirdLevelTag, val, isCDATA);
    }

    public static void FindCreateSetValue3(
        ICsiXmlElement sourceElement,
      string firstLevelTag,
      string secondLevelTag,
      string thirdLevelTag,
      string val)
    {
      CsiXmlHelper.FindCreateSetValue3(sourceElement, firstLevelTag, secondLevelTag, thirdLevelTag, val, false);
    }

    public static string GetFirstTextNodeValue(CsiXmlElement csiElement)
    {
      string empty = string.Empty;
      if (csiElement != null)
      {
        for (XmlNode xmlNode = csiElement.GetDomElement().FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
        {
          if (xmlNode.NodeType == XmlNodeType.Text || xmlNode.NodeType == XmlNodeType.CDATA)
            empty = xmlNode.Value;
        }
      }
      return empty;
    }

    public static long GetChildCount(XmlElement element)
    {
      try
      {
        XmlNodeList childNodes = element.ChildNodes;
        long num = 0;
        for (int i = 0; i < childNodes.Count; ++i)
        {
          if (childNodes[i].NodeType == XmlNodeType.Element)
            ++num;
        }
        return num;
      }
      catch (Exception ex)
      {
        throw new CsiClientException(-1L, ex, "csiXmlHelper.getChildCount()");
      }
    }
  }
}
