using System;
using System.Diagnostics.CodeAnalysis;

namespace Solarponics.Data
{
    [ExcludeFromCodeCoverage]
    public sealed class DatabaseConnectionUnavailableException : Exception
    {
        internal DatabaseConnectionUnavailableException()
            : base("The database connection is not currently available.")
        {
        }
    }
}