using OSSAssessment.DataLayer;
using OSSAssessment.Windows;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace OSSAssessment.ViewModels
{
    internal class RoleManagementViewModel : BindableBase
    {
        public RoleManagementViewModel()
        {
            GlobalDataModel.Instance.Model.PropertyChanged += Model_PropertyChanged;
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged("");
        }

        public ObservableCollection<Role> Roles
        {
            get { return GlobalDataModel.Instance.Model.Roles; }
            set
            {
                SetProperty(ref GlobalDataModel.Instance.Model.Roles, value);
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

        public ActionCommand EditCommand
        {
            get
            {
                return new ActionCommand(EditRoleAction);
            }
        }

        public ActionCommand DeleteCommand
        {
            get
            {
                return new ActionCommand(DeleteRoleAction);
            }
        }

        public ActionCommand AddCommand
        {
            get
            {
                return new ActionCommand(AddRoleAction);
            }
        }

        private void AddRoleAction(object data)
        {
            RoleDetailDialog roleDialog = new RoleDetailDialog();
            roleDialog.Closed += RoleDialog_Closed;
            roleDialog.Show();
        }

        private void EditRoleAction(object roleId)
        {
            RoleDetailDialog roleDialog = new RoleDetailDialog((int)roleId);
            roleDialog.Closed += RoleDialog_Closed;
            roleDialog.Show();
        }

        private void RoleDialog_Closed(object sender, EventArgs e)
        {
            OnPropertyChanged("");
        }

        private void DeleteRoleAction(object roleId)
        {
            var roleName = Roles.Where(x => x.Id == (int)roleId).Select(x => x.Name).FirstOrDefault();
            if (MessageBox.Show("Do you want delete role \"" + roleName + "\" ?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question)
              == MessageBoxResult.Yes)
            {
                DeleteRole((int)roleId);
            }
        }

        private void DeleteRole(int id)
        {
            var role = Roles.Select(x => x).Where(x => x.Id == id).FirstOrDefault();
            Roles.Remove(role);
        }

        private int GetNewRoleId()
        {
            int maxId;
            if (Roles.Count > 0)
            {
                maxId = Roles.Select(x => x.Id).Max();
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