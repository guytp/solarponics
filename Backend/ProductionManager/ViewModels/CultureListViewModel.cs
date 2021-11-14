using Serilog;
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Solarponics.ProductionManager.ViewModels
{
    public class CultureListViewModel : ViewModelBase, ICultureListViewModel
    {
        private readonly ICultureApiClient apiClient;
        private readonly IHardwareProvider hardwareProvider;
        private readonly IDialogBox dialogBox;
        private readonly ISupplierApiClient supplierApiClient;
        private readonly IRecipeApiClient recipeApiClient;

        public CultureListViewModel(ILoggedInButtonsViewModel loggedInButtonsViewModel, ICultureApiClient apiClient, IHardwareProvider hardwareProvider, IDialogBox dialogBox, ISupplierApiClient supplierApiClient, IRecipeApiClient recipeApiClient)
        {
            this.LoggedInButtonsViewModel = loggedInButtonsViewModel;
            this.apiClient = apiClient;
            this.hardwareProvider = hardwareProvider;
            this.dialogBox = dialogBox;
            this.PrintLabelCommand = new RelayCommand(o => this.PrintLabel((CultureDataGridRowEntry)o));
            this.supplierApiClient = supplierApiClient;
            this.recipeApiClient = recipeApiClient;
        }

        public bool IsUiEnabled { get; private set; }

        public ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }

        public CultureDataGridRowEntry[] Cultures { get; private set; }

        public ICommand PrintLabelCommand { get; }

        private void PrintLabel(CultureDataGridRowEntry culture)
        {
            if (this.hardwareProvider.LabelPrinterSmall == null)
            {
                this.dialogBox.Show("No small label printer detected");
                return;
            }

            try
            {
                this.IsUiEnabled = false;


                CultureLabelDefinition definition;
                if (culture.Supplier != null)
                {
                    definition = new CultureLabelDefinition(culture.OriginalCulture, culture.Supplier);
                }
                else
                {
                    definition = new CultureLabelDefinition(culture.OriginalCulture, culture.ParentCulture, culture.Recipe);
                }

                this.hardwareProvider.LabelPrinterSmall.Print(definition);
                this.dialogBox.Show("Printed label");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to re-print culture label");
                this.dialogBox.Show("Failed to print label", ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }

        public async override Task OnShow()
        {
            try
            {
                this.IsUiEnabled = false;
                var cultures = await this.apiClient.GetAll();
                var recipes = await this.recipeApiClient.Get();
                var suppliers = await this.supplierApiClient.Get();
                var list = new List<CultureDataGridRowEntry>();
                foreach (var culture in cultures)
                {
                    Culture parentCulture = null;
                    if (culture.ParentCultureId.HasValue)
                        parentCulture = cultures.First(c => c.Id == culture.ParentCultureId);
                    Supplier supplier = null;
                    if (culture.SupplierId.HasValue)
                        supplier = suppliers.First(s => s.Id == culture.SupplierId.Value);
                    Recipe recipe = null;
                    if (culture.RecipeId.HasValue)
                        recipe = recipes.First(r => r.Id == culture.RecipeId.Value);
                    list.Add(new CultureDataGridRowEntry(culture, parentCulture, supplier, recipe));
                }
                Cultures = list.ToArray();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to load cultures");
                this.dialogBox.Show("Failed to load culture list", ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }
    }
}