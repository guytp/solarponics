﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Solarponics.Data;
using Solarponics.Models;
using Solarponics.WebApi.Abstractions;

namespace Solarponics.WebApi.Data
{
    internal class SensorReadingRepository : ISensorReadingRepository
    {
        private const string ProcedureNameReadingAggregateGet =
            "[dbo].[ReadingAggregateGet]";

        public SensorReadingRepository(IDatabaseConnection connection)
        {
            Connection = connection;
        }

        private IDatabaseConnection Connection { get; }

        public async Task<SensorReading[]> GetReadings(int id, AggregateTimeframe timeframe)
        {
            var databaseTimeframe = timeframe switch
            {
                AggregateTimeframe.OneMinute => "1M",
                AggregateTimeframe.FiveMinutes => "5M",
                AggregateTimeframe.FifteenMinutes => "15M",
                AggregateTimeframe.ThirtyMinutes => "30M",
                AggregateTimeframe.OneHour => "1H",
                AggregateTimeframe.FourHours => "4H",
                AggregateTimeframe.TwelveHours => "12H",
                AggregateTimeframe.OneDay => "1D",
                _ => throw new ArgumentOutOfRangeException(nameof(timeframe), timeframe, null)
            };

            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameReadingAggregateGet, new[]
            {
                new StoredProcedureParameter("id", id),
                new StoredProcedureParameter("timeframe", databaseTimeframe)
            });

            await storedProcedure.ExecuteReaderAsync();
            return (await storedProcedure.GetDataSetAsync<SensorReading>())?.ToArray();
        }
    }
}