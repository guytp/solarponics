using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.ApiClients;
using Solarponics.ProductionManager.Abstractions.Hardware;
using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Commands;
using Solarponics.ProductionManager.Core;
using Solarponics.ProductionManager.LabelDefinitions;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Solarponics.ProductionManager.ViewModels
{
    public class CultureAgarLiquidPrepViewModel : ViewModelBase, ICultureAgarLiquidPrepViewModel
    {
        private Recipe[] recipes;
        private readonly IDialogBox dialogBox;
        private readonly IRecipeApiClient recipeApiClient;
        private readonly ICultureApiClient cultureApiClient;
        private readonly IHardwareProvider hardwareProvider;

        public CultureAgarLiquidPrepViewModel(IDialogBox dialogBox, IRecipeApiClient recipeApiClient, ICultureApiClient cultureApiClient, IHardwareProvider hardwareProvider, ILoggedInButtonsViewModel loggedInButtonsViewModel)
        {
            this.LoggedInButtonsViewModel = loggedInButtonsViewModel;
            this.dialogBox = dialogBox;
            this.recipeApiClient = recipeApiClient;
            this.cultureApiClient = cultureApiClient;
            this.hardwareProvider = hardwareProvider;
            MediumTypes = new[]
            {
                CultureMediumType.Agar,
                CultureMediumType.Liquid
            };
            GenerateCommand = new RelayCommand(_ => Generate());
        }

        public bool IsGenerateEnabled => SelectedRecipe != null && SelectedMediumType != null && !string.IsNullOrEmpty(Quantity);

        public bool IsUiEnabled { get; private set; }

        public ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }

        public string Quantity { get; set; }

        public Recipe[] Recipes { get; private set; }

        public Recipe SelectedRecipe { get; set; }

        public CultureMediumType? SelectedMediumType { get; set; }

        public CultureMediumType[] MediumTypes { get; }

        public string Notes { get; set; }

        public ICommand GenerateCommand { get; }

        public async override Task OnShow()
        {
            try
            {
                this.IsUiEnabled = false;
                this.recipes = await this.recipeApiClient.Get();
                this.ResetUi();
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("There was a problem loading recipe details", exception: ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }

        private void ResetUi()
        {
            this.Quantity = null;
            this.SelectedMediumType = null;
            this.SelectedRecipe = null;
            this.Notes = null;
        }

#pragma warning disable IDE0051 // Remove unused private members
        private void OnSelectedMediumTypeChanged()
#pragma warning restore IDE0051 // Remove unused private members
        {
            this.SelectedRecipe = null;

            if (!SelectedMediumType.HasValue)
            {
                this.Recipes = null;
                return;
            }

            var desiredRecipeType = SelectedMediumType.Value == CultureMediumType.Agar ? RecipeType.Agar : RecipeType.LiquidSpawn;

            Recipes = this.recipes?.Where(r => r.Type == desiredRecipeType).ToArray();
        }

        private async void Generate()
        {
            if (!IsUiEnabled || !IsGenerateEnabled)
                return;

            if (hardwareProvider?.LabelPrinterSmall == null)
            {
                this.dialogBox.Show("Unable to book in culture without label printer");
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
                    var culture = await this.cultureApiClient.CreateFromRecipe(new Models.WebApi.CultureCreateFromReciptRequest
                    {
                        MediumType = this.SelectedMediumType.Value,
                        Notes = this.Notes,
                        RecipeId = this.SelectedRecipe.Id
                    });

                    this.hardwareProvider.LabelPrinterSmall.Print(new CultureLabelDefinition(culture, null, this.SelectedRecipe));
                }
                this.ResetUi();

                this.dialogBox.Show($"Generated {quantity} {SelectedMediumType} labels");
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("There was a problem generating a label", exception: ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }
    }
}