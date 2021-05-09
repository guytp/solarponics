using System;

namespace Solarponics.IngestionServer.Exceptions
{
    public class ClientMissingHandshakeException : Exception
    {
        public ClientMissingHandshakeException()
            : base("Not yet authenticated to access this command")
        {
        }
    }
}