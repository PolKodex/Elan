using System;
using System.Runtime.Serialization;

namespace Elan.Common.Exceptions
{
    public class SignInFailedException : Exception
    {
        public SignInFailedException()
        {
        }

        public SignInFailedException(string message) : base(message)
        {
        }

        public SignInFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SignInFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
