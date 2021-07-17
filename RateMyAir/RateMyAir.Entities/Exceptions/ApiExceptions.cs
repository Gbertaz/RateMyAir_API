using System;
using System.Globalization;

namespace RateMyAir.Entities.Exceptions
{
    public class ApiException : Exception
    {
        public ApiException() : base("An error occurred.") { }

        public ApiException(string message) : base(message) { }

        public ApiException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
