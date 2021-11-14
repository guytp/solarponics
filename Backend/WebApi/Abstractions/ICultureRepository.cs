using Solarponics.Models;
using System;
using System.Threading.Tasks;

namespace Solarponics.WebApi.Abstractions
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface ICultureRepository
    {
        Task<Culture[]> GetAll();

        Task<Culture> Get(int id);

        Task Innoculate(int id, int parentCultureId, string additionalNotes, int userId);

        Task<int> Add(int? supplierId, int? parentCultureId, int userId, int? recipeId, CultureMediumType mediumType, DateTime? orderDate, string strain, string notes, int? generation);
    }
}