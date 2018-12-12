using OSSAssessment.ViewModels;
using System.Windows.Controls;

namespace OSSAssessment.Controls
{
    /// <summary>
    /// Interaction logic for StructureControl.xaml
    /// </summary>
    public partial class StructureControl : UserControl
    {
        private StructureViewModel ViewModel;

        public StructureControl()
        {
            InitializeComponent();
            ViewModel = new StructureViewModel();
            this.DataContext = ViewModel;
        }
    }
}