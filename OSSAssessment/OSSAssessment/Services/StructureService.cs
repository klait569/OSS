using OSSAssessment.DataLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OSSAssessment.Services
{
    internal class StructureService
    {
        public Position GetPositionById(int positionId)
        {
            Position result = null;
            foreach (var item in GlobalDataModel.Instance.Model.Structures)
            {
                if (positionId == item.RootPosition.Id)
                {
                    result = item.RootPosition;
                    break;
                }
                result = FindPositionInStructure(positionId, item.RootPosition.SubPositions);
                if (result != null) break;
            }
            return result;
        }

        private Position FindPositionInStructure(int positionId, ObservableCollection<Position> subPositions)
        {
            Position result = null;
            foreach (var item in subPositions)
            {
                if (item.Id == positionId)
                {
                    return item;
                }
                if (item.SubPositions.Count > 0)
                {
                    result = FindPositionInStructure(positionId, item.SubPositions);
                    if (result != null) break;
                }
            }
            return result;
        }

        public List<Position> GetAllPositions()
        {
            List<Position> result = new List<Position>();
            foreach (var item in GlobalDataModel.Instance.Model.Structures)
            {
                result.Add(item.RootPosition);
                if (item.RootPosition.SubPositions.Count > 0)
                {
                    result.AddRange(GetPositions(item.RootPosition.SubPositions));
                }
            }
            return result;
        }

        private List<Position> GetPositions(ObservableCollection<Position> subPositions)
        {
            List<Position> result = new List<Position>();
            foreach (var item in subPositions)
            {
                result.Add(item);
                if (item.SubPositions.Count > 0)
                {
                    result.AddRange(GetPositions(item.SubPositions));
                }
            }
            return result;
        }

        public List<Person> GetUnassignedPersons()
        {
            var allPositions = GetAllPositions();
            var assignedPersonsIds = allPositions.Select(x => x.PersonId).ToList();
            var unassignedPersons = from item in GlobalDataModel.Instance.Model.Persons
                                    where !assignedPersonsIds.Contains(item.Id)
                                    select item;
            return unassignedPersons.ToList();
        }

        internal void RemovePerson(int id)
        {
            var person = GlobalDataModel.Instance.Model.Persons.Select(x => x).Where(x => x.Id == id).FirstOrDefault();
            GlobalDataModel.Instance.Model.Persons.Remove(person);
            foreach (var item in GetAllPositions())
            {
                if (item.PersonId == id)
                {
                    item.PersonId = 0;
                }
            }
        }

        public int GetNewPositionId()
        {
            int maxId;
            var allPositions = GetAllPositions();
            if (allPositions.Count > 0)
            {
                maxId = allPositions.Select(x => x.Id).Max();
                maxId++;
            }
            else
            {
                maxId = 1;
            }
            return maxId;
        }

        public int GetNewStructureId()
        {
            int maxId;
            if (GlobalDataModel.Instance.Model.Structures.Count > 0)
            {
                maxId = GlobalDataModel.Instance.Model.Structures.Select(x => x.Id).Max();
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