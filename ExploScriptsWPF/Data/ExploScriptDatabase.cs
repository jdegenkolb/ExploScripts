using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExploScripts.Data
{
    class ExploScriptDatabase
    {
        private const string DBFILE = "Scripts.xml";

        public bool NewInstall { get; private set; }

        [XmlArray("ExploScriptList")]
        public ObservableCollection<ExploScript> Scripts { get; private set; }

        public ExploScriptDatabase()
        {
            NewInstall = !File.Exists(DBFILE);
        }

        public void Initialize()
        {
            if (NewInstall)
            {
                Scripts = new ObservableCollection<ExploScript>();
                SaveDatabase();
            }
            else
            {
                LoadDatabase();
            }
        }

        private void LoadDatabase()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<ExploScript>));
            using (StreamReader reader = new StreamReader(DBFILE))
            {
                Scripts = (ObservableCollection<ExploScript>)serializer.Deserialize(reader);
            }
        }

        public void SaveDatabase()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<ExploScript>));
            using (StreamWriter writer = new StreamWriter(DBFILE))
            {
                serializer.Serialize(writer, Scripts);
            }
        }
    }
}
