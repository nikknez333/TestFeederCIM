using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Writing;
using VDS.RDF.Writing.Formatting;

namespace CIMProfileLoader.Storage
{
    class HelperMethods
    {
        public static string ExtractDataset(string address)
        {
            string dataset = string.Empty;
            string pattern = @"\/(\w+)\/";

            Match match = Regex.Match(address,pattern);

            if(match.Success && match.Groups.Count > 1)
            {
                dataset = match.Groups[1].Value;
            }
            else
            {
                dataset = string.Empty;
            }

            return dataset;
        }

        public static Process InitializeJenaStorageProccess(Process jenaStorage)
        {
            string directory =  Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string fusekiServerBATDirectory = Path.Combine(directory, "apache-jena-fuseki-4.8.0", "apache-jena-fuseki-4.8.0");

            jenaStorage.StartInfo.WorkingDirectory = fusekiServerBATDirectory;
            jenaStorage.StartInfo.FileName = "cmd.exe";
            jenaStorage.StartInfo.UseShellExecute = false;
            
            return jenaStorage;
        }

        public static void InitializeFusekiConfig(StreamWriter writer)
        {
            writer.WriteLine("PREFIX : \t<#>");
            writer.WriteLine("PREFIX fuseki: \t<http://jena.apache.org/fuseki#>");
            writer.WriteLine("PREFIX rdf: \t<http://www.w3.org/1999/02/22-rdf-syntax-ns#>");
            writer.WriteLine("PREFIX rdfs: \t<http://www.w3.org/2000/01/rdf-schema#>");
            writer.WriteLine("PREFIX ja: \t<http://jena.hpl.hp.com/2005/11/Assembler#>");
            writer.WriteLine("PREFIX tdb: \t<http://jena.apache.org/2016/tdb#>");
            writer.WriteLine();
            writer.WriteLine("[] rdf:type fuseki:Server ;");
            writer.WriteLine("\tfuseki:services (");
            writer.WriteLine("\t)");
            writer.WriteLine(".");  
        }

        public static void AddNewService(string serviceName, string configPath)
        {
            string[] lines = File.ReadAllLines(configPath);
            string targetLine = "fuseki:services";

            using (StreamWriter writer = new StreamWriter(configPath))
            {
                bool foundTargetLine = false;
                int lineIndex = 0;

                while (lineIndex < lines.Length)
                {
                    string line = lines[lineIndex];

                    if (line.Contains(targetLine))
                    {
                        foundTargetLine = true;
                    }

                    if (foundTargetLine && line.Trim() == ")")
                    {
                        writer.Flush();

                        if (lineIndex > 0)
                        {
                            writer.BaseStream.Position -= lines[lineIndex - 1].Length + Environment.NewLine.Length;
                            writer.WriteLine(lines[lineIndex - 1]);
                        }

                        writer.WriteLine("\t" + serviceName);
                    }

                    writer.WriteLine(line);

                    lineIndex++;
                }
            }
        }

        public static void AddSparqlAndGraphStoreProtocolServices(string serviceName, string configPath)
        {
            using(StreamWriter writer = new StreamWriter(configPath, true))
            {
                writer.WriteLine(serviceName + " rdf:type " + "fuseki:Service ;");
                writer.WriteLine("\t" + "fuseki:name " + "\"" + serviceName.Substring(1) + "Dataset" + "\" ;");
                writer.WriteLine("\t" + "fuseki:endpoint" + " [ fuseki:operation " + "fuseki:query ] ;");
                writer.WriteLine("\t" + "fuseki:endpoint" + " [ fuseki:operation " + "fuseki:update ] ;");
                writer.WriteLine("\t" + "fuseki:endpoint" + " [ fuseki:operation " + "fuseki:gsp-rw ] ;");

                writer.WriteLine("\t" + "fuseki:endpoint [");
                writer.WriteLine("\t\t" + "fuseki:operation " + "fuseki:query ;");
                writer.WriteLine("\t\t" + "fuseki:name " + "\"" + "sparql" + "\"");
                writer.WriteLine("\t" + "] ;");

                writer.WriteLine("\t" + "fuseki:endpoint [");
                writer.WriteLine("\t\t" + "fuseki:operation " + "fuseki:query ;");
                writer.WriteLine("\t\t" + "fuseki:name " + "\"" + "query" + "\"");
                writer.WriteLine("\t" + "] ;");

                writer.WriteLine("\t" + "fuseki:endpoint [");
                writer.WriteLine("\t\t" + "fuseki:operation " + "fuseki:update ;");
                writer.WriteLine("\t\t" + "fuseki:name " + "\"" + "update" + "\"");
                writer.WriteLine("\t" + "] ;");

                writer.WriteLine("\t" + "fuseki:endpoint [");
                writer.WriteLine("\t\t" + "fuseki:operation " + "fuseki:gsp-r ;");
                writer.WriteLine("\t\t" + "fuseki:name " + "\"" + "get" + "\"");
                writer.WriteLine("\t" + "] ;");

                writer.WriteLine("\t" + "fuseki:endpoint [");
                writer.WriteLine("\t\t" + "fuseki:operation " + "fuseki:gsp-rw ;");
                writer.WriteLine("\t\t" + "fuseki:name " + "\"" + "data" + "\"");
                writer.WriteLine("\t" + "] ;");

                writer.WriteLine("\t" + "fuseki:dataset" + " :rdfsDataset" + serviceName.Substring(1) +" ;");
                writer.WriteLine("\t" + ".");
            }
        }

        public static void AddInferenceAndCreateDataset(string serviceName, string rdfsFile, string configPath)
        {
            string inferenceModel = ":inferenceModel" + char.ToUpper(Path.GetFileNameWithoutExtension(rdfsFile)[0]) + Path.GetFileNameWithoutExtension(rdfsFile).Substring(1);
            string schemaGraph = ":" + Path.GetFileNameWithoutExtension(rdfsFile) + "Graph";
            using (StreamWriter writer = new StreamWriter(configPath, true)) 
            {
                writer.WriteLine(":rdfsDataset" + serviceName.Substring(1) + " rdf:type " + "ja:RDFDataset ;");
                writer.WriteLine("\t" + "ja:defaultGraph " + inferenceModel + " ;");
                writer.WriteLine("\t" + ".");

                writer.WriteLine();

                writer.WriteLine(inferenceModel + " rdf:type " + "ja:InfModel" + " ;");
                writer.WriteLine("\t" + "ja:baseModel " + schemaGraph + " ;");
                writer.WriteLine("\t" + "ja:reasoner [\n\tja:reasonerURL" + " <http://jena.hpl.hp.com/2003/RDFSExptRuleReasoner>\n");
                writer.WriteLine("\t" + "] .");

                writer.WriteLine();

                writer.WriteLine(schemaGraph + " rdf:type " + "ja:MemoryModel" + " ;");
                writer.WriteLine("\t" + "ja:content " + "[ ja:externalContent <" + rdfsFile.Replace("\\","/").ToLower() + ">" + "] ;");
                writer.WriteLine("\t" + ".");
            }
        }

        public static List<string> ExtractDatasets(string responseData, ref string datasetInfo)
        {
            List<string> datasetList = new List<string>();

            JObject responseJson = JObject.Parse(responseData);
            JArray datasets = responseJson["datasets"] as JArray;
            StringBuilder datasetInfoSB = new StringBuilder();

            foreach (JToken dataset in datasets)
            {
                string datasetName = dataset["ds.name"].ToString().Substring(1);
              
                datasetList.Add(datasetName);
                datasetInfo = datasetInfoSB.AppendLine("\t\tname: " + datasetName).ToString();
                datasetInfo = AddDatasetsInfoJSON(dataset, datasetInfoSB);
            }
            
            return datasetList;
        }

        public static string AddDatasetsInfoJSON(JToken dataset, StringBuilder sb)
        {

            sb.AppendLine("\n\t\tservices: \n");
            JContainer serviceInfos = dataset["ds.services"] as JContainer;
            foreach(JToken serviceInfo in serviceInfos)
            {
                sb.AppendLine("\t\t\ttype: " + serviceInfo["srv.type"].ToString() + "\n");
                sb.AppendLine("\t\t\tdescription: " + serviceInfo["srv.description"].ToString() + "\n");
                JContainer endpoints = serviceInfo["srv.endpoints"] as JContainer;
                sb.AppendLine("\t\t\tendpoints: \n");
                foreach (JToken endpoint in endpoints)
                {                  
                    sb.AppendLine("\t\t\t\t" + endpoint.ToString() + "\n");                  
                }
            }

            return sb.ToString();
        }

        public static List<string> readCurrentConfigOntologies(string configPath)
        {
            List<string> ontologies = new List<string>();
            string ttlContent = File.ReadAllText(configPath);

            string pattern = @"ja:externalContent\s+<([^>]+)>";

            Regex regex = new Regex(pattern);

            MatchCollection matches = regex.Matches(ttlContent);
            
            foreach(Match match in matches)
            {
                string path = match.Groups[1].Value;
                ontologies.Add(path);
            }
            
            return ontologies;
        }

        public static string ConvertToUnixPath(string path)
        {
            return path.Replace('\\', '/').ToLower();
        }
    }
}
