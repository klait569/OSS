using OSSAssessment.DataLayer;
using OSSAssessment.Services;
using System.Linq;
using System.Windows;

namespace OSSAssessment.ViewModels
{
    internal class PersonDetailDialogViewModel : BindableBase
    {
        private readonly int personId;

        public PersonDetailDialogViewModel(int personId)
        {
            this.personId = personId;

            if (personId == 0)
            {
                AddMode = true;
            }
            else
            {
                var person = GlobalDataModel.Instance.Model.Persons.Select(x => x).Where(x => x.Id == personId).FirstOrDefault();
                LastName = person.LastName;
                FirstName = person.FirstName;
            }
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

        private bool addMode = false;

        public bool AddMode
        {
            get { return addMode; }
            set
            {
                SetProperty(ref addMode, value);
            }
        }

        private string firstName;

        public string FirstName
        {
            get { return firstName; }
            set
            {
                SetProperty(ref firstName, value);
            }
        }

        private string lastName;

        public string LastName
        {
            get { return lastName; }
            set
            {
                SetProperty(ref lastName, value);
            }
        }

        public ActionCommand SaveCommand
        {
            get
            {
                return new ActionCommand(SaveAction);
            }
        }

        private void SaveAction(object window)
        {
            if (AddMode)
            {
                var person = new Person()
                {
                    Id = GetNewPersonId(),
                    FirstName = FirstName,
                    LastName = LastName
                };
                GlobalDataModel.Instance.Model.Persons.Add(person);
            }
            else
            {
                var person = GlobalDataModel.Instance.Model.Persons.Select(x => x).Where(x => x.Id == personId).FirstOrDefault();
                person.FirstName = FirstName;
                person.LastName = LastName;
                person.UpdateUi();
                foreach (var item in StructureService.GetAllPositions())
                {
                    if (item.PersonId == person.Id)
                    {
                        item.UpdateUi();
                    }
                }
            }

            if (window != null)
            {
                ((Window)window).Close();
            }
        }

        //move to reole service
        private int GetNewPersonId()
        {
            int maxId;
            if (GlobalDataModel.Instance.Model.Persons.Count > 0)
            {
                maxId = GlobalDataModel.Instance.Model.Persons.Select(x => x.Id).Max();
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