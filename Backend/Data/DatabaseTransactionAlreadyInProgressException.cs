// <copyright file="DatabaseTransactionAlreadyInProgressException.cs" company="De Beers Group">
// Copyright (C) 2019 De Beers Group. All rights reserved.</copyright>

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