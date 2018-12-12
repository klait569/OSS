using System.Collections.ObjectModel;
using System.Linq;

namespace OSSAssessment.DataLayer
{
    public class Position : BindableBase
    {
        private int id;

        public int Id
        {
            get { return id; }
            set
            {
                SetProperty(ref id, value);
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
                OnPropertyChanged("PersonFullName");
                OnPropertyChanged("PersonRole");
            }
        }

        public ObservableCollection<Position> SubPositions { get; set; } = new ObservableCollection<Position>();

        public string PersonFullName
        {
            get
            {
                var fullname = GlobalDataModel.Instance.Model.Persons.Where(x => x.Id == PersonId).Select(x => x.FullName).FirstOrDefault();
                if (fullname != null)
                {
                    return fullname;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string PersonRole
        {
            get
            {
                var rolename = GlobalDataModel.Instance.Model.Persons.Where(x => x.Id == PersonId).Select(x => x.RoleName).FirstOrDefault();
                if (rolename != null)
                {
                    return rolename;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public void UpdateUi()
        {
            OnPropertyChanged("");
        }
    }

    public class Structure : BindableBase
    {
        private int id;

        public int Id
        {
            get { return id; }
            set
            {
                SetProperty(ref id, value);
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

        private Position rootPosition;

        public Position RootPosition
        {
            get { return rootPosition; }
            set
            {
                SetProperty(ref rootPosition, value);
            }
        }
    }

    public class Person : BindableBase
    {
        private int id;

        public int Id
        {
            get { return id; }
            set
            {
                SetProperty(ref id, value);
            }
        }

        private string firstName;

        public string FirstName
        {
            get { return firstName; }
            set
            {
                SetProperty(ref firstName, value);
                OnPropertyChanged("FullName");
            }
        }

        private string lastName;

        public string LastName
        {
            get { return lastName; }
            set
            {
                SetProperty(ref lastName, value);
                OnPropertyChanged("FullName");
            }
        }

        private int? roleId;

        public int? RoleId
        {
            get { return roleId; }
            set
            {
                SetProperty(ref roleId, value);
                OnPropertyChanged("RoleName");
            }
        }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public string RoleName
        {
            get
            {
                if (RoleId != null)
                {
                    return GlobalDataModel.Instance.Model.Roles.Where(x => x.Id == RoleId).Select(x => x.Name).FirstOrDefault();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public void UpdateUi()
        {
            OnPropertyChanged("");
        }
    }

    public class Role : BindableBase
    {
        private int id;

        public int Id
        {
            get { return id; }
            set
            {
                SetProperty(ref id, value);
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

        private string description;

        public string Description
        {
            get { return description; }
            set
            {
                SetProperty(ref description, value);
            }
        }
    }
}