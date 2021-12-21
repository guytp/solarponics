using Solarponics.Models;
using System;
using System.Threading.Tasks;

namespace Solarponics.WebApi.Abstractions
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface IFruitingBlockRepository
    {
        Task<FruitingBlock[]> GetAll();

        Task<FruitingBlock> Get(int id);

        Task Innoculate(int id, int cultureId, string additionalNotes, DateTime date, int userId);

        Task ShelfPlaceFruiting(int id, int shelfId, string additionalNotes, DateTime date, int userId);
        Task ShelfPlaceIncubate(int id, int shelfId, string additionalNotes, DateTime date, int userId);

        Task<int> Add(int userId, int recipeId, decimal weight, DateTime date, string notes);
    }
}