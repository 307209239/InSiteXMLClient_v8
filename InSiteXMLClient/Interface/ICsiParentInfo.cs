namespace Camstar.XMLClient.Interface
{
    public interface ICsiParentInfo : ICsiXmlElement
    {
        void SetObjectId(string Id);

        string GetParentId();

        void SetParentId(string Id);

        void SetObjectType(string cdoType);

        string GetNamedObjectRef();

        void SetNamedObjectRef(string name);

        string GetName();

        void SetName(string name);

        ICsiParentInfo GetParentInfo();

        void SetContainerRef(string name, string level);

        void SetRevisionedObjectRef(string name, string revision, bool useROR);

        ICsiParentInfo ParentInfo();

        void GetContainerRef(out string name, out string level);

        void GetRevisionedObjectRef(out string name, out string revision, out bool useROR);
    }
}