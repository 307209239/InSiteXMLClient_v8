namespace Camstar.XMLClient.Interface
{
    public interface ICsiXmlElement
    {
        ICsiDocument GetOwnerDocument();

        ICsiXmlElement GetParentElement();

        bool IsField();

        bool IsObject();

        bool IsContainer();

        bool IsList();

        bool IsNamedObject();

        bool IsService();

        bool IsRevisionedObject();

        bool IsRequestData();

        bool IsDataField();

        bool IsSubentity();

        bool IsDataList();

        bool IsContainerList();

        bool IsNamedObjectList();

        bool IsRevisionedObjectList();

        bool IsSubentityList();

        bool IsNamedSubentityList();

        bool IsNamedSubentity();

        bool IsObjectList();

        ICsiXmlElement[] GetAllChildren();

        string GetElementName();

        void RemoveAllChildren();

        ICsiXmlElement FindChildByName(string name);

        ICsiXmlElement AppendChild(ICsiXmlElement child);

        ICsiXmlElement RemoveChild(ICsiXmlElement child);

        ICsiField AsField();

        ICsiDataField AsDataField();

        ICsiList AsList();

        ICsiObject AsObject();

        ICsiService AsService();

        ICsiContainer AsContainer();

        ICsiRevisionedObject AsRevisionedObject();

        ICsiNamedObject AsNamedObject();

        ICsiSubentity AsSubentity();

        ICsiRequestData AsRequestData();

        ICsiDataList AsDataList();

        ICsiContainerList AsContainerList();

        ICsiNamedObjectList AsNamedObjectList();

        ICsiSubentityList AsSubentityList();

        ICsiRevisionedObjectList AsRevisionedObjectList();

        ICsiObjectList AsObjectList();

        ICsiNamedSubentityList AsNamedSubentityList();

        ICsiNamedSubentity AsNamedSubentity();

        ICsiXmlElement[] GetChildrenByName(string name);

        bool HasChildren();

        void SetAttribute(string name, string val);

        string GetAttribute(string name);
    }
}