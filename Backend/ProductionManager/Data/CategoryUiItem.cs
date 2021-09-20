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

        public event PropertyChangedEventHandler PropertyChanged;
    }
}