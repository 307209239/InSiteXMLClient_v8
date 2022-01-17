using Camstar.XMLClient.Interface;
using System.Xml;

namespace Camstar.XMLClient.API
{
    internal class CsiQueryParameters :
      CsiParameters,
      ICsiQueryParameters,
      ICsiParameters,
      ICsiXmlElement
    {
        public CsiQueryParameters(ICsiDocument doc, ICsiXmlElement parent)
          : base(doc, "__queryParameters", parent)
        {
        }

        public CsiQueryParameters(ICsiDocument doc, string queryName, ICsiXmlElement element)
          : this(doc, element)
          => this.SetAttribute("__queryName", queryName);

        public CsiQueryParameters(ICsiDocument doc, XmlElement element)
          : base(doc, element)
        {
            string name = element.Name;
            if (!name.Equals("__queryParameters"))
                throw new CsiClientException(-1L, CsiXmlHelper.GetInvalidElement(name) + "(valid element is: __queryParameters). ", this.GetType().FullName + ".Constructor()");
        }
    }
}