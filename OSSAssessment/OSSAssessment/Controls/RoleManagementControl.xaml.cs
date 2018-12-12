using OSSAssessment.ViewModels;
using System.Windows.Controls;

namespace OSSAssessment.Controls
{
    /// <summary>
    /// Interaction logic for RoleManagementControl.xaml
    /// </summary>
    public partial class RoleManagementControl : UserControl
    {
        private RoleManagementViewModel ViewModel;

        public RoleManagementControl()
        {
            InitializeComponent();
            ViewModel = new RoleManagementViewModel();
            this.DataContext = ViewModel;
        }
    }
}