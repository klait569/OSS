using OSSAssessment.ViewModels;
using System.Windows;

namespace OSSAssessment.Windows
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class RoleDetailDialog : Window
    {
        private RoleDetailDialogViewModel viewModel;

        public RoleDetailDialog(int roleId = 0)
        {
            InitializeComponent();
            viewModel = new RoleDetailDialogViewModel(roleId);
            this.DataContext = viewModel;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}