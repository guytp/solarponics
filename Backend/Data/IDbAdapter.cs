// <copyright file="DapperStoredProcedure.cs" company="De Beers Group">
// Copyright (C) 2019 De Beers Group. All rights reserved.</copyright>

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

using Microsoft.Extensions.Logging;

namespace Solarponics.Data
{
    public interface IDbAdapter
    {
        void Execute(IDbConnection connection, string name, DynamicParameters dynamicParameters, IDbTransaction transaction, CommandType commandType);
        Task ExecuteAsync(IDbConnection connection, string name, DynamicParameters dynamicParameters, IDbTransaction transaction, CommandType commandType);
        SqlMapper.GridReader QueryMultiple(IDbConnection connection, string name, DynamicParameters dynamicParameters, CommandType commandType, IDbTransaction transaction);
        Task<SqlMapper.GridReader> QueryMultipleAsync(IDbConnection connection, string name, DynamicParameters dynamicParameters, CommandType commandType, IDbTransaction transaction);
        Task<T> ExecuteScalarAsync<T>(IDbConnection connection, string name, DynamicParameters dynamicParameters, IDbTransaction transaction, CommandType commandType);
    }
}