namespace Camstar.XMLClient.Interface
{
    public interface ICsiSubentity : ICsiObject, ICsiField, ICsiXmlElement
    {
        ICsiParentInfo GetParentInfo();

        void SetParentId(string id);

        ICsiParentInfo ParentInfo();
    }
}