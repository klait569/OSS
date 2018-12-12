using OSSAssessment.ViewModels;
using System.Windows;

namespace OSSAssessment.Windows
{
    /// <summary>
    /// Interaction logic for NodeDetailDialog.xaml
    /// </summary>
    public partial class NodeDetailDialog : Window
    {
        private NodeDetailDialogViewModel viewModel;

        public NodeDetailDialog(int positionId = 0, int parentPositionId = 0)
        {
            InitializeComponent();
            viewModel = new NodeDetailDialogViewModel(positionId, parentPositionId);
            this.DataContext = viewModel;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}