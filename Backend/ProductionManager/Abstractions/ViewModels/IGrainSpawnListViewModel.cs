using Solarponics.Models;
using System.Windows.Input;

namespace Solarponics.ProductionManager.Abstractions.ViewModels
{
    public interface IGrainSpawnListViewModel : IViewModel
    {
        bool IsUiEnabled { get; }

        ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }

        GrainSpawn[] GrainSpawns { get; }

        ICommand PrintLabelCommand { get; }
    }
}