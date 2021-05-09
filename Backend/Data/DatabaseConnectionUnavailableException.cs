// <copyright file="DatabaseConnectionUnavailableException.cs" company="De Beers Group">
// Copyright (C) 2019 De Beers Group. All rights reserved.</copyright>

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