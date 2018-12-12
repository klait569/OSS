using Microsoft.Win32;
using OSSAssessment.DataLayer;
using OSSAssessment.Services;

namespace OSSAssessment.ViewModels
{
    internal class MainWindowViewModel : BindableBase
    {
        public ActionCommand SaveCommand
        {
            get
            {
                return new ActionCommand(SaveAction);
            }
        }

        public ActionCommand OpenCommand
        {
            get
            {
                return new ActionCommand(OpenAction);
            }
        }

        private void OpenAction(object data)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FileService fs = new FileService();
                GlobalDataModel.Instance.LoadModel(fs.LoadDataFromFile(openFileDialog.FileName));
            }
        }

        private void SaveAction(object data)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                FileService fs = new FileService();
                fs.SaveDataToFile(GlobalDataModel.Instance.Model, saveFileDialog.FileName);
            }
        }
    }
}