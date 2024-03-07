using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Configuration;
using VDS.RDF.Storage;
using CIMProfileLoader.Core;
using System.IO;
using VDS.RDF.Parsing;
using System.Net.NetworkInformation;
using System.Net;
using VDS.RDF.Query;
using System.Diagnostics;
using System.Net.Http;
using System.Windows.Forms;
using System.Net.Sockets;
using Newtonsoft.Json.Linq;

namespace CIMProfileLoader.Storage
{
    public enum NodeFlag
    {
        Subject = 0,
        Predicate = 1,
        Object = 2
    }
    public class FusekiStorageAPI
    {
        private FusekiConnector fuseki = null;
        private string graphName = string.Empty;
        private bool savedSuccesfully = false;
        private string baseURI = "http://iec.ch/TC57/2010/CIM-schema-cim17/";
        //private GenericQueryProcessor queryProcessor = null;
        List<string> currentOntologies = new List<string>();
        private string configPath = Config.Instance.GetConfigPath();
        private const string baseServiceUri = "http://localhost:3030/";
        private Dictionary<string, string> graphURIs = new Dictionary<string, string>(); 
        private Graph preSavedNamespaces = new Graph();

        public void ConnectToDatasetEndpoint(string selectedDataset)
        {
            if (fuseki == null)
            {
                fuseki = new FusekiConnector(baseServiceUri + selectedDataset + "/data");
            }
            else
            {
                string currentDatasetConnected = HelperMethods.ExtractDataset(fuseki.ToString());

                if (!currentDatasetConnected.Equals(selectedDataset))
                {
                    fuseki = null;
                    fuseki = new FusekiConnector("http://localhost:3030/" + selectedDataset + "/data");
                }
            }
        }

        public string ResetFusekiConfig()
        {
            string msg = string.Empty;

            try
            {
                using (FileStream fs = File.Open(configPath, FileMode.Truncate, FileAccess.Write))
                {
                    fs.SetLength(0);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            try
            {

                using (StreamWriter writer = new StreamWriter(configPath, true))
                {
                    HelperMethods.InitializeFusekiConfig(writer);                    
                }

                msg = File.ReadAllText(configPath);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return msg;
        }

        public string UpdateFusekiConfig(string rdfsFile)
        {
            string msg = string.Empty;

            currentOntologies = HelperMethods.readCurrentConfigOntologies(configPath);
            string rdfsUnixPath = HelperMethods.ConvertToUnixPath(rdfsFile);

            if (!currentOntologies.Any() || !currentOntologies.Contains(rdfsUnixPath))
            {
                try
                {
                    string serviceName = ":" + Path.GetFileNameWithoutExtension(rdfsFile) + "Service";
                    HelperMethods.AddNewService(serviceName, configPath);
                    HelperMethods.AddSparqlAndGraphStoreProtocolServices(serviceName, configPath);
                    HelperMethods.AddInferenceAndCreateDataset(serviceName, rdfsFile, configPath);
                    msg = File.ReadAllText(configPath);
                    
                    //currentOntologies.Add(rdfsFile);

                    //msg = string.Format("successfully updated fuseki config with sparql and graph store protocol services and added inference and reasoning for dataset: {0}", serviceName + "Dataset");
                }
                catch (Exception e)
                {
                    msg = String.Format("ERROR: Could not update fuseki config file, exception: {0}", e.Message);
                    return msg;
                }
            }
            else
            {
                msg = string.Format("ERROR: Schema file is already present as ontology of some dataset.Please choose another schema file.");
            }
            return msg;
        }

        public bool IsFusekiOnline(ref string responseMessage)
        {
            string pingFusekiUrl = "http://localhost:3030/$/ping";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = client.GetAsync(pingFusekiUrl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string pingResponse = response.Content.ReadAsStringAsync().Result;
                        DateTimeOffset pingTimestamp = DateTimeOffset.Parse(pingResponse);
                        return true;
                    }
                    else
                    {
                        responseMessage = $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                        return false;
                    }
                }
                catch (HttpRequestException ex)
                {
                    responseMessage = $"HttpRequestException occurred: {ex.Message}";
                    return false;
                }
                catch (InvalidOperationException ex)
                {
                    responseMessage = $"InvalidOperationException occurred: {ex.Message}";
                    return false;
                }
                catch (Exception ex)
                {
                    responseMessage = $"Exception occurred: {ex.Message}";
                    return false;
                }
            }
        }

        public void StartFusekiStore()
        {
            Process jenaStorage = new Process();
            jenaStorage = HelperMethods.InitializeJenaStorageProccess(jenaStorage);

            string command = "fuseki-server";
            jenaStorage.StartInfo.Arguments = "/k " + command;

            jenaStorage.Start();

            File.WriteAllText("jenaProcessId.txt", jenaStorage.Id.ToString());
        }

        public void StopFusekiStore()
        {
            int jenaStorageId;
            if (File.Exists("jenaProcessId.txt") && int.TryParse(File.ReadAllText("jenaProcessId.txt"), out jenaStorageId))
            {
                try
                {
                    Process jenaStorage = Process.GetProcessById(jenaStorageId);

                    if (!jenaStorage.CloseMainWindow())
                    {
                        jenaStorage.Kill();
                    }
                }
                catch (ArgumentException)
                {

                }
                catch (Exception)
                {

                }
            }
        }

        public List<string> GetAllDatasetsFromFuseki(ref string errorMessage, ref string datasetInfo)
        {
            string getDatasetsURL = "http://localhost:3030/$/datasets";

            string parsedJSON = string.Empty;

            List<string> datasetList = new List<string>();
            JArray datasetsListJSON = new JArray();

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = client.GetAsync(getDatasetsURL).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = response.Content.ReadAsStringAsync().Result;
                        datasetList = HelperMethods.ExtractDatasets(responseData, ref datasetInfo);
                        //parsedJSON = HelperMethodsJSON(datasetsListJSON);
                    }
                    else
                    {
                        errorMessage = $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                    }
                }
                catch (HttpRequestException ex)
                {
                    errorMessage = $"HttpRequestException occurred: {ex.Message}";
                }
                catch (InvalidOperationException ex)
                {
                    errorMessage = $"InvalidOperationException occurred: {ex.Message}";
                }
                catch (Exception ex)
                {
                    errorMessage = $"Exception occurred: {ex.Message}";
                }
            }

            return datasetList;
        }

        public Graph LoadGraph(string selectedDataset, string graphName)
        {
            ConnectToDatasetEndpoint(selectedDataset);
            Graph loadedGraph = new Graph();
            if (graphName == "default")
            {
                fuseki.LoadGraph(loadedGraph, string.Empty);
            }
            else
            {
                fuseki.LoadGraph(loadedGraph, graphURIs[graphName] + graphName);
            }
            foreach (string prefix in preSavedNamespaces.NamespaceMap.Prefixes)
            {
                loadedGraph.NamespaceMap.AddNamespace(prefix, preSavedNamespaces.NamespaceMap.GetNamespaceUri(prefix));
            }

            return loadedGraph;
        }

        public IEnumerable<Uri> ListGraphs(string selectedDataset)
        {
            IEnumerable<Uri> graphs;

            ConnectToDatasetEndpoint(selectedDataset);

            if (fuseki.ListGraphsSupported)
            {
                try
                {
                    graphs = fuseki.ListGraphs();
                }
                catch (RdfStorageException)
                {
                    return Enumerable.Empty<Uri>();
                }
            }
            else
            {
                graphs = null;
            }

            return graphs;
        }

        public bool UpdateGraph(string selectedDataset, string graphName, string _subject, string _predicate, string _object, bool addTriple, bool removeTriple)
        {
            bool updated = false;
            List<String> currentNamespacePrefixes = null;
            List<String> prefixes = null;
            List<String> tripleFullnames = new List<string>() { _subject, _predicate, _object };

            ConnectToDatasetEndpoint(selectedDataset);

            if (fuseki.UpdateSupported)
            {
                if (addTriple)
                {
                    currentNamespacePrefixes = GetAllNamespacesPrefixesForGivenGraph(selectedDataset, graphName);
                    prefixes = extractNamespacePrefixes(tripleFullnames);

                    Graph g = new Graph();
                    Graph selectedGraph = LoadGraph(selectedDataset, graphName);

                    foreach (string prefix in prefixes)
                    {
                        if (!currentNamespacePrefixes.Contains(prefix)) //check namespace prefix
                        {
                            if (!string.IsNullOrEmpty(prefix))
                            {
                                selectedGraph.NamespaceMap.AddNamespace(prefix, new Uri("http://wwww." + prefix + ".org/#"));
                            }
                        }
                    }

                    foreach (string prefix in selectedGraph.NamespaceMap.Prefixes)
                    {
                        if (!g.NamespaceMap.Prefixes.Contains(prefix))
                        {
                            g.NamespaceMap.AddNamespace(prefix, selectedGraph.NamespaceMap.GetNamespaceUri(prefix));
                        }
                    }

                    INode subjectNode = CreateNodeBasedOnType(g, _subject, NodeFlag.Subject);
                    INode predicateNode = CreateNodeBasedOnType(g, _predicate, NodeFlag.Predicate);
                    INode objectNode = CreateNodeBasedOnType(g, _object, NodeFlag.Object);
                    if (subjectNode != null && predicateNode != null && objectNode != null)
                    {
                        Triple t = new Triple(subjectNode, predicateNode, objectNode);

                        try
                        {
                            fuseki.UpdateGraph(string.Empty, new Triple[] { t }, null);
                            fuseki.UpdateGraph(selectedGraph.BaseUri, new Triple[] { t }, null);
                            updated = true;
                        }
                        catch (RdfStorageException ex)
                        {
                            string msg = ex.Message;
                            updated = false;
                        }
                    }
                    else
                    {
                        updated = false;
                    }
                }
                else if (removeTriple)
                {
                    Graph g = new Graph();
                    Graph selectedGraph = LoadGraph(selectedDataset, graphName);

                    foreach (string prefix in selectedGraph.NamespaceMap.Prefixes)
                    {
                        if (!g.NamespaceMap.Prefixes.Contains(prefix))
                        {
                            g.NamespaceMap.AddNamespace(prefix, selectedGraph.NamespaceMap.GetNamespaceUri(prefix));
                        }
                    }

                    INode subjectNode = CreateNodeBasedOnType(g, _subject, NodeFlag.Subject);
                    INode predicateNode = CreateNodeBasedOnType(g, _predicate, NodeFlag.Predicate);
                    INode objectNode = CreateNodeBasedOnType(g, _object, NodeFlag.Object);

                    if (subjectNode != null && predicateNode != null && objectNode != null)
                    {
                        Triple t = new Triple(subjectNode, predicateNode, objectNode);

                        if (selectedGraph.Triples.Contains(t))
                        {
                            fuseki.UpdateGraph(string.Empty, null, new Triple[] { t });
                            fuseki.UpdateGraph(selectedGraph.BaseUri, null, new Triple[] { t });
                            updated = true;
                        }
                        else
                        {
                            updated = false;
                        }
                    }
                    else
                    {
                        updated = false;
                    }
                }
            }
            else
            {
                updated = false;
            }

            return updated;
        }


        private INode CreateNodeBasedOnType(Graph g, string nodeValue, NodeFlag nodeFlag)
        {
            INode node;

            switch (nodeFlag)
            {
                case NodeFlag.Subject:
                    if (Uri.IsWellFormedUriString(nodeValue, UriKind.Absolute) || IsQName(nodeValue))
                    {
                        if (IsQName(nodeValue))
                            node = g.CreateUriNode(nodeValue);
                        else
                            node = g.CreateUriNode(new Uri(nodeValue));
                    }
                    else if (string.IsNullOrWhiteSpace(nodeValue) || nodeValue.StartsWith("_:"))
                    {
                        if (nodeValue.StartsWith("_:"))
                            node = g.CreateBlankNode(nodeValue.Substring(2));
                        else
                            node = g.CreateBlankNode();
                    }
                    else
                    {
                        node = null;
                    }
                    return node;
                case NodeFlag.Predicate:
                    if (Uri.IsWellFormedUriString(nodeValue, UriKind.Absolute) || IsQName(nodeValue))
                    {
                        if (IsQName(nodeValue))
                            node = g.CreateUriNode(nodeValue);
                        else
                            node = g.CreateUriNode(new Uri(nodeValue));
                    }
                    else
                    {
                        node = null;
                    }
                    return node;
                case NodeFlag.Object:
                    if (Uri.IsWellFormedUriString(nodeValue, UriKind.Absolute) || IsQName(nodeValue))
                    {
                        if (IsQName(nodeValue))
                            node = g.CreateUriNode(nodeValue);
                        else
                            node = g.CreateUriNode(new Uri(nodeValue));
                    }
                    else if (string.IsNullOrWhiteSpace(nodeValue) || nodeValue.StartsWith("_:"))
                    {
                        if (nodeValue.StartsWith("_:"))
                            node = g.CreateBlankNode(nodeValue.Substring(2));
                        else
                            node = g.CreateBlankNode();
                    }
                    else
                    {
                        node = g.CreateLiteralNode(nodeValue);
                    }
                    return node;
                default:
                    node = null;
                    return node;
            }
        }

        private bool IsQName(string qname)
        {
            string[] splitted = qname.Split(':');

            if (splitted.Length != 2)
            {
                return false;
            }

            return true;
        }

        private List<string> GetAllNamespacesPrefixesForGivenGraph(string selectedDataset, string graphName)
        {
            Graph g = LoadGraph(selectedDataset, graphName);
            List<string> prefixes = new List<string>();

            foreach (string prefix in g.NamespaceMap.Prefixes)
            {
                prefixes.Add(prefix);
            }
            return prefixes;
        }

        private List<string> extractNamespacePrefixes(List<string> fullnames)
        {
            List<string> prefixes = new List<string>();
            foreach (string fullname in fullnames)
            {
                string[] splitted = fullname.Split(':');
                if (splitted.Length == 2)
                    prefixes.Add(splitted[0]);
                else
                    prefixes.Add("");
            }
            return prefixes;
        }

        public bool DeleteGraph(string selectedDataset, string graphName)
        {
            bool deleted = false;
            Graph loadedGraph = new Graph();
            ConnectToDatasetEndpoint(selectedDataset);

            if (fuseki.DeleteSupported)
            {
                fuseki.LoadGraph(loadedGraph, graphURIs[graphName] + graphName);
                fuseki.UpdateGraph(string.Empty, null, loadedGraph.Triples);

                fuseki.DeleteGraph(baseURI + graphName);
                deleted = true;
                graphURIs.Remove(graphName);
            }
            else
            {
                deleted = false;

            }

            return deleted;
        }

        public bool SaveGraph(string selectedDataset, Graph data, string filename)
        {

            ConnectToDatasetEndpoint(selectedDataset);

            graphName = Path.GetFileNameWithoutExtension(filename);
            if (data.BaseUri == null)
                data.BaseUri = new Uri(data.NamespaceMap.GetNamespaceUri("cim") + graphName);
            int baseUriIndex = data.BaseUri.ToString().IndexOf('#');
            if (baseUriIndex != -1)
                baseURI = data.BaseUri.ToString().Substring(0, baseUriIndex + 1);

            if (!graphURIs.ContainsKey(graphName))
                graphURIs.Add(graphName, baseURI);
            else
                return false;
            
            if (!fuseki.IsReadOnly)
            {
                fuseki.SaveGraph(data);
                foreach (string prefix in data.NamespaceMap.Prefixes)
                {
                    preSavedNamespaces.NamespaceMap.AddNamespace(prefix, data.NamespaceMap.GetNamespaceUri(prefix));
                }

                fuseki.UpdateGraph(string.Empty, data.Triples, null);

                savedSuccesfully = true;
            }
            else
            {
                savedSuccesfully = false;
            }

            return savedSuccesfully;
        }


        #region Query

        private GenericQueryProcessor GetQueryProcessor()
        {
            GenericQueryProcessor processor = new GenericQueryProcessor(fuseki);

            return processor;
        }

        public SparqlResultSet GetClassesWithAttributes(string className, string selectedDataset, string graphName)
        {
            ConnectToDatasetEndpoint(selectedDataset);

            Graph selectedGraph = LoadGraph(selectedDataset, graphName);
            Graph defaultGraph = LoadGraph(selectedDataset, "default");
            SparqlQueryParser sparqlParser = new SparqlQueryParser();
            SparqlParameterizedString cmdString = new SparqlParameterizedString();

            cmdString.Namespaces = selectedGraph.NamespaceMap;
            foreach (string prefix in defaultGraph.NamespaceMap.Prefixes)
            {
                cmdString.Namespaces.AddNamespace(prefix, defaultGraph.NamespaceMap.GetNamespaceUri(prefix));
            }
            cmdString.QueryProcessor = GetQueryProcessor();
            if (string.IsNullOrEmpty(className))
            {
                cmdString.CommandText = "PREFIX cims:<http://iec.ch/TC57/1999/rdf-schema-extensions-19990926#> " +
                                        "SELECT DISTINCT ?anySubClass ?property ?type " +
                                        "WHERE { " +
                                            "{ " +                                              //concrete/abstract classes with attributes and types
                                                "?anySubClass rdfs:subClassOf* ?anyClass . " +
                                                "?property rdfs:domain ?anyClass . " +
                                                "?property cims:dataType|rdfs:range ?type " +
                                                "FILTER (!STRSTARTS(STR(?anySubClass), str(rdf:)) && !STRSTARTS(STR(?anySubClass), str(rdfs:))) " +
                                            "} " +
                                            "UNION " +                                  
                                            "{" +                                              //datatype classes with attributes and type
                                                "?anySubClass rdf:type rdfs:Class . " +
                                                "?anySubClass ?property ?type " +
                                                "FILTER NOT EXISTS{ ?anySubClass cims:belongsToCategory []} " +
                                                "FILTER (!STRSTARTS(STR(?anySubClass), str(rdf:)) && !STRSTARTS(STR(?anySubClass), str(rdfs:)) && !STRSTARTS(STR(?anySubClass),str(cims:ClassCategory))" +
                                                " && ?anySubClass != ?type && ?type != rdfs:Resource && ?property != rdfs:comment && ?property != rdfs:label) " +
                                            "} " +
                                            "UNION " +
                                            "{ " +                                            //enumerations with attributes and type
                                                "?anySubClass rdf:type rdfs:Class . " +
                                                "?property rdf:type ?anySubClass . " +
                                                "?property rdfs:label ?label . " +
                                                "?property rdf:type ?type " +
                                                "FILTER (!STRSTARTS(STR(?anySubClass), str(rdf:)) && !STRSTARTS(STR(?anySubClass), str(rdfs:)) && !STRSTARTS(STR(?anySubClass), str(cims:ClassCategory)) && ?type != rdfs:Resource) " +
                                            "} " +
                                         " } " +
                                         "ORDER BY ?anySubClass";
            }
            else
            {
                cmdString.CommandText = "PREFIX cims: <http://iec.ch/TC57/1999/rdf-schema-extensions-19990926#> " +
                                        "SELECT DISTINCT ?choosedClass ?property ?type " +
                                        "WHERE { " +
                                                "{ " +
                                                    "@className rdfs:subClassOf* ?anyClass . " +
                                                    "?property rdfs:domain ?anyClass . " +
                                                    "?property cims:dataType|rdfs:range ?type . " +
                                                    "BIND(@className as ?choosedClass) " +
                                                "} " +
                                                "UNION " +
                                                "{ " +
                                                    "@className rdf:type rdfs:Class . " +
                                                    "@className ?property ?type " +
                                                    "FILTER NOT EXISTS{ @className cims:belongsToCategory [] } " +
                                                    "FILTER(@className != ?type && ?type != rdfs:Resource && ?property != rdfs:comment && ?property != rdfs:label) " +
                                                    "BIND(@className as ?choosedClass) " +
                                                "} " +
                                                "UNION " +
                                                "{ " +
                                                    "@className rdf:type rdfs:Class . " +
                                                    "?property rdf:type @className . " +
                                                    "?property rdfs:label ?label . " +
                                                    "?property rdf:type ?type " +
                                                    "FILTER(?type != rdfs:Resource) " +
                                                    "BIND(@className as ?choosedClass) " +
                                                "} " +
                                          "} " +
                                          "ORDER BY ?property";

                cmdString.SetUri("className", new Uri(graphURIs[graphName] + className));
            }
            SparqlQuery query = sparqlParser.ParseFromString(cmdString);
            Object results = cmdString.QueryProcessor.ProcessQuery(query);

            if (results is SparqlResultSet)
            {
                SparqlResultSet rset = (SparqlResultSet)results;
                return rset;
            }
            else
            {
                return null;
            }
        }

        public SparqlResultSet GetNumOfClasses(string selectedDataset, string graphName)
        {
            ConnectToDatasetEndpoint(selectedDataset);

            Graph selectedGraph = LoadGraph(selectedDataset, graphName);
            Graph defaultGraph = LoadGraph(selectedDataset, "default");
            SparqlQueryParser sparqlParser = new SparqlQueryParser();
            SparqlParameterizedString cmdString = new SparqlParameterizedString();

            cmdString.Namespaces = selectedGraph.NamespaceMap;
            foreach(string prefix in defaultGraph.NamespaceMap.Prefixes)
            {
                cmdString.Namespaces.AddNamespace(prefix, defaultGraph.NamespaceMap.GetNamespaceUri(prefix));
            }
            cmdString.QueryProcessor = GetQueryProcessor();
            cmdString.CommandText = "PREFIX cims: <http://iec.ch/TC57/1999/rdf-schema-extensions-19990926#> " +
                                    "SELECT DISTINCT (STR(COUNT(?class)) as ?numOfClasses)" +
                                    "WHERE { " +
                                        "?class rdf:type rdfs:Class . " +
                                        "FILTER(!STRSTARTS(STR(?class), str(rdf:)) && !STRSTARTS(STR(?class), str(rdfs:)) && !STRSTARTS(STR(?class), str(cims:)))" +
                                        "}";


            SparqlQuery query = sparqlParser.ParseFromString(cmdString);
            Object results = cmdString.QueryProcessor.ProcessQuery(query);

            if (results is SparqlResultSet)
            {
                SparqlResultSet result = (SparqlResultSet)results;
                return result;
            }
            else
            {
                return null;
            }

        }

        public SparqlResultSet GetTypeOfEquipment(string selectedDataset, string graphName, string typeOfEquipment)
        {
            ConnectToDatasetEndpoint(selectedDataset);

            Graph selectedGraph = LoadGraph(selectedDataset, graphName);
            SparqlQueryParser sparqlParser = new SparqlQueryParser();
            SparqlParameterizedString cmdString = new SparqlParameterizedString();

            cmdString.Namespaces = selectedGraph.NamespaceMap;
            cmdString.QueryProcessor = GetQueryProcessor();
            cmdString.CommandText = "SELECT DISTINCT ?class ?property ?value " +
                                    "WHERE { " +
                                        "?class rdf:type @value . " +
                                        "?class ?property ?value " +
                                        "FILTER (?value != rdfs:Resource) " +
                                        "} ";

            if (typeOfEquipment == "Switch")
            {
                cmdString.SetUri("value", new Uri(graphURIs[graphName] + "Switch"));
            }
            else if(typeOfEquipment == "Conducting Equipment")
            {
                cmdString.SetUri("value", new Uri(graphURIs[graphName] + "ConductingEquipment"));
            }

            SparqlQuery query = sparqlParser.ParseFromString(cmdString);
            Object results = cmdString.QueryProcessor.ProcessQuery(query);

            if (results is SparqlResultSet)
            {
                SparqlResultSet result = (SparqlResultSet)results;
                return result;
            }
            else
            {
                return null;
            }
        }

        public SparqlResultSet GetConnectivityNodeComponents(string selectedDataset, string graphName, string nodeID)
        {
            ConnectToDatasetEndpoint(selectedDataset);

            Graph selectedGraph = LoadGraph(selectedDataset, graphName);
            SparqlQueryParser sparqlParser = new SparqlQueryParser();
            SparqlParameterizedString cmdString = new SparqlParameterizedString();

            cmdString.Namespaces = selectedGraph.NamespaceMap;
            cmdString.QueryProcessor = GetQueryProcessor();
            cmdString.CommandText = "SELECT DISTINCT ?class ?property ?value " +
                                    "WHERE { " +
                                        "?class ?reference @NodeID . " +
                                        "?class ?property ?value " +
                                        "FILTER (?value != rdfs:Resource) " +
                                        "} ";

            
            cmdString.SetUri("NodeID", new Uri(graphURIs[graphName] + nodeID));

            SparqlQuery query = sparqlParser.ParseFromString(cmdString);
            Object results = cmdString.QueryProcessor.ProcessQuery(query);

            if (results is SparqlResultSet)
            {
                SparqlResultSet result = (SparqlResultSet)results;
                return result;
            }
            else
            {
                return null;
            }
        }

        public SparqlResultSet GetComponentInfo(string selectedDataset, string graphName, string componentID)
        {
            ConnectToDatasetEndpoint(selectedDataset);

            Graph selectedGraph = LoadGraph(selectedDataset, graphName);

            SparqlQueryParser sparqlParser = new SparqlQueryParser();
            SparqlParameterizedString cmdString = new SparqlParameterizedString();


            cmdString.Namespaces = selectedGraph.NamespaceMap;
            cmdString.QueryProcessor = GetQueryProcessor();
            cmdString.CommandText = "SELECT DISTINCT ?class ?property ?value " +
                                    "WHERE { " +
                                         $"?class <{graphURIs[graphName]}IdentifiedObject.mRID> @value . " +
                                         "?class ?property ?value " +
                                         "FILTER (!STRSTARTS(STR(?class), str(rdf:)) && !STRSTARTS(STR(?class), str(rdfs:)) && (?value != rdfs:Resource)) " +
                                         "}" +
                                    "ORDER BY ?class";

            cmdString.SetLiteral("value", componentID);

            SparqlQuery query = sparqlParser.ParseFromString(cmdString);
            Object results = cmdString.QueryProcessor.ProcessQuery(query);

            if (results is SparqlResultSet)
            {
                SparqlResultSet result = (SparqlResultSet)results;
                
                return result;
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}
