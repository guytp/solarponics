using System.Windows;
using System.Windows.Controls;

namespace Solarponics.ProductionManager.Views
{
    public partial class ViewHeader : UserControl
    {
        public ViewHeader()
        {
            InitializeComponent();
        }
        
        public static readonly DependencyProperty CategoryProperty = DependencyProperty.Register("Category", typeof(string), typeof(ViewHeader),new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(ViewHeader),new PropertyMetadata(string.Empty));
        
        public string Category
        {
            get { return (string)this.GetValue(CategoryProperty); }
            set { this.SetValue(CategoryProperty, value); }
        }

        public string Title
        {
            get { return (string)this.GetValue(TitleProperty); }
            set { this.SetValue(TitleProperty, value); }
        }
    }
}