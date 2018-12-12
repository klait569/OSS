using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OSSAssessment.DataLayer
{
    public class DataModel //: BindableBase
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public DataModel()
        {
            Structures.CollectionChanged += CollectionChanged;
            Roles.CollectionChanged += CollectionChanged;
            Persons.CollectionChanged += CollectionChanged;
        }

        private void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, null);
        }

        public DataModel(string json)
        {
            DataModel model = JsonConvert.DeserializeObject<DataModel>(json);
            this.Structures = model.Structures;
            this.Roles = model.Roles;
            this.Persons = model.Persons;
        }

        public ObservableCollection<Structure> Structures = new ObservableCollection<Structure>();
        public ObservableCollection<Role> Roles = new ObservableCollection<Role>();
        public ObservableCollection<Person> Persons = new ObservableCollection<Person>();
    }
}