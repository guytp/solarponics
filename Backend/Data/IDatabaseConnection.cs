// <copyright file="IDatabaseConnection.cs" company="De Beers Group">
// Copyright (C) 2019 De Beers Group. All rights reserved.</copyright>

using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Solarponics.Data
{
    public interface IDatabaseConnection
    {
        IStoredProcedure CreateStoredProcedure(string name, IReadOnlyList<IDbDataParameter> parameters);

        Task ExecuteQueryAsync(string queryText, CommandType commandType);

        void TransactionBegin();

        void TransactionCommit();

        void TransactionRollback();
    }
}