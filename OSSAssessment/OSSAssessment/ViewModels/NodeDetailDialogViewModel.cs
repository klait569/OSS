using OSSAssessment.DataLayer;
using OSSAssessment.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace OSSAssessment.ViewModels
{
    internal class NodeDetailDialogViewModel : BindableBase
    {
        private readonly int positionId;
        private readonly int parentPositionId;

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

        public NodeDetailDialogViewModel(int positionId, int parentPositionId)
        {
            this.positionId = positionId;
            this.parentPositionId = parentPositionId;
            Persons.Clear();

            if (positionId == 0)
            {
                AddMode = true;
                if (parentPositionId == 0)
                {
                    StructureMode = true;
                }
            }
            else
            {
                foreach (var item in GlobalDataModel.Instance.Model.Structures)
                {
                    if (item.RootPosition.Id == positionId)
                    {
                        StructureName = item.Name;
                        StructureMode = true;
                        break;
                    }
                }

                var position = StructureService.GetPositionById(positionId);
                Name = position.Name;
                PersonId = position.PersonId;
                var person = GlobalDataModel.Instance.Model.Persons.Select(x => x).Where(x => x.Id == PersonId).FirstOrDefault();
                if (person != null)
                {
                    Persons.Add(person);
                    SelectedPerson = person;
                    SelectedRole = Roles.Select(x => x).Where(x => x.Id == person.RoleId).FirstOrDefault();
                }
            }

            foreach (var item in StructureService.GetUnassignedPersons())
            {
                Persons.Add(item);
            }
        }

        public ObservableCollection<Role> Roles
        {
            get { return GlobalDataModel.Instance.Model.Roles; }
            set
            {
                SetProperty(ref GlobalDataModel.Instance.Model.Roles, value);
            }
        }

        private ObservableCollection<Person> persons = new ObservableCollection<Person>();

        public ObservableCollection<Person> Persons
        {
            get { return persons; }
            set
            {
                SetProperty(ref persons, value);
            }
        }

        private Person selectedPerson = new Person();

        public Person SelectedPerson
        {
            get { return selectedPerson; }
            set
            {
                SetProperty(ref selectedPerson, value);
                OnPropertyChanged("IsAssignedPerson");
            }
        }

        private Role selectedRole;

        public Role SelectedRole
        {
            get { return selectedRole; }
            set
            {
                SetProperty(ref selectedRole, value);
            }
        }

        private bool structureMode = false;

        public bool StructureMode
        {
            get { return structureMode; }
            set
            {
                SetProperty(ref structureMode, value);
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

        public bool IsAssignedPerson
        {
            get
            {
                bool result = ((SelectedPerson != null) && (SelectedPerson.Id != 0));
                return result;
            }
        }

        private string structureName;

        public string StructureName
        {
            get { return structureName; }
            set
            {
                SetProperty(ref structureName, value);
            }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                SetProperty(ref name, value);
            }
        }

        private int personId;

        public int PersonId
        {
            get { return personId; }
            set
            {
                SetProperty(ref personId, value);
            }
        }

        public ActionCommand UnassignPersonCommand
        {
            get
            {
                return new ActionCommand(UnassignPersonAction);
            }
        }

        public ActionCommand SaveCommand
        {
            get
            {
                return new ActionCommand(SaveAction);
            }
        }

        private void UnassignPersonAction(object data)
        {
            SelectedPerson = null;
        }

        private void SaveAction(object window)
        {
            if (AddMode)
            {
                var position = new Position()
                {
                    Id = StructureService.GetNewPositionId(),
                    Name = Name,
                    PersonId = SelectedPerson.Id
                };

                if (SelectedRole != null)
                {
                    SelectedPerson.RoleId = SelectedRole.Id;
                }

                if (parentPositionId > 0)
                {
                    var parentPosition = StructureService.GetPositionById(parentPositionId);
                    parentPosition.SubPositions.Add(position);
                }
                else
                {
                    var structure = new Structure()
                    {
                        Id = StructureService.GetNewStructureId(),
                        Name = StructureName,
                        RootPosition = position
                    };
                    GlobalDataModel.Instance.Model.Structures.Add(structure);
                }
            }
            else
            {
                if (StructureMode)
                {
                    var structure = GlobalDataModel.Instance.Model.Structures.Select(x => x).Where(x => x.RootPosition.Id == positionId).FirstOrDefault();
                    structure.Name = StructureName;
                }

                var position = StructureService.GetPositionById(positionId);
                position.Name = Name;
                position.PersonId = SelectedPerson != null ? SelectedPerson.Id : 0;
                if ((SelectedRole != null) && (SelectedPerson != null))
                {
                    SelectedPerson.RoleId = SelectedRole.Id;
                }
            }

            if (window != null)
            {
                ((Window)window).Close();
            }
        }
    }
}