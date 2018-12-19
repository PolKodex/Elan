using System;
using System.Runtime.Serialization;

namespace Elan.Common.Exceptions
{
    public class PasswordHintException : Exception
    {
        public PasswordHintException()
        {
        }

        public PasswordHintException(string message) : base(message)
        {
        }

        public PasswordHintException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PasswordHintException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}