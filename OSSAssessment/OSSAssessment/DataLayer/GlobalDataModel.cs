using System.Collections.ObjectModel;

namespace OSSAssessment.DataLayer
{
    internal class GlobalDataModel : BindableBase
    {
        private static GlobalDataModel instance;

        public static GlobalDataModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GlobalDataModel();
                }
                return instance;
            }
        }

        public GlobalDataModel()
        {
            Model.PropertyChanged += Model_PropertyChanged;
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged("");
        }

        public void LoadModel(DataModel model)
        {
            Model.Structures.Clear();
            Model.Roles.Clear();
            Model.Persons.Clear();
            foreach (var item in model.Roles)
            {
                Model.Roles.Add(item);
            }
            foreach (var item in model.Structures)
            {
                Model.Structures.Add(item);
            }
            foreach (var item in model.Persons)
            {
                Model.Persons.Add(item);
            }
        }

        public string Port { get; set; }

        public DataModel Model { get; set; } = new DataModel();

        public ObservableCollection<DeviceModel> Devices = new ObservableCollection<DeviceModel>();
    }
}