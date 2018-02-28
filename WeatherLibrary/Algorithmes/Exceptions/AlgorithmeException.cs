using System;

namespace WeatherLibrary.Algorithmes.Exceptions
{
    public class AlgorithmeException : Exception
    {
        public AlgorithmeException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
