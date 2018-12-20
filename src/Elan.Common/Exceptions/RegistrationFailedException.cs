using System;
using System.Runtime.Serialization;

namespace Elan.Common.Exceptions
{
    public class RegistrationFailedException : Exception
    {
        public RegistrationFailedException()
        {
        }

        public RegistrationFailedException(string message) : base(message)
        {
        }

        public RegistrationFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RegistrationFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
