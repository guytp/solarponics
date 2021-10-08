using Solarponics.Models;
using Solarponics.Models.WebApi;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.ApiClients;
using Solarponics.ProductionManager.Abstractions.Hardware;
using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Commands;
using Solarponics.ProductionManager.Core;
using Solarponics.ProductionManager.Data;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Solarponics.ProductionManager.ViewModels
{
    public class CultureBookInViewModel : ViewModelBase, ICultureBookInViewModel
    {
        private readonly ICultureApiClient cultureApiClient;
        private readonly ISupplierApiClient supplierApiClient;
        private readonly IDialogBox dialogBox;
        private readonly IHardwareProvider hardwareProvider;

        public CultureBookInViewModel(ILoggedInButtonsViewModel loggedInButtonsViewModel, ISupplierApiClient supplierApiClient, ICultureApiClient cultureApiClient, IDialogBox dialogBox, IHardwareProvider hardwareProvider)
        {
            this.LoggedInButtonsViewModel = loggedInButtonsViewModel;
            this.supplierApiClient = supplierApiClient;
            this.BookInCommand = new RelayCommand(_ => BookIn());
            this.dialogBox = dialogBox;
            this.hardwareProvider = hardwareProvider;
            this.IsUiEnabled = true;
            this.cultureApiClient = cultureApiClient;
            this.MediumTypes = new[]
            {
                CultureMediumType.Agar,
                CultureMediumType.Liquid
            };
        }

        public ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }

        public Supplier SelectedSupplier { get; set; }

        public Supplier[] Suppliers { get; private set; }

        public CultureMediumType? MediumType { get; set; }

        public CultureMediumType[] MediumTypes { get; set; }

        public DateTime? OrderDate { get; set; }

        public string Strain { get; set; }

        public string Notes { get; set; }

        public ICommand BookInCommand { get; }

        public bool IsBookInEnabled => SelectedSupplier != null && MediumType.HasValue && OrderDate.HasValue && !string.IsNullOrEmpty(Strain);

        public bool IsUiEnabled { get; private set; }

        public override async Task OnShow()
        {
            try
            {
                this.IsUiEnabled = false;

                this.Suppliers = await this.supplierApiClient.Get();
                this.ResetUi();
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("Failed to update supplier list", exception: ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }

        private void ResetUi()
        {
            this.SelectedSupplier = null;
            this.MediumType = null;
            this.OrderDate = null;
            this.Strain = null;
            this.Notes = null;
        }

        private async void BookIn()
        {
            if (!IsBookInEnabled || !this.IsUiEnabled)
                return;

            if (hardwareProvider?.LabelPrinter == null)
            {
                this.dialogBox.Show("Unable to book in culture without label printer");
                return;
            }

            this.IsUiEnabled = false;
            try
            {
                var culture = await this.cultureApiClient.BookIn(new CultureBookInRequest
                {
                    MediumType = this.MediumType.Value,
                    Notes = this.Notes,
                    OrderDate = this.OrderDate.Value,
                    Strain = this.Strain,
                    SupplierId = this.SelectedSupplier.Id
                });

                this.hardwareProvider.LabelPrinter.Print(new CultureLabelDefinition(culture, this.SelectedSupplier));

                this.ResetUi();

                this.dialogBox.Show("Booked in culture and label printed");
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("There was an unexpected problem booking in the culture.", exception: ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }
    }
}