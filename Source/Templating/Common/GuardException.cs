using System;
using System.Runtime.Serialization;

namespace Chimote.Tridion.Templating.Intranet.Common
{
    [Serializable]
    public class GuardException : Exception
    {
        public GuardException()
        {
        }

        public GuardException(string message)
            : base(message)
        {
        }

        public GuardException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected GuardException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}