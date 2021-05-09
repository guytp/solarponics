// <copyright file="IStoredProcedureFactory.cs" company="De Beers Group">
// Copyright (C) 2019 De Beers Group. All rights reserved.</copyright>

using System.Collections.Generic;
using System.Data;

namespace Solarponics.Data
{
    public interface IStoredProcedureFactory
    {
        IStoredProcedure Create(string name, IReadOnlyList<IDbDataParameter> parameters, IDbConnection connection, IDbTransaction transaction);
    }
}