// <copyright file="IStoredProcedure.cs" company="De Beers Group">
// Copyright (C) 2019 De Beers Group. All rights reserved.</copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Solarponics.Data
{
    public interface IStoredProcedure : IDisposable
    {
        void ExecuteNonQuery();

        Task ExecuteNonQueryAsync();

        void ExecuteReader();

        Task ExecuteReaderAsync();

        Task<T> ExecuteScalarAsync<T>();

        Task<IEnumerable<T>> GetDataSetAsync<T>();
        
        Task<T> GetFirstOrDefaultRowAsync<T>();
    }
}