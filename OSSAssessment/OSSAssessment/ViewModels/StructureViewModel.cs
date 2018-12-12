using OSSAssessment.DataLayer;
using OSSAssessment.Services;
using OSSAssessment.Windows;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace OSSAssessment.ViewModels
{
    internal class StructureViewModel : BindableBase
    {
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

        public ObservableCollection<Structure> Structures
        {
            get { return GlobalDataModel.Instance.Model.Structures; }
            set
            {
                SetProperty(ref GlobalDataModel.Instance.Model.Structures, value);
            }
        }

        private object selectedNode;

        public object SelectedNode
        {
            get { return selectedNode; }
            set
            {
                SetProperty(ref selectedNode, value);
                OnPropertyChanged("IsSelectedNode");
            }
        }

        public bool IsSelectedNode
        {
            get
            {
                if (SelectedNode != null)
                    return true;
                else
                    return false;
            }
        }

        public ActionCommand AddRootNodeCommand
        {
            get
            {
                return new ActionCommand(AddRootNodeAction);
            }
        }

        public ActionCommand AddNodeCommand
        {
            get
            {
                return new ActionCommand(AddNodeAction);
            }
        }

        public ActionCommand ClearStructureCommand
        {
            get
            {
                return new ActionCommand(ClearStructureAction);
            }
        }

        public ActionCommand DeleteNodeCommand
        {
            get
            {
                return new ActionCommand(DeleteNodeAction);
            }
        }

        public ActionCommand EditNodeCommand
        {
            get
            {
                return new ActionCommand(EditNodeAction);
            }
        }

        private void EditNodeAction(object data)
        {
            Position position = null;
            if (SelectedNode is Structure)
            {
                position = ((Structure)SelectedNode).RootPosition;
            }
            else
            {
                position = (Position)SelectedNode;
            }

            NodeDetailDialog win2 = new NodeDetailDialog(position.Id);
            win2.Closed += NodeDialog_Closed;
            win2.Show();
        }

        private void NodeDialog_Closed(object sender, EventArgs e)
        {
            OnPropertyChanged("");
            if (SelectedNode != null)
            {
                if (SelectedNode is Structure)
                {
                    ((Structure)SelectedNode).RootPosition.UpdateUi();
                }
                else
                {
                    ((Position)SelectedNode).UpdateUi();
                }
            }
        }

        private void DeleteNodeAction(object data)
        {
            string nodeName = string.Empty;
            if (SelectedNode is Structure)
            {
                nodeName = ((Structure)SelectedNode).Name;
            }
            else
            {
                nodeName = ((Position)SelectedNode).Name;
            }

            if (MessageBox.Show("Do you want delete position \"" + nodeName + "\" ?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (SelectedNode is Structure)
                {
                    Structures.Remove((Structure)SelectedNode);
                }
                else
                {
                    Position position = (Position)SelectedNode;
                    var allPositions = StructureService.GetAllPositions();
                    var parent = allPositions.Select(x => x).Where(x => x.SubPositions.Contains(position)).FirstOrDefault();
                    if (parent != null)
                    {
                        parent.SubPositions.Remove(position);
                    }
                }
            }
        }

        private void ClearStructureAction(object data)
        {
            Structures.Clear();
        }

        private void AddRootNodeAction(object data)
        {
            NodeDetailDialog nodeDetailDialog = new NodeDetailDialog(0, 0);
            nodeDetailDialog.Closed += NodeDialog_Closed;
            nodeDetailDialog.Show();
        }

        private void AddNodeAction(object data)
        {
            Position position = null;
            if (SelectedNode is Structure)
            {
                position = ((Structure)SelectedNode).RootPosition;
            }
            else
            {
                position = (Position)SelectedNode;
            }

            int parentId = 0;
            if (position != null)
            {
                parentId = position.Id;
            }

            NodeDetailDialog nodeDetailDialog = new NodeDetailDialog(0, parentId);
            nodeDetailDialog.Closed += NodeDialog_Closed;
            nodeDetailDialog.Show();
        }
    }
}