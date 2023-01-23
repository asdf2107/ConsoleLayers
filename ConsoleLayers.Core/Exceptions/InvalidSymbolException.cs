using System;

namespace ConsoleLayers.Core.Exceptions
{
    [Serializable]
    public class InvalidSymbolException : Exception
    {
        public InvalidSymbolException() { }
        public InvalidSymbolException(string message) : base(message) { }
        public InvalidSymbolException(string message, Exception inner) : base(message, inner) { }
        protected InvalidSymbolException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
