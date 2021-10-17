using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.ApiClients;
using Solarponics.ProductionManager.Abstractions.Hardware;
using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Commands;
using Solarponics.ProductionManager.Core;
using Solarponics.ProductionManager.Data;
using Solarponics.ProductionManager.LabelDefinitions;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Solarponics.ProductionManager.ViewModels
{
    public class CultureInnoculateViewModel : ViewModelBase, ICultureInnoculateViewModel
    {
        private readonly IDialogBox dialogBox;
        private readonly ICultureApiClient cultureApiClient;
        private readonly ISupplierApiClient supplierApiClient;
        private readonly IRecipeApiClient recipeApiClient;
        private readonly IHardwareProvider hardwareProvider;
        private Supplier[] suppliers;
        private Recipe[] recipes;

        public CultureInnoculateViewModel(IDialogBox dialogBox, ICultureApiClient cultureApiClient, ISupplierApiClient supplierApiClient, IRecipeApiClient recipeApiClient, IHardwareProvider hardwareProvider, ILoggedInButtonsViewModel loggedInButtonsViewModel)
        {
            this.LoggedInButtonsViewModel = loggedInButtonsViewModel;
            this.dialogBox = dialogBox;
            this.cultureApiClient = cultureApiClient;
            this.supplierApiClient = supplierApiClient;
            this.recipeApiClient = recipeApiClient;
            this.hardwareProvider = hardwareProvider;
            this.ConfirmCommand = new RelayCommand(_ => this.Confirm());
            this.CancelCommand = new RelayCommand(_ => this.Cancel());
        }

        public bool IsUiEnabled { get; private set; }

        public bool IsConfirmEnabled => OriginCulture != null && DestinationCulture != null;
        public bool IsCancelEnabled => OriginCulture != null || DestinationCulture != null;

        private Culture DestinationCulture { get; set; }

        private Culture OriginCulture { get; set; }

        public ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }

        public string Notes { get; set; }

        public string ActionMessage { get; private set; }

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }

        public async override Task OnShow()
        {
            hardwareProvider.BarcodeScanner.BarcodeRead += OnBarcodeRead;
            try
            {
                this.ActionMessage = "Loading data...";
                this.IsUiEnabled = false;
                this.suppliers = await supplierApiClient.Get();
                this.recipes = await this.recipeApiClient.Get();
                this.ResetUi();
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("Error loading suppliers and recipes", exception: ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }

        public override Task OnHide()
        {
            hardwareProvider.BarcodeScanner.BarcodeRead -= OnBarcodeRead;
            return Task.CompletedTask;
        }

        private void Cancel()
        {
            if (!this.IsUiEnabled || !this.IsCancelEnabled)
                return;

            this.ResetUi();
        }

        private async void Confirm()
        {
            if (!this.IsUiEnabled || !this.IsConfirmEnabled)
                return;

            if (hardwareProvider?.LabelPrinterSmall == null)
            {
                this.dialogBox.Show("Unable to book in culture without label printer");
                return;
            }

            try
            {
                this.IsUiEnabled = false;
                var culture = await this.cultureApiClient.Innoculate(new Models.WebApi.CultureInnoculateRequest
                {
                    AdditionalNotes = Notes,
                    Id = this.DestinationCulture.Id,
                    ParentCultureId = this.OriginCulture.Id
                });

                var recipe = this.recipes.First(r => r.Id == culture.RecipeId);
                this.hardwareProvider.LabelPrinterSmall.Print(new CultureLabelDefinition(culture, recipe));
                this.ResetUi();
                this.dialogBox.Show("Confirmed innoculation and new label printed");
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("There was an unexpected problem confirming the innoculation.", exception: ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }

        private void ResetUi()
        {
            this.DestinationCulture = null;
            this.OriginCulture = null;
            this.Notes = null;
            this.ActionMessage = "Scan the source to innoculate from.";
        }

        private async void OnBarcodeRead(object sender, Data.BarcodeReadEventArgs e)
        {
            if ((this.OriginCulture != null && this.DestinationCulture != null) || !this.IsUiEnabled)
            {
                return;
            }

            if (!e.Barcode.StartsWith("C") || e.Barcode.Length < 2)
            {
                this.dialogBox.Show("Invalid barcode, please scan a culture.");
                return;
            }

            int cultureId;
            if (!int.TryParse(e.Barcode.Substring(1), out cultureId))
            {
                this.dialogBox.Show("Invalid barcode");
                return;
            }

            var culture = await this.cultureApiClient.Get(cultureId);
            if (this.OriginCulture == null)
            {
                if (string.IsNullOrEmpty(culture.Strain) || (culture.SupplierId == null && culture.ParentCultureId == null))
                {
                    this.dialogBox.Show("You cannot use a non-innoculated culture as the source");
                    return;
                }

                this.OriginCulture = culture;
                this.ActionMessage = "Origin is " + culture.Strain + " (Gen " + culture.Generation + ")";
                if (culture.SupplierId != null)
                {
                    this.ActionMessage += " from " + this.suppliers.First(s => s.Id == culture.SupplierId.Value).Name;
                }
                
                this.ActionMessage += Environment.NewLine + Environment.NewLine;

                this.ActionMessage += "Please scan destination culture";
                return;
            }

            if (!string.IsNullOrEmpty(culture.Strain) || (culture.SupplierId.HasValue && culture.ParentCultureId.HasValue))
            {
                this.dialogBox.Show("You cannot use an innoculated culture as the destination");
                return;
            }

            this.ActionMessage = this.ActionMessage.Replace("Please scan destination culture", string.Empty);

            this.ActionMessage += "Destination is " + culture.MediumType + " with recipe " + (recipes.First(r => r.Id == culture.RecipeId).Name);

            this.ActionMessage += Environment.NewLine + Environment.NewLine;

            this.ActionMessage += "Press Confirm or Cancel to continue";

            this.DestinationCulture = culture;
        }
    }
}