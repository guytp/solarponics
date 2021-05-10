using System;
using System.Diagnostics.CodeAnalysis;

namespace Solarponics.Data
{
    [ExcludeFromCodeCoverage]
    public sealed class StoredProcedureExecutedTooManyTimesException : Exception
    {
        internal StoredProcedureExecutedTooManyTimesException()
            : base("The stored procedure can only be executed once during its lifetime")
        {
        }
    }
}