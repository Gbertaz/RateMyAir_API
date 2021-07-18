using System;
using System.Globalization;

namespace RateMyAir.Entities.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("The resource you are looking for does not exist.") { }

        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
