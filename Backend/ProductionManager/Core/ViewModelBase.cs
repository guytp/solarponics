using System.ComponentModel;
using System.Threading.Tasks;

namespace Solarponics.ProductionManager.Core
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
#pragma warning disable CS0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0067

        // ReSharper disable once UnusedMember.Global
        public virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual Task OnHide()
        {
            return Task.CompletedTask;
        }

        public virtual Task OnShow()
        {
            return Task.CompletedTask;
        }
    }
}