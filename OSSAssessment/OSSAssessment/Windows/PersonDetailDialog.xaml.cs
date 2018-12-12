using OSSAssessment.ViewModels;
using System.Windows;

namespace OSSAssessment.Windows
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class PersonDetailDialog : Window
    {
        private PersonDetailDialogViewModel viewModel;

        public PersonDetailDialog(int personId = 0)
        {
            InitializeComponent();
            viewModel = new PersonDetailDialogViewModel(personId);
            this.DataContext = viewModel;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}