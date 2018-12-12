using OSSAssessment.DataLayer;
using OSSAssessment.Services;
using System.Linq;
using System.Windows;

namespace OSSAssessment.ViewModels
{
    internal class RoleDetailDialogViewModel : BindableBase
    {
        private readonly int roleId;

        public RoleDetailDialogViewModel(int roleId)
        {
            this.roleId = roleId;

            if (roleId == 0)
            {
                AddMode = true;
            }
            else
            {
                var role = GlobalDataModel.Instance.Model.Roles.Select(x => x).Where(x => x.Id == roleId).FirstOrDefault();
                Description = role.Description;
                Name = role.Name;
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
                var role = new Role()
                {
                    Id = GetNewRoleId(),
                    Name = Name,
                    Description = Description
                };
                GlobalDataModel.Instance.Model.Roles.Add(role);
            }
            else
            {
                var role = GlobalDataModel.Instance.Model.Roles.Select(x => x).Where(x => x.Id == roleId).FirstOrDefault();
                role.Name = Name;
                role.Description = Description;
                StructureService.UpdatePositionsWithRole(role.Id);
            }

            if (window != null)
            {
                ((Window)window).Close();
            }
        }

        private int GetNewRoleId()
        {
            int maxId;
            if (GlobalDataModel.Instance.Model.Roles.Count > 0)
            {
                maxId = GlobalDataModel.Instance.Model.Roles.Select(x => x.Id).Max();
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