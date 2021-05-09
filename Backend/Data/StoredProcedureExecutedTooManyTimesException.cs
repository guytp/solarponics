// <copyright file="StoredProcedureExecutedTooManyTimesException.cs" company="De Beers Group">
// Copyright (C) 2019 De Beers Group. All rights reserved.</copyright>

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