﻿using Solarponics.Models;
using System.Threading.Tasks;

namespace Solarponics.WebApi.Abstractions
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface IGrainSpawnRepository
    {
        Task<GrainSpawn[]> GetAll();

        Task<GrainSpawn> Get(int id);

        Task Innoculate(int id, int cultureId, string additionalNotes, int userId);

        Task ShelfPlace(int id, int shelfId, string additionalNotes, int userId);

        Task<int> Add(int userId, int recipeId, decimal weight, string notes);
    }
}