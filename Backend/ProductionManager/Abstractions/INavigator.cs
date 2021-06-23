using System;
using Solarponics.ProductionManager.EventArgs;

namespace Solarponics.ProductionManager.Abstractions
{
    public interface INavigator
    {
        IView ActiveView { get; }

        event EventHandler<ViewEventArgs> ViewChanged;
        void ReturnToLogin();
        void NavigateTo(IView view);
        void SetLoginView(ILoginView view);
    }
}