using System;
using System.Runtime.Serialization;

namespace Elan.Common.Exceptions
{
    public class InvalidUserImageIdException : Exception
    {
        public InvalidUserImageIdException()
        {
        }

        public InvalidUserImageIdException(string message) : base(message)
        {
        }

        public InvalidUserImageIdException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidUserImageIdException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
