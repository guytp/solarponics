// <copyright file="DatabaseTransactionNotInProgressException.cs" company="De Beers Group">
// Copyright (C) 2019 De Beers Group. All rights reserved.</copyright>

using System;
using System.Diagnostics.CodeAnalysis;

namespace Solarponics.Data
{
    [ExcludeFromCodeCoverage]
    public class DatabaseTransactionNotInProgressException : Exception
    {
        internal DatabaseTransactionNotInProgressException()
            : base("Cannot perform that operation - a transaction is not in progress on this connection.")
        {
        }
    }
}