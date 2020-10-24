using System;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;

namespace Deadliner
{
    class SaveLoad
    {
        private readonly string _path = $"{Environment.CurrentDirectory}\\tasksList.json";

        public void Save(BindingList<Task> tasks)
        {
            using (StreamWriter sw = File.CreateText(_path))
            {
                string output = JsonConvert.SerializeObject(tasks);
                sw.Write(output);
            }
        }

        public BindingList<Task> Load()
        {
            if (!File.Exists(_path))
            {
                File.CreateText(_path).Dispose();
                return new BindingList<Task>();
            }

            using (StreamReader sr = File.OpenText(_path))
            {
                string fileText = sr.ReadToEnd();
                return JsonConvert.DeserializeObject<BindingList<Task>>(fileText);
            }
        }
    }
}
