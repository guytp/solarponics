using Solarponics.Models;
using System;
using System.Threading.Tasks;

namespace Solarponics.WebApi.Abstractions
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface IGrainSpawnRepository
    {
        Task<GrainSpawn[]> GetAll();

        Task<GrainSpawn> Get(int id);

        Task Innoculate(int id, int cultureId, string additionalNotes, int userId, DateTime date);

        Task ShelfPlace(int id, int shelfId, string additionalNotes, int userId, DateTime date);

        Task<int> Add(int userId, int recipeId, decimal weight, string notes, DateTime date);
        Task AddMix(int id, int userId, DateTime date, string additionalNotes);
    }
}