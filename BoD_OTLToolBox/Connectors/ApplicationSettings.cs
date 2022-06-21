using CsvHelper;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;

namespace BoD_OTLToolBox.Connectors
{
    public class ApplicationSettings
    {
        private Dictionary<string, string> settings;
        private string path;

        public ApplicationSettings()
        {
            settings = new Dictionary<string, string>();
        }

        public string ReadSetting(string key)
        {
            return settings[key];
        }
        

        public void WriteSetting(string key, string value)
        {
            // put in dictionary
            settings[key] = value;
            // convert settings to Setting objects
            List<Setting> temp = new List<Setting>();
            foreach (KeyValuePair<string,string> item in settings)
            {
                temp.Add(new Setting { Id = item.Key, Name = item.Value });
            }
            
            using (var writer = new StreamWriter(path))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(temp);
            }
        }

        // execute only once
        public void Init(string path)
        {
            this.path = path;

            try
            {
               
                using (var reader = new StreamReader(path))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    // get all CSV data
                    var records = csv.GetRecords<Setting>();
                    // move to dictionary
                    foreach (Setting item in records)
                    {
                        settings.Add(item.Id, item.Name);
                    }
                }
            }
            catch
            {
                // file not found - prevent plugin execution at all costs, as wrong parameters will lead to invalid data.
            }

        }
    }
        public class Setting
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
