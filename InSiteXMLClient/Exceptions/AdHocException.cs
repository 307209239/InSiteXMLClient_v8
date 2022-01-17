namespace Camstar.Exceptions
{
    public class AdHocException : CamstarException
    {
        private string mMessage = string.Empty;
        private string mId = string.Empty;
        private string mKey = string.Empty;

        public AdHocException(string id, string message)
        {
            this.mId = id;
            this.mMessage = message;
            this.mKey = this.GetType().Name;
        }

        public AdHocException(string id, string key, string message)
        {
            this.mId = id;
            this.mMessage = message;
            this.mKey = key;
        }

        public override string Message => this.mMessage;

        public override string Id => this.mId;

        public override string Key => this.mKey;
    }
}