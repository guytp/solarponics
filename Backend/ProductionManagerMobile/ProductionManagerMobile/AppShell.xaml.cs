using Solarponics.ProductionManagerMobile.ViewModels;
using Solarponics.ProductionManagerMobile.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Solarponics.ProductionManagerMobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
