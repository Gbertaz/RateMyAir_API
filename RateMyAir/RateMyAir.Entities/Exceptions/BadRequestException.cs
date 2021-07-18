using System;
using System.Globalization;

namespace RateMyAir.Entities.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException() : base("Bad Request.") { }

        public BadRequestException(string message) : base(message) { }

        public BadRequestException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
