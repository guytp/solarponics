using Solarponics.Models;
using System.Threading.Tasks;

namespace Solarponics.WebApi.Abstractions
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface IWasteReasonRepository
    {
        Task<int> Add(WasteReason wasteReason, int userId);

        Task Delete(int id, int userId);

        Task<WasteReason[]> Get();
    }
}