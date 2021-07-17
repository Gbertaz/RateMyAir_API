using System;
using System.Globalization;

namespace RateMyAir.Entities.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException() : base("You are not authorized to access this resource.") { }

        public ForbiddenException(string message) : base(message) { }

        public ForbiddenException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
