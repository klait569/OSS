using Newtonsoft.Json;
using OSSAssessment.DataLayer;
using System;
using System.IO;

namespace OSSAssessment.Services
{
    internal class FileService
    {
        public bool SaveDataToFile(DataModel model, string filename = "temp.json")
        {
            try
            {
                string json = JsonConvert.SerializeObject(GlobalDataModel.Instance.Model, Formatting.Indented);
                File.WriteAllText(filename, json);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public DataModel LoadDataFromFile(string filename = "temp.json")
        {
            try
            {
                var json = File.ReadAllText(filename);
                DataModel model = JsonConvert.DeserializeObject<DataModel>(json);
                return model;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
}