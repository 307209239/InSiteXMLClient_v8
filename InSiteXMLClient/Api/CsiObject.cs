using Camstar.Util;
using System;
using System.Xml;
using Camstar.XMLClient.Interface;

namespace Camstar.XMLClient.API
{
  internal class CsiObject : CsiField, ICsiObject, ICsiField, ICsiXmlElement
  {
    public CsiObject(ICsiDocument doc, XmlElement domElement)
      : base(doc, domElement)
    {
    }

    public CsiObject(ICsiDocument doc, string name, ICsiXmlElement parent)
      : base(doc, name, parent)
    {
    }

    public CsiObject(ICsiDocument doc, ICsiXmlElement parent)
      : this(doc, "__inputData", parent)
    {
    }

    public override bool IsObject() => true;

    public virtual string GetObjectId() => !(this.FindChildByName("__Id") is CsiDataField childByName) ? (string) null : childByName.GetValue();

    public virtual void SetObjectId(string Id)
    {
      if (!(this.FindChildByName("__Id") is CsiDataField csiDataFieldImpl))
        csiDataFieldImpl = new CsiDataField(this.GetOwnerDocument(), "__Id", (ICsiXmlElement) this);
      csiDataFieldImpl.SetValue(Id);
    }

    public virtual string GetObjectType() => this.GetAttribute("__CDOTypeName");

    public virtual void SetObjectType(string type) => this.SetAttribute("__CDOTypeName", type);

    public override Array GetFields() => this.GetAllChildren(true);

    public virtual Array GetUserDefinedFields() => this.FindChildByName("__userDefinedFields") is CsiXmlElement childByName ? childByName.GetAllChildren(false) : (Array) null;

    public ICsiPerform Perform(string eventName)
    {
      CsiPerform csiPerformImpl = new CsiPerform(this.GetOwnerDocument(), (ICsiXmlElement) this);
      new CsiDataField(this.GetOwnerDocument(), "__eventName", (ICsiXmlElement) csiPerformImpl).SetValue(eventName);
      return (ICsiPerform) csiPerformImpl;
    }

    public void CreateObject(string sCDOType)
    {
      this.SetAttribute("__action", "create");
      if (StringUtil.IsEmptyString(sCDOType))
        return;
      this.SetAttribute("__CDOTypeName", sCDOType);
    }

    public ICsiObject ObjectField(string fieldName) => (ICsiObject) new CsiObject(this.GetOwnerDocument(), fieldName, (ICsiXmlElement) this);

    public virtual ICsiObjectList ObjectList(string fieldName) => (ICsiObjectList) new CsiObjectList(this.GetOwnerDocument(), fieldName, (ICsiXmlElement) this);

    public ICsiDataList DataList(string listName) => (ICsiDataList) new CsiDataList(this.GetOwnerDocument(), listName, (ICsiXmlElement) this);

    public ICsiContainerList ContainerList(string listName) => (ICsiContainerList) new CsiContainerList(this.GetOwnerDocument(), listName, (ICsiXmlElement) this);

    public ICsiSubentityList SubentityList(string listName) => (ICsiSubentityList) new CsiSubentityList(this.GetOwnerDocument(), listName, (ICsiXmlElement) this);

    public ICsiNamedObjectList NamedObjectList(string listName) => (ICsiNamedObjectList) new CsiNamedObjectList(this.GetOwnerDocument(), listName, (ICsiXmlElement) this);

    public ICsiRevisionedObjectList RevisionedObjectList(string listName) => (ICsiRevisionedObjectList) new CsiRevisionedObjectList(this.GetOwnerDocument(), listName, (ICsiXmlElement) this);

    public ICsiNamedSubentityList NamedSubentityList(string listName) => (ICsiNamedSubentityList) new CsiNamedSubentityList(this.GetOwnerDocument(), listName, (ICsiXmlElement) this);

    public ICsiSubentity SubentityField(string fieldName) => (ICsiSubentity) new CsiSubentity(this.GetOwnerDocument(), fieldName, (ICsiXmlElement) this);

    public ICsiContainer ContainerField(string fieldName) => (ICsiContainer) new CsiContainer(this.GetOwnerDocument(), fieldName, (ICsiXmlElement) this);

    public ICsiNamedSubentity NamedSubentityField(string objectName) => (ICsiNamedSubentity) new CsiNamedSubentity(this.GetOwnerDocument(), objectName, (ICsiXmlElement) this);

    public ICsiDataField DataField(string dataFieldName) => (ICsiDataField) new CsiDataField(this.GetOwnerDocument(), dataFieldName, (ICsiXmlElement) this);

    public ICsiNamedObject NamedObjectField(string fieldName) => (ICsiNamedObject) new CsiNamedObject(this.GetOwnerDocument(), fieldName, (ICsiXmlElement) this);

    public ICsiRevisionedObject RevisionedObjectField(string fieldName) => (ICsiRevisionedObject) new CsiRevisionedObject(this.GetOwnerDocument(), fieldName, (ICsiXmlElement) this);

    public ICsiField GetField(string tagName) => (ICsiField) (this.FindChildByName(tagName) as CsiField);
  }
}
