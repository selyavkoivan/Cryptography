using System;
using System.Runtime.Serialization;

namespace Cryptography.Crypto
{
    public class IncorrectValueException : Exception
    {
        public IncorrectValueException() : base()
        {
        }

        public IncorrectValueException(string message) : base(message)
        {
        }

        public IncorrectValueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public IncorrectValueException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}