using System;
using System.Collections;
using System.Xml;
using Camstar.XMLClient.Interface;

namespace Camstar.XMLClient.API
{
  internal class CsiMetaData : 
    CsiXmlElement,
    ICsiMetaData,
    ICsiCdoType,
    ICsiCdoDefinition,
    ICsiFieldDefinition,
    ICsiXmlElement,
    ICsiFieldDefinitions,
    ICsiLabel
  {
    public CsiMetaData(ICsiDocument doc, XmlElement element)
      : base(doc, element)
    {
    }

    public CsiMetaData(ICsiDocument doc, ICsiXmlElement parent)
      : base(doc, "__metadata", parent)
    {
    }

    public ICsiCdoType GetCdoType()
    {
      if (!(this.FindChildByName("__CDOSubTypes") is ICsiCdoType childByName))
        childByName = this.FindChildByName("__CDOType") as ICsiCdoType;
      if (childByName == null)
        childByName = this.FindChildByName("__field.__CDOType") as ICsiCdoType;
      return childByName;
    }

    public virtual ICsiCdoDefinition GetCdoDefinition() => this.FindChildByName("__CDODefinition") as ICsiCdoDefinition;

    public virtual ICsiCdoType GetFieldCdoType() => this.FindChildByName("__CDOType") as ICsiCdoType;

    public virtual ICsiLabel GetCdoLabel() => this.FindChildByName("__CDODefinition.__label") as ICsiLabel;

    public virtual ICsiLabel GetLabel() => (this.FindChildByName("__label") ?? this.FindChildByName("__field.__label")) as ICsiLabel;

    public virtual ICsiLabel GetFieldLabel() => this.FindChildByName("__label") as ICsiLabel;

    public virtual ICsiQueryParameters GetQueryParameters() => this.FindChildByName("__queryParameters") as ICsiQueryParameters;

    public virtual ICsiFieldDefinition GetCdoField() => this.FindChildByName("__fieldDef.__field") as ICsiFieldDefinition;

    public virtual Array GetUserDefinedFields()
    {
      CsiMetaData childByName = this.FindChildByName("__CDODefinition.__fieldDefs.__userDefinedFields") as CsiMetaData;
      ArrayList arrayList = new ArrayList();
      if (childByName != null)
      {
        foreach (object obj in childByName.GetChildrenByName("__userDefinedField"))
          arrayList.Add(obj);
      }
      return (Array) arrayList.ToArray();
    }

    public virtual void RequestCdoSubTypesByName(string baseName, bool recursive) => this.RequestCdoSubTypes("__CDOTypeName", baseName, recursive);

    public virtual void RequestCdoSubTypesById(int id, bool recurse) => this.RequestCdoSubTypes("__CDOTypeId", Convert.ToString(id), recurse);

    public virtual void RequestCdoDefinitionByName(string typeName) => CsiXmlHelper.FindCreateSetValue(this.ClearXmlElementChildByName("__CDODefinition"), "__CDOTypeName", typeName);

    public virtual void RequestCdoDefinitionById(int id) => CsiXmlHelper.FindCreateSetValue(this.ClearXmlElementChildByName("__CDODefinition"), "__CDOTypeId", Convert.ToString(id));

    public virtual void RequestCdoDefinition() => this.ClearXmlElementChildByName("__CDODefinition");

    public virtual void RequestCdoDefinitionFieldByName(string typeName, string fieldName)
    {
      ICsiXmlElement csiXmlElement = this.ClearXmlElementChildByName("__CDODefinition");
      CsiXmlHelper.FindCreateSetValue(csiXmlElement, "__CDOTypeName", typeName);
      new CsiXmlElement(this.GetOwnerDocument(), "__field", csiXmlElement).SetAttribute("__name", fieldName);
    }

    public virtual void RequestLabelById(int labelId) => CsiXmlHelper.FindCreateSetValue(this.ClearXmlElementChildByName("__label"), "__Id", Convert.ToString(labelId));

    public virtual void RequestQueryParameters(string queryName)
    {
      CsiQueryParameters queryParametersImpl = new CsiQueryParameters(this.GetOwnerDocument(), queryName, (ICsiXmlElement) this);
    }

    public virtual void RequestFieldItem(string itemName) => new CsiXmlElement(this.GetOwnerDocument(), "__field", (ICsiXmlElement) this).SetAttribute("__name", itemName);

    public virtual void RequestFieldLabel() => this.ClearXmlElementChildByName("__label");

    public virtual void RequestCdoLabel()
    {
      ICsiXmlElement parentElement = this.ClearXmlElementChildByName("__CDODefinition");
      CsiXmlElement csiXmlElementImpl = new CsiXmlElement(this.GetOwnerDocument(), "__label", parentElement);
    }

    public virtual void RequestUserDefinedFields()
    {
      ICsiXmlElement parentElement = this.ClearXmlElementChildByName("__CDODefinition");
      CsiXmlElement csiXmlElementImpl = new CsiXmlElement(this.GetOwnerDocument(), "__userDefinedFields", parentElement);
    }

    public virtual void RequestLabelByName(string name) => CsiXmlHelper.FindCreateSetValue(this.ClearXmlElementChildByName("__label"), "__name", name);

    public virtual string GetCdoTypeName()
    {
      if ("__CDODefinition".Equals(this.GetElementName()))
      {
        ICsiXmlElement childByName = this.FindChildByName("__CDOTypeName");
        if (childByName != null)
          return (childByName as CsiXmlElement).GetElementValue();
      }
      else
      {
        ICsiXmlElement childByName = this.FindChildByName("__name");
        if (childByName != null)
          return (childByName as CsiXmlElement).GetElementValue();
      }
      return (string) null;
    }

    public virtual int GetCdoTypeId() => "__CDODefinition".Equals(this.GetElementName()) ? int.Parse((this.FindChildByName("__CDOTypeId") as CsiXmlElement).GetElementValue().Trim()) : int.Parse((this.FindChildByName("__Id") as CsiXmlElement).GetElementValue().Trim());

    public virtual ICsiFieldDefinitions GetFieldDefinitions()
    {
      try
      {
        return this.FindChildByName("__fieldDefs") as ICsiFieldDefinitions;
      }
      catch (Exception ex)
      {
        return  null;
      }
    }

    public virtual string GetFieldName() => this.FindChildByName("__name") is CsiXmlElement childByName ? childByName.GetElementValue() : string.Empty;

    public virtual int GetFieldId()
    {
      try
      {
        if (this.FindChildByName("__Id") is CsiXmlElement childByName)
          return int.Parse(childByName.GetElementValue());
      }
      catch (Exception ex)
      {
      }
      return -1;
    }

    public virtual string GetDataType() => this.FindChildByName("__dataType") is CsiXmlElement childByName ? childByName.GetElementValue() : string.Empty;

    public virtual bool IsHidden()
    {
      try
      {
        return "true".Equals(((CsiXmlElement) this.FindChildByName("__isHidden")).GetElementValue());
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public virtual bool IsReadOnly()
    {
      try
      {
        return "true".Equals(((CsiXmlElement) this.FindChildByName("__isReadOnly")).GetElementValue());
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public virtual bool IsListField()
    {
      try
      {
        return "true".Equals(((CsiXmlElement) this.FindChildByName("__isList")).GetElementValue());
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public virtual bool IsObjectField()
    {
      try
      {
        return "true".Equals(((CsiXmlElement) this.FindChildByName("__isObject")).GetElementValue());
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public virtual bool IsRequired()
    {
      try
      {
        return "true".Equals(((CsiXmlElement) this.FindChildByName("__isRequired")).GetElementValue());
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public virtual bool IsValueRequired()
    {
      try
      {
        return "true".Equals(((CsiXmlElement) this.FindChildByName("__isValueRequired")).GetElementValue());
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public virtual bool IsUserDefinedField()
    {
      try
      {
        return "true".Equals(((CsiXmlElement) this.FindChildByName("__isUserDefinedField")).GetElementValue());
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public virtual bool IsTypeUnique()
    {
      try
      {
        return "true".Equals(((CsiXmlElement) this.FindChildByName("__isTypeUnique")).GetElementValue());
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public virtual bool OwnsObject()
    {
      try
      {
        return "true".Equals(((CsiXmlElement) this.FindChildByName("__ownsSubentity")).GetElementValue());
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public virtual bool HasSelectionValues()
    {
      try
      {
        return "true".Equals(((CsiXmlElement) this.FindChildByName("__hasSelectionValues")).GetElementValue());
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public virtual Array GetAllFieldDefinitions() => this.GetChildrenByName("__field");

    public virtual ICsiFieldDefinition GetFieldDefinitionByName(string name)
    {
      Array fieldDefinitions = this.GetAllFieldDefinitions();
      for (int index = 0; index < fieldDefinitions.Length; ++index)
      {
        if (fieldDefinitions.GetValue(index) is ICsiFieldDefinition csiFieldDefinition && name.Equals(csiFieldDefinition.GetFieldName()))
          return csiFieldDefinition;
      }
      return  null;
    }

    public virtual ICsiFieldDefinition GetFieldDefinitionById(int id)
    {
      Array fieldDefinitions = this.GetAllFieldDefinitions();
      for (int index = 0; index < fieldDefinitions.Length; ++index)
      {
        if (fieldDefinitions.GetValue(index) is ICsiFieldDefinition csiFieldDefinition && id == csiFieldDefinition.GetFieldId())
          return csiFieldDefinition;
      }
      return  null;
    }

    public virtual string GetDefaultValue()
    {
      try
      {
        return ((CsiXmlElement) this.FindChildByName("__defaultValue")).GetElementValue();
      }
      catch (Exception ex)
      {
        return (string) null;
      }
    }

    public virtual string GetValue()
    {
      try
      {
        return ((CsiXmlElement) this.FindChildByName("__value")).GetElementValue();
      }
      catch (Exception ex)
      {
        return (string) null;
      }
    }

    public virtual int GetLabelId()
    {
      try
      {
        return int.Parse(((CsiXmlElement) this.FindChildByName("__Id")).GetElementValue().Trim());
      }
      catch (Exception ex)
      {
        return -1;
      }
    }

    public virtual string GetLabelName()
    {
      try
      {
        return ((CsiXmlElement) this.FindChildByName("__name")).GetElementValue();
      }
      catch (Exception ex)
      {
        return (string) null;
      }
    }

    private void RequestCdoSubTypes(string childTag, string nameOrId, bool recurse)
    {
      ICsiXmlElement csiXmlElement = this.ClearXmlElementChildByName("__CDOSubType");
      if (recurse)
      {
        CsiXmlElement csiXmlElementImpl = new CsiXmlElement(this.GetOwnerDocument(), "__recurse", csiXmlElement);
      }
      if (nameOrId == null || childTag == null)
        return;
      CsiXmlHelper.FindCreateSetValue(csiXmlElement, childTag, nameOrId);
    }
  }
}
