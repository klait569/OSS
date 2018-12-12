using OSSAssessment.DataLayer;
using System;
using System.Windows;
using System.Windows.Threading;

namespace OSSAssessment.Services
{
    internal class SharingService
    {
        public void ReceiveStructureData(DataModel model, string sender)
        {
            if (MessageBox.Show("Load data from \"" + sender + "\"? Actual structure will be lost. ", "Received data",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => GlobalDataModel.Instance.LoadModel(model)));
            }
        }
    }
}