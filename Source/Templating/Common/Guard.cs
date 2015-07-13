namespace Chimote.Tridion.Templating.Intranet.Common
{
    public static class Guard
    {
        [GuardException.HideFromStackTrace]
        public static void Requires(bool condition, string message = "Condition cannot evaluate to false")
        {
            if (!condition)
            {
                throw new GuardException(string.Format("Guard.Requires failed: {0}", message));
            }
        }

        [GuardException.HideFromStackTrace]
        public static void Requires(bool condition, string message, params object[] parameters)
        {
            Requires(condition, string.Format(message, parameters));
        }

        [GuardException.HideFromStackTrace]
        public static void IsNotNull(object value, string message = "Object cannot be null")
        {
            if (value == null)
            {
                throw new GuardException(string.Format("Guard.IsNotNull failed: {0}", message));
            }
        }

        [GuardException.HideFromStackTrace]
        public static void IsNotNull(object value, string message, params object[] parameters)
        {
            IsNotNull(value, string.Format(message, parameters));
        }

        [GuardException.HideFromStackTrace]
        public static void IsNotNullOrEmpty(string value, string message = "String cannot be null or empty")
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new GuardException(string.Format("Guard.IsNotNullOrEmpty failed: {0}", message));
            }
        }

        [GuardException.HideFromStackTrace]
        public static void IsNotNullOrEmpty(string value, string message, params object[] parameters)
        {
            IsNotNullOrEmpty(value, string.Format(message, parameters));
        }

        [GuardException.HideFromStackTrace]
        public static void ArgumentIsNotNull(object value, string parameterName)
        {
            if (value == null)
            {
                throw new GuardException(string.Format("Guard.ArgumentIsNotNull failed: {0} cannot be null", parameterName));
            }
        }

        [GuardException.HideFromStackTrace]
        public static void ArgumentIsNotNullOrEmpty(string value, string parameterName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new GuardException(string.Format("Guard.ArgumentIsNotNullOrEmpty failed: {0} cannot be null or empty", parameterName));
            }
        }

        [GuardException.HideFromStackTrace]
        public static void Ensures(bool condition, string message = "Condition cannot evaluate to false")
        {
            if (!condition)
            {
                throw new GuardException(string.Format("Guard.Ensures failed: {0}", message));
            }
        }

        [GuardException.HideFromStackTrace]
        public static void Ensures(bool condition, string message, params object[] parameters)
        {
            Ensures(condition, string.Format(message, parameters));
        }
    }
}