using Solarponics.Models;
using System.Windows.Input;

namespace Solarponics.ProductionManager.Abstractions.ViewModels
{
    public interface IFruitingBlockListViewModel : IViewModel
    {
        bool IsUiEnabled { get; }

        ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }

        FruitingBlock[] FruitingBlocks { get; }

        ICommand PrintLabelCommand { get; }
    }
}