using System;
using System.Diagnostics.CodeAnalysis;

namespace Solarponics.Data
{
    [ExcludeFromCodeCoverage]
    public sealed class DatabaseTransactionAlreadyInProgressException : Exception
    {
        internal DatabaseTransactionAlreadyInProgressException()
            : base("Cannot create a transaction, this connection already has one in progress")
        {
        }
    }
}