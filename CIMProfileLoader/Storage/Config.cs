using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Configuration;
using VDS.RDF.Storage;

namespace CIMProfileLoader.Storage
{
    public class Config
    {
        private string solutionFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        private string configPath = string.Empty;

        public static Config instance = null;

        public static Config Instance 
        { 
            get
            {
                if(instance == null)
                {
                    instance = new Config();
                }

                return instance;
            }
        }

        public string GetConfigPath()
        {
            configPath = Path.Combine(solutionFolder, "apache-jena-fuseki-4.8.0", "apache-jena-fuseki-4.8.0", "run", "config.ttl");
            return ConfigPath;
        }

        public string ConfigPath 
        { 
            get
            {
                return configPath;
            }
        }

        /*public IGraph Configuration
        { 
            get
            {
                return configuration;
            }
        }

        public void LoadConfiguration()
        {
            configuration = ConfigurationLoader.LoadConfiguration("fusekiConfig.ttl");
        }*/

    }
}
