using System.ComponentModel;

namespace Solarponics.ProductionManager.Data
{
    public class CategoryUiItem : INotifyPropertyChanged
    {
        public CategoryUiItem(string category)
        {
            this.Name = category;
        }

        public string Name { get; }

        public bool IsSelected { get; set;}
        
        #pragma warning disable CS0067
        public event PropertyChangedEventHandler PropertyChanged;
        #pragma warning restore CS0067
    }
}