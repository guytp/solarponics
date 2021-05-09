// <copyright file="SqlDbConnectionFactory.cs" company="De Beers Group">
// Copyright (C) 2019 De Beers Group. All rights reserved.</copyright>

using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;

namespace Solarponics.Data
{
    [ExcludeFromCodeCoverage]
    public sealed class SqlDbConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection Create()
        {
            return new SqlConnection();
        }
    }
}