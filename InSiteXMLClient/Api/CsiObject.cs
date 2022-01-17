using Camstar.Util;
using Camstar.XMLClient.Interface;
using System;
using System.Xml;

namespace Camstar.XMLClient.API
{
    internal class CsiObject : CsiField, ICsiObject, ICsiField, ICsiXmlElement
    {
        public CsiObject(ICsiDocument doc, XmlElement domElement) : base(doc, domElement)
        {
        }

        public CsiObject(ICsiDocument doc, string name, ICsiXmlElement parent) : base(doc, name, parent)
        {
        }

        public CsiObject(ICsiDocument doc, ICsiXmlElement parent) : this(doc, "__inputData", parent)
        {
        }

        public override bool IsObject() => true;

        public virtual string GetObjectId() => !(this.FindChildByName("__Id") is CsiDataField childByName) ? null : childByName.GetValue();

        public virtual void SetObjectId(string Id)
        {
            if (!(this.FindChildByName("__Id") is CsiDataField csiDataFieldImpl))
                csiDataFieldImpl = new CsiDataField(this.GetOwnerDocument(), "__Id", this);
            csiDataFieldImpl.SetValue(Id);
        }

        public virtual string GetObjectType() => this.GetAttribute("__CDOTypeName");

        public virtual void SetObjectType(string type) => this.SetAttribute("__CDOTypeName", type);

        public override ICsiField[] GetFields() => this.GetAllChildren(true) as ICsiField[];

        public virtual ICsiXmlElement[] GetUserDefinedFields() => this.FindChildByName("__userDefinedFields") is CsiXmlElement childByName ? childByName.GetAllChildren(false) : null;

        public ICsiPerform Perform(string eventName)
        {
            CsiPerform csiPerformImpl = new CsiPerform(this.GetOwnerDocument(), this);
            new CsiDataField(this.GetOwnerDocument(), "__eventName", csiPerformImpl).SetValue(eventName);
            return csiPerformImpl;
        }

        public void CreateObject(string sCDOType)
        {
            this.SetAttribute("__action", "create");
            if (StringUtil.IsEmptyString(sCDOType))
                return;
            this.SetAttribute("__CDOTypeName", sCDOType);
        }

        public ICsiObject ObjectField(string fieldName) => new CsiObject(this.GetOwnerDocument(), fieldName, this);

        public virtual ICsiObjectList ObjectList(string fieldName) => new CsiObjectList(this.GetOwnerDocument(), fieldName, this);

        public ICsiDataList DataList(string listName) => new CsiDataList(this.GetOwnerDocument(), listName, this);

        public ICsiContainerList ContainerList(string listName) => new CsiContainerList(this.GetOwnerDocument(), listName, this);

        public ICsiSubentityList SubentityList(string listName) => new CsiSubentityList(this.GetOwnerDocument(), listName, this);

        public ICsiNamedObjectList NamedObjectList(string listName) => (ICsiNamedObjectList)new CsiNamedObjectList(this.GetOwnerDocument(), listName, this);

        public ICsiRevisionedObjectList RevisionedObjectList(string listName) => (ICsiRevisionedObjectList)new CsiRevisionedObjectList(this.GetOwnerDocument(), listName, this);

        public ICsiNamedSubentityList NamedSubentityList(string listName) => (ICsiNamedSubentityList)new CsiNamedSubentityList(this.GetOwnerDocument(), listName, this);

        public ICsiSubentity SubentityField(string fieldName) => (ICsiSubentity)new CsiSubentity(this.GetOwnerDocument(), fieldName, this);

        public ICsiContainer ContainerField(string fieldName) => (ICsiContainer)new CsiContainer(this.GetOwnerDocument(), fieldName, this);

        public ICsiNamedSubentity NamedSubentityField(string objectName) => (ICsiNamedSubentity)new CsiNamedSubentity(this.GetOwnerDocument(), objectName, this);

        public ICsiDataField DataField(string dataFieldName) => (ICsiDataField)new CsiDataField(this.GetOwnerDocument(), dataFieldName, this);

        public ICsiNamedObject NamedObjectField(string fieldName) => (ICsiNamedObject)new CsiNamedObject(this.GetOwnerDocument(), fieldName, this);

        public ICsiRevisionedObject RevisionedObjectField(string fieldName) => (ICsiRevisionedObject)new CsiRevisionedObject(this.GetOwnerDocument(), fieldName, this);

        public ICsiField GetField(string tagName) => (ICsiField)(this.FindChildByName(tagName) as CsiField);
    }
}