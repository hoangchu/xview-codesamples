using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;

namespace Chimote.Tridion.Templating.Intranet.Common
{
    [Serializable]
    public class GuardException : Exception
    {
        [HideFromStackTrace]
        public GuardException()
        {
        }

        [HideFromStackTrace]
        public GuardException(string message)
            : base(message + "\n" + GetStackTrace())
        {
        }

        [HideFromStackTrace]
        public GuardException(string message, Exception innerException)
            : base(message + "\n" + GetStackTrace(), innerException)
        {
        }

        [HideFromStackTrace]
        protected GuardException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        [HideFromStackTrace]
        private static string GetStackTrace()
        {
            return string.Concat(
                new StackTrace(true)
                    .GetFrames()
                    .Where(frame => !frame.GetMethod().IsDefined(typeof(HideFromStackTraceAttribute), true))
                    .Select(frame => new StackTrace(frame).ToString())
                    .ToArray());
        }

        [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, Inherited = false)]
        public class HideFromStackTraceAttribute : Attribute
        {
        }
    }
}