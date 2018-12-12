using OSSAssessment.DataLayer;
using OSSAssessment.Services;
using System.Windows;
using System.Windows.Threading;

namespace OSSAssessment
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Start(object sender, StartupEventArgs args)
        {
            Application.Current.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(AppDispatcherUnhandledException);
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            FileService fs = new FileService();
            var tmpModel = fs.LoadDataFromFile();
            if (tmpModel != null)
            {
                GlobalDataModel.Instance.LoadModel(tmpModel);
            }
        }

        void AppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            //e.Handled = tr;
            

            MessageBox.Show(e.Exception.Message, "Oops.. ", MessageBoxButton.OK, MessageBoxImage.Error);


        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            FileService fs = new FileService();
            fs.SaveDataToFile(GlobalDataModel.Instance.Model);
        }
    }
}