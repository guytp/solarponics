using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ProductionManager.Core.Abstractions;
using Solarponics.ProductionManager.Core.Abstractions;
using Solarponics.ProductionManager.Core.Abstractions.ApiClients;
using Solarponics.ProductionManager.Core.Enums;

namespace Solarponics.ProductionManager.ViewModels
{
    public class SetupSupplierViewModel : ViewModelBase, ISetupSupplierViewModel
    {
        private readonly ISupplierApiClient supplierApiClient;
        private readonly IDialogBox dialogBox;
        private readonly IBannerNotifier bannerNotifier;

        public SetupSupplierViewModel(ILoggedInButtonsViewModel loggedInButtonsViewModel, ISupplierApiClient supplierApiClient, IDialogBox dialogBox, IBannerNotifier bannerNotifier)
        {
            this.LoggedInButtonsViewModel = loggedInButtonsViewModel;
            this.supplierApiClient = supplierApiClient;
            this.dialogBox = dialogBox;
            this.AddCommand = new RelayCommand(_ => this.Add());
            this.DeleteCommand = new RelayCommand(_ => this.Delete());
            this.IsUiEnabled = true;
            this.bannerNotifier = bannerNotifier;
        }

        public ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }

        public bool IsAddEnabled { get; private set; }
        public bool IsDeleteEnabled { get; private set; }
        public bool IsUiEnabled { get; private set; }
        public Supplier[] Suppliers { get; private set; }
        public Supplier SelectedSupplier { get; set; }
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public string NewName { get; set; }

        public override async Task OnShow()
        {
            this.Suppliers = await this.supplierApiClient.Get();
            this.SelectedSupplier = null;
            this.NewName = null;
        }
        
        private async void Add()
        {
            if (!this.IsUiEnabled)
                return;

            if (string.IsNullOrWhiteSpace(this.NewName))
            {
                this.dialogBox.Show("Name is required");
                return;
            }

            try
            {
                this.IsUiEnabled = false;
                var supplier = await this.supplierApiClient.Add(new Supplier
                {
                    Name = NewName
                });
                var suppliers = new List<Supplier>();
                if (this.Suppliers != null && this.Suppliers.Length > 0)
                {
                    suppliers.AddRange(this.Suppliers);
                }
                suppliers.Add(supplier);
                this.Suppliers = suppliers.OrderBy(s => s.Name).ToArray();
                this.NewName = null;
                this.bannerNotifier.DisplayMessage("Supplier added.");
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("Failed to add supplier.", ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }

        private async void Delete()
        {
            if (!this.IsUiEnabled)
                return;

            if (this.SelectedSupplier == null)
                return;

            if (!this.dialogBox.Show("Do you want to delete " + this.SelectedSupplier.Name + "?", buttons: DialogBoxButtons.YesNo))
            {
                return;
            }

            try
            {
                this.IsUiEnabled = false;
                await this.supplierApiClient.Delete(this.SelectedSupplier.Id);
                this.Suppliers = this.Suppliers.Where(s => s != this.SelectedSupplier).ToArray();
                this.SelectedSupplier = null;
                this.bannerNotifier.DisplayMessage("Supplier deleted.");
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("Failed to delete supplier.", ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }

#pragma warning disable IDE0051 // Remove unused private members -- Fody
        private void OnSelectedSupplierChanged()
#pragma warning restore IDE0051 // Remove unused private members
        {
            this.IsDeleteEnabled = this.SelectedSupplier != null;
        }

#pragma warning disable IDE0051 // Remove unused private members -- Fody
        private void OnNewNameChanged()
#pragma warning restore IDE0051 // Remove unused private members
        {
            this.IsAddEnabled = !string.IsNullOrWhiteSpace(this.NewName);
        }
    }
}