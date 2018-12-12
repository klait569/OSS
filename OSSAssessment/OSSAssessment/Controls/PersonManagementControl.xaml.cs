using OSSAssessment.ViewModels;
using System.Windows.Controls;

namespace OSSAssessment.Controls
{
    /// <summary>
    /// Interaction logic for RoleManagementControl.xaml
    /// </summary>
    public partial class PersonManagementControl : UserControl
    {
        private PersonManagementViewModel ViewModel;

        public PersonManagementControl()
        {
            InitializeComponent();
            ViewModel = new PersonManagementViewModel();
            this.DataContext = ViewModel;
        }
    }
}