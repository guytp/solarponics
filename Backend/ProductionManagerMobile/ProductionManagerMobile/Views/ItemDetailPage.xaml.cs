using Solarponics.ProductionManagerMobile.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Solarponics.ProductionManagerMobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}