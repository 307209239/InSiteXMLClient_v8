namespace Camstar.Exceptions
{
    public class XMLServerDownException : CamstarException
    {
        public XMLServerDownException()
          : base(nameof(XMLServerDownException))
        {
        }
    }
}