namespace Chimote.Tridion.Templating.Intranet.Common
{
    public static class Guard
    {
        public static void Requires(bool condition, string message = "Condition cannot evaluate to false")
        {
            if (!condition)
            {
                throw new GuardException(string.Format("Guard.Requires condition failed: {0}", message));
            }
        }

        public static void Requires(bool condition, string message, params object[] parameters)
        {
            Requires(condition, string.Format(message, parameters));
        }

        public static void Ensures(bool condition, string message = "Condition cannot evaluate to false")
        {
            if (!condition)
            {
                throw new GuardException(string.Format("Guard.Requires condition failed: {0}", message));
            }
        }

        public static void Ensures(bool condition, string message, params object[] parameters)
        {
            Ensures(condition, string.Format(message, parameters));
        }

        public static void ArgumentNotNull(object value, string parameterName)
        {
            if (value == null)
            {
                throw new GuardException(string.Format("Guard.ArgumentNotNull failed: {0}", parameterName));
            }
        }

        public static void ArgumentNotNullOrEmpty(string value, string parameterName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new GuardException(string.Format("Guard.ArgumentNoNullOrEmpty failed: {0}", parameterName));
            }
        }

        public static void IsNotNull(object value, string message = "Object cannot be null")
        {
            if (value == null)
            {
                throw new GuardException(string.Format("Guard.IsNotNull failed: {0}", message));
            }
        }

        public static void IsNotNull(object value, string message, params object[] parameters)
        {
            IsNotNull(value, string.Format(message, parameters));
        }

        public static void IsNotNullOrEmpty(string value, string message = "String cannot be null or empty")
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new GuardException(string.Format("Guard.IsNotNullOrEmpty failed: {0}", message));
            }
        }

        public static void IsNotNullOrEmpty(string value, string message, params object[] parameters)
        {
            IsNotNullOrEmpty(value, string.Format(message, parameters));
        }
    }
}