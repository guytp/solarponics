using System;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.Views;
using Solarponics.ProductionManager.EventArgs;

namespace Solarponics.ProductionManager.Domain
{
    public class Navigator : INavigator
    {
        private ILoginView loginView;
        
        public void SetLoginView(ILoginView view)
        {
            this.loginView = view;
            if (this.ActiveView == null)
                this.ReturnToLogin();
        }

        public IView ActiveView { get; private set; }
        public event EventHandler<ViewEventArgs> ViewChanged;

        public void NavigateTo(IView view)
        {
            if (view == ActiveView)
                return;

            ActiveView?.ViewModel?.OnHide();
            ActiveView = view;
            view?.ViewModel?.OnShow();
            ViewChanged?.Invoke(this, new ViewEventArgs(view));
        }

        public void ReturnToLogin()
        {
            this.NavigateTo(this.loginView);
        }
    }
}