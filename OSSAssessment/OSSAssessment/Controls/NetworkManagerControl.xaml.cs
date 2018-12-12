using OSSAssessment.ViewModels;
using System.Windows.Controls;

namespace OSSAssessment.Controls
{
    /// <summary>
    /// Interaction logic for NetworkSenderControl.xaml
    /// </summary>
    public partial class NetworkManagerControl : UserControl
    {
        private NetworkManagerViewModel viewModel;

        public NetworkManagerControl()
        {
            InitializeComponent();
            viewModel = new NetworkManagerViewModel();
            this.DataContext = viewModel;
        }
    }
}