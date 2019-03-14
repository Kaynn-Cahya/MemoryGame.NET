using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MemoryGameDotNet {
    public class InvalidBoardSizeException : Exception {

        public InvalidBoardSizeException() : base() {

        }

        public InvalidBoardSizeException(String message) : base(message) {
        }

        public InvalidBoardSizeException(String message, Exception innerException) : base(message, innerException) {
        }

        protected InvalidBoardSizeException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
