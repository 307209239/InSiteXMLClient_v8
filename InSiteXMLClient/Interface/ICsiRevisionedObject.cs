namespace Camstar.XMLClient.Interface
{
    public interface ICsiRevisionedObject : ICsiObject, ICsiField, ICsiXmlElement
    {
        void GetRef(out string name, out string revision, out bool useROR);

        void SetRef(string name, string revision, bool useROR);

        string GetName();

        string GetRevision();

        bool GetUseRor();
    }
}