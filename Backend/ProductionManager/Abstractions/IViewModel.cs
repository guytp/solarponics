using System.Threading.Tasks;

namespace Solarponics.ProductionManager.Abstractions
{
    public interface IViewModel
    {
        Task OnHide();

        Task OnShow();
    }
}