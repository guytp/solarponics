using Solarponics.ProductionManagerMobile.Abstractions.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Solarponics.ProductionManagerMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = DependencyService.Get<ILoginViewModel>();
        }
    }
}