// Decompiled with JetBrains decompiler
// Type: Camstar.Util.XPathNavUtil
// Assembly: Camstar.Util, Version=8.3.7535.15629, Culture=neutral, PublicKeyToken=null
// MVID: BDD78B04-B363-4ACB-8ADC-34FD2F207CCE
// Assembly location: C:\Program Files (x86)\Camstar\InSite XML Client\Camstar.Util.dll

using System;
using System.Xml.XPath;

namespace Camstar.Util
{
  public class XPathNavUtil
  {
    public static bool HasChildElements(XPathNavigator nav)
    {
      if (nav == null)
        throw new ArgumentNullException(nameof (nav));
      bool flag = false;
      if (nav.HasChildren)
      {
        nav.MoveToFirstChild();
        do
        {
          if (nav.LocalName.Length > 0)
            flag = true;
        }
        while (!flag && nav.MoveToNext());
        nav.MoveToParent();
      }
      return flag;
    }

    public static int CountChildElements(XPathNavigator nav)
    {
      if (nav == null)
        throw new ArgumentNullException(nameof (nav));
      int num = 0;
      if (nav.HasChildren)
      {
        nav.MoveToFirstChild();
        do
        {
          if (nav.NodeType == XPathNodeType.Element)
            ++num;
        }
        while (nav.MoveToNext());
        nav.MoveToParent();
      }
      return num;
    }

    public static XPathNavigator SelectSingleChild(XPathNavigator nav, string XPath)
    {
      XPathNodeIterator xpathNodeIterator = nav != null ? nav.SelectChildren(XPath, "") : throw new ArgumentNullException(nameof (nav));
      return xpathNodeIterator.Count > 0 && xpathNodeIterator.MoveNext() ? xpathNodeIterator.Current : (XPathNavigator) null;
    }

    public static XPathNavigator SelectSingleChild(
      XPathNavigator nav,
      string XPath,
      string nodeValue)
    {
      if (nav == null)
        throw new ArgumentNullException(nameof (nav));
      return XPathNavUtil.FindNavigator(nav.SelectChildren(XPath, ""), nodeValue);
    }

    public static XPathNavigator SelectSingle(XPathNavigator nav, string XPath)
    {
      XPathNodeIterator xpathNodeIterator = nav != null ? nav.Select(XPath) : throw new ArgumentNullException(nameof (nav));
      return xpathNodeIterator.Count > 0 && xpathNodeIterator.MoveNext() ? xpathNodeIterator.Current : (XPathNavigator) null;
    }

    public static XPathNavigator SelectSingle(
      XPathNavigator nav,
      string XPath,
      string nodeValue)
    {
      if (nav == null)
        throw new ArgumentNullException(nameof (nav));
      return XPathNavUtil.FindNavigator(nav.Select(XPath), nodeValue);
    }

    public static string GetChildValue(XPathNavigator nav, string childPath)
    {
      string str = (string) null;
      XPathNavigator xpathNavigator = XPathNavUtil.SelectSingleChild(nav, childPath);
      if (xpathNavigator != null)
        str = xpathNavigator.Value;
      return str;
    }

    public static string GetValue(XPathNavigator nav, string path)
    {
      string str = (string) null;
      XPathNavigator xpathNavigator = XPathNavUtil.SelectSingle(nav, path);
      if (xpathNavigator != null)
        str = xpathNavigator.Value;
      return str;
    }

    protected static XPathNavigator FindNavigator(
      XPathNodeIterator nodeIterator,
      string nodeValue)
    {
      XPathNavigator xpathNavigator = (XPathNavigator) null;
      if (nodeIterator.Count > 0)
      {
        bool flag = false;
        while (!flag && nodeIterator.MoveNext())
        {
          flag = nodeIterator.Current.Value == nodeValue;
          if (flag)
            xpathNavigator = nodeIterator.Current;
        }
      }
      return xpathNavigator;
    }
  }
}
