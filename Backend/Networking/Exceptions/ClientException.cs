using System;

namespace Solarponics.Networking.Exceptions
{
    public class ClientException : Exception
    {
        public ClientException(string urn, string message)
            : base(message)
        {
            Urn = urn;
        }

        public string Urn { get; }
    }
}