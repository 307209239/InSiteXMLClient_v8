using Camstar.XMLClient.Enum;
using Camstar.XMLClient.Interface;
using System;
using System.Collections;
using System.Xml;

namespace Camstar.XMLClient.API
{
    internal class CsiList : CsiField, ICsiList, ICsiField, ICsiXmlElement
    {
        public CsiList(ICsiDocument doc, XmlElement domElement): base(doc, domElement)
        {
        }

        public CsiList(ICsiDocument doc, string name, ICsiXmlElement parent): base(doc, name, parent)
        {
        }

        public override bool IsList() => true;

        public virtual void SetListAction(ListActions action)
        {
            string val = "change";
            switch (action)
            {
                case ListActions.ListActionChange:
                    this.SetAttribute("__listAction", val);
                    break;

                case ListActions.ListActionReplace:
                    val = "replace";
                    goto case ListActions.ListActionChange;
                default:
                    throw new CsiClientException(-2147467259L, this.GetType().FullName + ".setListAction()");
            }
        }

        public virtual ICsiXmlElement[] GetListItems()
        {
            var childrenByName = CsiXmlHelper.GetChildrenByName(this.GetOwnerDocument(), this.GetDomElement(), "__listItem");
            return childrenByName.Length > 0 ? childrenByName : null;
        }

        public virtual void SetProxyField(string val) => this.SetAttribute("__proxyField", val);

        public virtual void DeleteItemByIndex(int index)
        {
            ICsiXmlElement sourceElement = (ICsiXmlElement)new CsiXmlElement(this.GetOwnerDocument(), "__listItem", (ICsiXmlElement)this);
            sourceElement.SetAttribute("__listItemAction", "delete");
            CsiXmlHelper.FindCreateSetValue(sourceElement, "__index", Convert.ToString(index));
        }

        protected internal virtual CsiXmlElement GetItem(int index)
        {
            IEnumerator enumerator = this.GetListItems().GetEnumerator();
            int num = 0;
            while (enumerator.MoveNext())
            {
                CsiXmlElement current = enumerator.Current as CsiXmlElement;
                if (num++ == index)
                    return current;
            }
            return (CsiXmlElement)null;
        }
    }
}