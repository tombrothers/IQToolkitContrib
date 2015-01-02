using System;
using System.Runtime.Serialization;

namespace IQToolkitContrib {
    /// <summary>
    /// Validation Exception
    /// </summary>
    public class ValidationException : ApplicationException {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        public ValidationException()
            : base() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="message">Exception Message</param>
        public ValidationException(string message)
            : base(message) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="message">Exception Message</param>
        /// <param name="innerException">Inner Exception</param>
        public ValidationException(string message, Exception innerException)
            : base(message, innerException) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data</param>
        /// <param name="context">The contextual information about the source or destination</param>
        public ValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context) {
        }
    }
}
