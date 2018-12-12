using OSSAssessment.DataLayer;
using OSSAssessment.Services;
using OSSAssessment.Windows;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace OSSAssessment.ViewModels
{
    internal class PersonManagementViewModel : BindableBase
    {
        public PersonManagementViewModel()
        {
            GlobalDataModel.Instance.Model.PropertyChanged += Model_PropertyChanged;
        }

        private StructureService structureService;

        public StructureService StructureService
        {
            get
            {
                if (structureService == null)
                {
                    structureService = new StructureService();
                }
                return structureService;
            }
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged("");
        }

        public ObservableCollection<Person> Persons
        {
            get { return GlobalDataModel.Instance.Model.Persons; }
            set
            {
                SetProperty(ref GlobalDataModel.Instance.Model.Persons, value);
            }
        }

        public ActionCommand EditCommand
        {
            get
            {
                return new ActionCommand(EditPersonAction);
            }
        }

        public ActionCommand DeleteCommand
        {
            get
            {
                return new ActionCommand(DeletePersonAction);
            }
        }

        public ActionCommand AddCommand
        {
            get
            {
                return new ActionCommand(AddPersonAction);
            }
        }

        private void AddPersonAction(object data)
        {
            PersonDetailDialog personDetailDialog = new PersonDetailDialog();
            personDetailDialog.Closed += PersonDetailDialog_Closed;
            personDetailDialog.Show();
        }

        private void EditPersonAction(object personId)
        {
            PersonDetailDialog personDetailDialog = new PersonDetailDialog((int)personId);
            personDetailDialog.Closed += PersonDetailDialog_Closed;
            personDetailDialog.Show();
        }

        private void PersonDetailDialog_Closed(object sender, EventArgs e)
        {
            OnPropertyChanged("");
        }

        private void DeletePersonAction(object personId)
        {
            var personName = Persons.Where(x => x.Id == (int)personId).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault();
            if (MessageBox.Show("Do you want delete person \"" + personName + "\" ?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question)
              == MessageBoxResult.Yes)
            {
                DeletePerson((int)personId);
            }
        }

        private void DeletePerson(int id)
        {
            StructureService.RemovePerson(id);
        }

        private int GetNewPersonId()
        {
            int maxId;
            if (Persons.Count > 0)
            {
                maxId = Persons.Select(x => x.Id).Max();
                maxId++;
            }
            else
            {
                maxId = 1;
            }
            return maxId;
        }
    }
}