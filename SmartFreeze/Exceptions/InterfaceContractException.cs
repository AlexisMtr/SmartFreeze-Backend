using System;
using System.Runtime.Serialization;
using System.Text;

namespace SmartFreeze.Exceptions
{
    public class InterfaceContractException : Exception
    {
        private readonly string parameterName;

        public InterfaceContractException()
        {
        }

        public InterfaceContractException(string message) : base(message)
        {
        }

        public InterfaceContractException(string message, string parameterName)
            : base(message)
        {
            this.parameterName = parameterName;
        }

        public InterfaceContractException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InterfaceContractException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder("API Contract Interface violation");
            if (!string.IsNullOrEmpty(parameterName)) builder.Append($" for {parameterName}");

            return $"{builder.ToString()}{Environment.NewLine}{base.ToString()}";
        }
    }
}
