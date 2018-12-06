using System;
using System.Runtime.Serialization;

namespace Elan.Common.Exceptions
{
    public class RelationNotFoundException : Exception
    {
        public RelationNotFoundException()
        {
        }

        public RelationNotFoundException(string message) : base(message)
        {
        }

        public RelationNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RelationNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
