using System.Diagnostics;

namespace Chimote.Tridion.Templating.Intranet.Common
{
    public static class DebugGuard
    {
        [Conditional("DEBUG")]
        [GuardException.HideFromStackTrace]
        public static void Requires(bool condition, string message = "Condition cannot evaluate to false")
        {
            if (!condition)
            {
                throw new GuardException(string.Format("DebugGuard.Requires failed: {0}", message));
            }
        }

        [Conditional("DEBUG")]
        [GuardException.HideFromStackTrace]
        public static void Requires(bool condition, string message, params object[] parameters)
        {
            Requires(condition, string.Format(message, parameters));
        }

        [Conditional("DEBUG")]
        [GuardException.HideFromStackTrace]
        public static void IsNotNull(object value, string message = "Object cannot be null")
        {
            if (value == null)
            {
                throw new GuardException(string.Format("DebugGuard.IsNotNull failed: {0}", message));
            }
        }

        [Conditional("DEBUG")]
        [GuardException.HideFromStackTrace]
        public static void IsNotNull(object value, string message, params object[] parameters)
        {
            IsNotNull(value, string.Format(message, parameters));
        }

        [Conditional("DEBUG")]
        [GuardException.HideFromStackTrace]
        public static void IsNotNullOrEmpty(string value, string message = "String cannot be null or empty")
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new GuardException(string.Format("DebugGuard.IsNotNullOrEmpty failed: {0}", message));
            }
        }

        [Conditional("DEBUG")]
        [GuardException.HideFromStackTrace]
        public static void IsNotNullOrEmpty(string value, string message, params object[] parameters)
        {
            IsNotNullOrEmpty(value, string.Format(message, parameters));
        }

        [Conditional("DEBUG")]
        [GuardException.HideFromStackTrace]
        public static void ArgumentIsNotNull(object value, string parameterName)
        {
            if (value == null)
            {
                throw new GuardException(string.Format("DebugGuard.ArgumentIsNotNull failed: {0} cannot be null", parameterName));
            }
        }

        [Conditional("DEBUG")]
        [GuardException.HideFromStackTrace]
        public static void ArgumentIsNotNullOrEmpty(string value, string parameterName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new GuardException(string.Format("DebugGuard.ArgumentIsNotNullOrEmpty failed: {0} cannot be null or empty", parameterName));
            }
        }

        [Conditional("DEBUG")]
        [GuardException.HideFromStackTrace]
        public static void Ensures(bool condition, string message = "Condition cannot evaluate to false")
        {
            if (!condition)
            {
                throw new GuardException(string.Format("DebugGuard.Ensures failed: {0}", message));
            }
        }

        [Conditional("DEBUG")]
        [GuardException.HideFromStackTrace]
        public static void Ensures(bool condition, string message, params object[] parameters)
        {
            Ensures(condition, string.Format(message, parameters));
        }
    }
}