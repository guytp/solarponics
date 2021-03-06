using Solarponics.Models;
using Solarponics.Models.WebApi;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.Hardware;
using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Commands;
using Solarponics.ProductionManager.LabelDefinitions;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ProductionManager.Core.Abstractions;
using Solarponics.ProductionManager.Core.Abstractions;
using Solarponics.ProductionManager.Core.Abstractions.ApiClients;

namespace Solarponics.ProductionManager.ViewModels
{
    public class GrainSpawnCreateViewModel : ViewModelBase, IGrainSpawnCreateViewModel
    {
        private readonly IGrainSpawnApiClient grainSpawnApiClient;
        private readonly IRecipeApiClient recipeApiClient;
        private readonly IDialogBox dialogBox;
        private readonly IHardwareProvider hardwareProvider;
        private readonly IBannerNotifier bannerNotifier;

        public GrainSpawnCreateViewModel(ILoggedInButtonsViewModel loggedInButtonsViewModel, IRecipeApiClient recipeApiClient, IGrainSpawnApiClient grainSpawnApiClient, IDialogBox dialogBox, IHardwareProvider hardwareProvider, IBannerNotifier bannerNotifier)
        {
            this.LoggedInButtonsViewModel = loggedInButtonsViewModel;
            this.recipeApiClient = recipeApiClient;
            this.bannerNotifier = bannerNotifier;
            this.AddCommand = new RelayCommand(_ => Add());
            this.dialogBox = dialogBox;
            this.hardwareProvider = hardwareProvider;
            this.IsUiEnabled = true;
            this.grainSpawnApiClient = grainSpawnApiClient;
        }

        public ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }

        public Recipe SelectedRecipe { get; set; }
        public DateTime Date { get; set; }
        public Recipe[] Recipes { get; private set; }

        public string Weight { get; set; }

        public string Notes { get; set; }
        public string Quantity { get; set; }

        public ICommand AddCommand { get; }

        public bool IsAddEnabled => SelectedRecipe != null && !string.IsNullOrEmpty(Weight) && decimal.TryParse(Weight, out _) && !string.IsNullOrEmpty(Quantity) && int.TryParse(Quantity, out _);

        public bool IsUiEnabled { get; private set; }

        public override async Task OnShow()
        {
            try
            {
                this.Date = DateTime.UtcNow;
                this.IsUiEnabled = false;
                this.Recipes = (await this.recipeApiClient.Get()).Where(r => r.Type == RecipeType.GrainSpawn).ToArray();
                this.ResetUi();
                if (this.hardwareProvider.Scale != null)
                    this.hardwareProvider.Scale.WeightRead += OnWeightRead;
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("Failed to update recipe list", exception: ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }

        public override Task OnHide()
        {
            if (this.hardwareProvider.Scale != null)
                this.hardwareProvider.Scale.WeightRead -= OnWeightRead;
            return base.OnHide();
        }

        private void OnWeightRead(object sender, Data.WeightReadEventArgs e)
        {
            if (!this.IsUiEnabled)
                return;

            this.Weight = e.Weight.ToString();
        }

        private void ResetUi()
        {
            this.SelectedRecipe = null;
            this.Weight = null;
            this.Notes = null;
            this.Quantity = null;
        }

        private async void Add()
        {
            if (!IsAddEnabled || !this.IsUiEnabled)
                return;

            if (hardwareProvider?.LabelPrinterLarge == null)
            {
                this.dialogBox.Show("Unable to create grain spawn large label printer");
                return;
            }

            int quantity;
            if (!int.TryParse(Quantity, out quantity))
            {
                this.dialogBox.Show("Invalid quantity");
                return;
            }

            try
            {
                this.IsUiEnabled = false;
                for (var i = 0; i < quantity; i++)
                {
                    var grainSpawn = await this.grainSpawnApiClient.Add(new GrainSpawnAddRequest
                    {
                        RecipeId = this.SelectedRecipe.Id,
                        Weight = decimal.Parse(this.Weight),
                        Date = this.Date
                    });

                    this.hardwareProvider.LabelPrinterLarge.Print(new GrainSpawnLabelDefinition(grainSpawn));
                }

                this.ResetUi();

                this.bannerNotifier.DisplayMessage($"Generated {quantity} grain spawn labels");
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("There was an unexpected problem creating the grain spawn.", exception: ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }
    }
}