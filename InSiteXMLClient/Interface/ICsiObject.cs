using System;

namespace Camstar.XMLClient.Interface
{
    public interface ICsiObject : ICsiField, ICsiXmlElement
    {
        string GetObjectId();

        void SetObjectId(string Id);

        string GetObjectType();

        void SetObjectType(string CDOType);

        ICsiXmlElement[] GetUserDefinedFields();

        ICsiPerform Perform(string eventName);

        ICsiDataField DataField(string fieldName);

        ICsiNamedObject NamedObjectField(string fieldName);

        ICsiRevisionedObject RevisionedObjectField(string fieldName);

        ICsiField GetField(string fieldName);

        ICsiDataList DataList(string fieldName);

        ICsiContainerList ContainerList(string fieldName);

        ICsiSubentityList SubentityList(string fieldName);

        ICsiNamedObjectList NamedObjectList(string fieldName);

        ICsiRevisionedObjectList RevisionedObjectList(string fieldName);

        ICsiNamedSubentityList NamedSubentityList(string fieldName);

        ICsiSubentity SubentityField(string fieldName);

        ICsiContainer ContainerField(string fieldName);

        ICsiNamedSubentity NamedSubentityField(string fieldName);

        void CreateObject(string CDOType);

        ICsiObject ObjectField(string fieldName);

        ICsiObjectList ObjectList(string fieldName);
    }
}