using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using CIMProfileLoader;
using CIMProfileLoader.Core;
using CIMProfileLoader.Storage;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using VDS.RDF.Storage;

namespace StorageBenchmark
{
    //otkomentarisati SPARQL upite za rdf graf srednje ili velike velicine, kao i arguments atribute prilikom njihovog testiranja a zakomentarisati preostala 2 slucaja
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.Method, BenchmarkDotNet.Order.MethodOrderPolicy.Declared)]
    public class TripleStoreBenchmark
    {
        private Dictionary<string, IGraph> testGraphs;
        private RDFFileHandler handler = new RDFFileHandler();

        private FusekiConnector inMemoryConnector = null;
        private FusekiConnector tdb2Connector = null;

        private const string baseServiceUri = "http://localhost:3030/";

        private Graph loadGraphTest = new Graph();
        private IGraph updateGraphTest = new Graph();

        private SparqlQueryParser sparqlQueryParser;
        private SparqlParameterizedString cmdString;
        private SparqlParameterizedString cmdCheck;
        private GenericQueryProcessor processorInMemory;
        private GenericQueryProcessor processorTDB2;

        private string basePath = string.Empty;

        [GlobalSetup]
        public void Setup()
        {
            inMemoryConnector = new FusekiConnector(baseServiceUri + "FTNServiceDataset" + "/data");
            inMemoryConnector.Timeout = 100000;
            tdb2Connector = new FusekiConnector(baseServiceUri + "TDB2ServiceDataset" + "/data");
            tdb2Connector.Timeout = 100000;

            basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            testGraphs = new Dictionary<string, IGraph>
            {
                {"small", handler.LoadRDF(Path.Combine(basePath, "TestFiles", "modelLabs_PowerTransformer_Example.xml")) },
                {"medium", handler.LoadRDF(Path.Combine(basePath, "TestFiles", "CGMES_v2.4.15_RealGridTestConfiguration_SSH_V2.xml")) },
                {"large", handler.LoadRDF(Path.Combine(basePath, "TestFiles", "CGMES_v2.4.15_RealGridTestConfiguration_EQ_V2.xml")) }
            };

            updateGraphTest = handler.LoadRDF("D:\\Nebojsa_Knezevic_Projekat_PR_104_2015\\TestFiles\\20171002T0930Z_1D_NL_TP_2.xml");
            sparqlQueryParser = new SparqlQueryParser();
            cmdString = new SparqlParameterizedString();
            cmdCheck = new SparqlParameterizedString();

            processorInMemory = new GenericQueryProcessor(inMemoryConnector);
            processorTDB2 = new GenericQueryProcessor(tdb2Connector);

            cmdString.QueryProcessor = processorInMemory;
            inMemoryConnector.UpdateGraph(string.Empty, testGraphs["small"].Triples, null);
        }

        private IGraph GetGraph(string graphName)
        {
            // Depending on the graph name, return the appropriate graph
            switch (graphName)
            {
                case "modelLabs_PowerTransformer_Example.xml":
                    return testGraphs["small"];
                case "CGMES_v2.4.15_RealGridTestConfiguration_SSH_V2":
                    return testGraphs["medium"];
                case "CGMES_v2.4.15_RealGridTestConfiguration_EQ_V2":
                    return testGraphs["large"];
                default:
                    throw new ArgumentException($"Invalid graph name: {graphName}");
            }
        }

        [Benchmark]
        [Arguments("http://iec.ch/TC57/2013/CIM-schema-cim15#", "modelLabs_PowerTransformer_Example.xml")]
        //[Arguments("http://iec.ch/TC57/2013/CIM-schema-cim16#","CGMES_v2.4.15_RealGridTestConfiguration_SSH_V2")]
        //[Arguments("http://iec.ch/TC57/2013/CIM-schema-cim16#", "CGMES_v2.4.15_RealGridTestConfiguration_EQ_V2")]
        public void SaveGraph_InMemory(string baseUri, string graphName)
        {
            IGraph graph = GetGraph(graphName);

            graph.BaseUri = new Uri(baseUri + graphName);

            inMemoryConnector.SaveGraph(graph);
        }
        
        [Benchmark]
        [Arguments("http://iec.ch/TC57/2013/CIM-schema-cim15#", "modelLabs_PowerTransformer_Example.xml")]
        //[Arguments("http://iec.ch/TC57/2013/CIM-schema-cim16#","CGMES_v2.4.15_RealGridTestConfiguration_SSH_V2")]
        //[Arguments("http://iec.ch/TC57/2013/CIM-schema-cim16#", "CGMES_v2.4.15_RealGridTestConfiguration_EQ_V2")]
        public void SaveGraph_TDB2(string baseUri, string graphName)
        {
            IGraph graph = GetGraph(graphName);
            graph.BaseUri = new Uri(baseUri + graphName);
            tdb2Connector.SaveGraph(graph);
        }

        [Benchmark]
        [Arguments("http://iec.ch/TC57/2013/CIM-schema-cim15#", "modelLabs_PowerTransformer_Example.xml")]
        //[Arguments("http://iec.ch/TC57/2013/CIM-schema-cim16#", "CGMES_v2.4.15_RealGridTestConfiguration_SSH_V2")]
        //[Arguments("http://iec.ch/TC57/2013/CIM-schema-cim16#", "CGMES_v2.4.15_RealGridTestConfiguration_EQ_V2")]
        public void LoadGraph_InMemory(string baseUri, string graphName)
        {
            if (graphName == "default" && inMemoryConnector.IsReady)
            {
                inMemoryConnector.LoadGraph(loadGraphTest, string.Empty);
            }
            else if (inMemoryConnector.IsReady)
            {
                inMemoryConnector.LoadGraph(loadGraphTest, baseUri + graphName);
            }
        }

        [Benchmark]
        [Arguments("http://iec.ch/TC57/2013/CIM-schema-cim15#", "modelLabs_PowerTransformer_Example.xml")]
        //[Arguments("http://iec.ch/TC57/2013/CIM-schema-cim16#", "CGMES_v2.4.15_RealGridTestConfiguration_SSH_V2")]
        //[Arguments("http://iec.ch/TC57/2013/CIM-schema-cim16#", "CGMES_v2.4.15_RealGridTestConfiguration_EQ_V2")]
        public void LoadGraph_TDB2(string baseUri, string graphName)
        {
            if (graphName == "default" && tdb2Connector.IsReady)
            {
                inMemoryConnector.LoadGraph(loadGraphTest, string.Empty);
            }
            else if (tdb2Connector.IsReady)
            {
                inMemoryConnector.LoadGraph(loadGraphTest, baseUri + graphName);
            }
        }

        [Benchmark]
        [Arguments("http://iec.ch/TC57/2013/CIM-schema-cim15#", "modelLabs_PowerTransformer_Example.xml")]
        //[Arguments("http://iec.ch/TC57/2013/CIM-schema-cim16#", "CGMES_v2.4.15_RealGridTestConfiguration_SSH_V2")]
        //[Arguments("http://iec.ch/TC57/2013/CIM-schema-cim16#", "CGMES_v2.4.15_RealGridTestConfiguration_EQ_V2")]
        public void UpdateGraphAddTriples_InMemory(string baseUri, string graphName)
        {
            inMemoryConnector.UpdateGraph(baseUri + graphName, updateGraphTest.Triples, null);
        }

        [Benchmark]
        [Arguments("http://iec.ch/TC57/2013/CIM-schema-cim15#", "modelLabs_PowerTransformer_Example.xml")]
        //[Arguments("http://iec.ch/TC57/2013/CIM-schema-cim16#", "CGMES_v2.4.15_RealGridTestConfiguration_SSH_V2")]
        //[Arguments("http://iec.ch/TC57/2013/CIM-schema-cim16#", "CGMES_v2.4.15_RealGridTestConfiguration_EQ_V2")]
        public void UpdateGraphAddTriples_TDB2(string baseUri, string graphName)
        {
            tdb2Connector.UpdateGraph(baseUri + graphName, updateGraphTest.Triples, null);
        }

        [Benchmark]
        [Arguments("http://iec.ch/TC57/2013/CIM-schema-cim15#", "modelLabs_PowerTransformer_Example.xml")]
        //[Arguments("http://iec.ch/TC57/2013/CIM-schema-cim16#", "CGMES_v2.4.15_RealGridTestConfiguration_SSH_V2")]
        //[Arguments("http://iec.ch/TC57/2013/CIM-schema-cim16#", "CGMES_v2.4.15_RealGridTestConfiguration_EQ_V2")]
        public void UpdateGraphDeleteTriples_InMemory(string baseUri, string graphName)
        {
            inMemoryConnector.UpdateGraph(baseUri + graphName, null, updateGraphTest.Triples);
        }

        [Benchmark]
        [Arguments("http://iec.ch/TC57/2013/CIM-schema-cim15#", "modelLabs_PowerTransformer_Example.xml")]
        //[Arguments("http://iec.ch/TC57/2013/CIM-schema-cim16#", "CGMES_v2.4.15_RealGridTestConfiguration_SSH_V2")]
        //[Arguments("http://iec.ch/TC57/2013/CIM-schema-cim16#", "CGMES_v2.4.15_RealGridTestConfiguration_EQ_V2")]
        public void UpdateGraphDeleteTriples_TDB2(string baseUri, string graphName)
        {
            tdb2Connector.UpdateGraph(baseUri + graphName, null, updateGraphTest.Triples);
        }
        
        [Benchmark]
        [Arguments("http://iec.ch/TC57/2013/CIM-schema-cim15#", "modelLabs_PowerTransformer_Example.xml")]
        public SparqlResultSet SparqlSimpleQuerySmallRDF_InMemory(string baseUri, string graphName)
        {
            cmdString.CommandText = "PREFIX cim: < http://iec.ch/TC57/2010/CIM-schema-cim15#> " +
                                    "SELECT ?baseVoltage ?nominalVoltage " +
                                    "FROM  <" + baseUri + graphName + "> " +
                                    "WHERE { " +
                                            "?baseVoltage a cim:BaseVoltage . " +
                                            "?baseVoltage cim:BaseVoltage.nominalVoltage ?nominalVoltage . " +
                                     "}";

            cmdString.QueryProcessor = processorInMemory;
            SparqlQuery query = sparqlQueryParser.ParseFromString(cmdString);
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

        [Benchmark]
        [Arguments("http://iec.ch/TC57/2013/CIM-schema-cim15#", "modelLabs_PowerTransformer_Example.xml")]
        public SparqlResultSet SparqlSimpleQuerySmallRDF_TDB2(string baseUri, string graphName)
        {
            cmdString.CommandText = "PREFIX cim: < http://iec.ch/TC57/2010/CIM-schema-cim15#> " +
                                    "SELECT ?baseVoltage ?nominalVoltage " +
                                    "FROM  <" + baseUri + graphName + "> " +
                                    "WHERE { " +
                                            "?baseVoltage a cim:BaseVoltage . " +
                                            "?baseVoltage cim:BaseVoltage.nominalVoltage ?nominalVoltage . " +
                                     "}";

            cmdString.QueryProcessor = processorTDB2;
            SparqlQuery query = sparqlQueryParser.ParseFromString(cmdString);
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
        /*
        [Benchmark]
        [Arguments("http://iec.ch/TC57/2013/CIM-schema-cim16#", "CGMES_v2.4.15_RealGridTestConfiguration_SSH_V2")]
        public SparqlResultSet SparqlSimpleQueryMediumRDF_InMemory(string baseUri, string graphName)
        {
            cmdString.CommandText = "PREFIX cim: < http://iec.ch/TC57/2013/CIM-schema-cim16#> " +
                                    "SELECT ?energyConsumer ?p " +
                                    "FROM  <" + baseUri + graphName + "> " +
                                    "WHERE { " +
                                            "?energyConsumer a cim:EnergyConsumer . " +
                                            "?energyConsumer cim:EnergyConsumer.p ?p . " +
                                     "}";

            cmdString.QueryProcessor = processorInMemory;
            SparqlQuery query = sparqlQueryParser.ParseFromString(cmdString);
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

        [Benchmark]
        [Arguments("http://iec.ch/TC57/2013/CIM-schema-cim16#", "CGMES_v2.4.15_RealGridTestConfiguration_SSH_V2")]
        public SparqlResultSet SparqlSimpleQueryMediumRDF_TDB2(string baseUri, string graphName)
        {
            cmdString.CommandText = "PREFIX cim: < http://iec.ch/TC57/2013/CIM-schema-cim16#> " +
                                    "SELECT ?energyConsumer ?p " +
                                    "FROM  <" + baseUri + graphName + "> " +
                                    "WHERE { " +
                                            "?energyConsumer a cim:EnergyConsumer . " +
                                            "?energyConsumer cim:EnergyConsumer.p ?p . " +
                                     "}";

            cmdString.QueryProcessor = processorTDB2;
            SparqlQuery query = sparqlQueryParser.ParseFromString(cmdString);
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
        
        [Benchmark]
        [Arguments("http://iec.ch/TC57/2013/CIM-schema-cim16#", "CGMES_v2.4.15_RealGridTestConfiguration_SSH_V2")]
        public SparqlResultSet SparqlComplexQueryMediumRDF_InMemory(string baseUri, string graphName)
        {
            cmdString.CommandText = "PREFIX cim: < http://iec.ch/TC57/2013/CIM-schema-cim16#> " +
                                    "PREFIX rdf: < http://www.w3.org/1999/02/22-rdf-syntax-ns#> " +
                                    "SELECT ?machine ?p ?q ?mode ?priority " +
                                    "FROM  <" + baseUri + graphName + "> " +
                                    "WHERE { " +
                                            "?machine rdf:type cim:SynchronousMachine . " +
                                            "?machine cim:RotatingMachine.p ?p . " +
                                            "?machine cim:RotatingMachine.q ?q . " +
                                            "?machine cim:SynchronousMachine.operatingMode cim:SynchronousMachineOperatingMode.generator . " +
                                            "?machine cim:SynchronousMachine.referencePriority ?priority . " +
                                     "}";

            cmdString.QueryProcessor = processorInMemory;
            SparqlQuery query = sparqlQueryParser.ParseFromString(cmdString);
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

        [Benchmark]
        [Arguments("http://iec.ch/TC57/2013/CIM-schema-cim16#", "CGMES_v2.4.15_RealGridTestConfiguration_SSH_V2")]
        public SparqlResultSet SparqlComplexQueryMediumRDF_TDB2(string baseUri, string graphName)
        {
            cmdString.CommandText = "PREFIX cim: < http://iec.ch/TC57/2013/CIM-schema-cim16#> " +
                                   "PREFIX rdf: < http://www.w3.org/1999/02/22-rdf-syntax-ns#> " +
                                   "SELECT ?machine ?p ?q ?mode ?priority " +
                                   "FROM  <" + baseUri + graphName + "> " +
                                   "WHERE { " +
                                           "?machine rdf:type cim:SynchronousMachine . " +
                                           "?machine cim:RotatingMachine.p ?p . " +
                                           "?machine cim:RotatingMachine.q ?q . " +
                                           "?machine cim:SynchronousMachine.operatingMode cim:SynchronousMachineOperatingMode.generator . " +
                                           "?machine cim:SynchronousMachine.referencePriority ?priority . " +
                                    "}";

            cmdString.QueryProcessor = processorTDB2;
            SparqlQuery query = sparqlQueryParser.ParseFromString(cmdString);
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
        */

        [Benchmark]
        [Arguments("http://iec.ch/TC57/2013/CIM-schema-cim15#", "modelLabs_PowerTransformer_Example.xml")]
        public SparqlResultSet SparqlComplexQuerySmallRDF_InMemory(string baseUri, string graphName)
        {
            cmdString.CommandText = "PREFIX cims:<http://iec.ch/TC57/1999/rdf-schema-extensions-19990926#> " +
                                    "PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#> " +
                                    "PREFIX rdf: < http://www.w3.org/1999/02/22-rdf-syntax-ns#> " +
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

            cmdString.QueryProcessor = processorInMemory;
            SparqlQuery query = sparqlQueryParser.ParseFromString(cmdString);
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

        [Benchmark]
        [Arguments("http://iec.ch/TC57/2013/CIM-schema-cim15#", "modelLabs_PowerTransformer_Example.xml")]
        public SparqlResultSet SparqlComplexQuerySmallRDF_TDB2(string baseUri, string graphName)
        {
            cmdString.CommandText = "PREFIX cims:<http://iec.ch/TC57/1999/rdf-schema-extensions-19990926#> " +
                                    "PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#> " +
                                    "PREFIX rdf: < http://www.w3.org/1999/02/22-rdf-syntax-ns#> " +
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

            cmdString.QueryProcessor = processorTDB2;
            SparqlQuery query = sparqlQueryParser.ParseFromString(cmdString);
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
        /*
        [Benchmark]
        [Arguments("http://iec.ch/TC57/2013/CIM-schema-cim16#", "CGMES_v2.4.15_RealGridTestConfiguration_EQ_V2")]
        public SparqlResultSet SparqlSimpleQuerylargeRDF_InMemory(string baseUri, string graphName)
        {
            cmdString.CommandText = "PREFIX cim: < http://iec.ch/TC57/2013/CIM-schema-cim16#> " +
                                    "PREFIX rdf: < http://www.w3.org/1999/02/22-rdf-syntax-ns#> " +
                                    "SELECT ?lineName ?regionName " +
                                    "FROM  <" + baseUri + graphName + "> " +
                                    "WHERE { " +
                                            "?line rdf:type cim:Line . " +
                                            "?line cim:IdentifiedObject.name ?lineName . " +
                                            "?line cim:Line.Region ?region . " +
                                            "?region cim:IdentifiedObject.name ?regionName . " +
                                     "}";

            cmdString.QueryProcessor = processorInMemory;
            SparqlQuery query = sparqlQueryParser.ParseFromString(cmdString);
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

        [Benchmark]
        [Arguments("http://iec.ch/TC57/2013/CIM-schema-cim16#", "CGMES_v2.4.15_RealGridTestConfiguration_EQ_V2")]
        public SparqlResultSet SparqlSimpleQuerylargeRDF_TDB2(string baseUri, string graphName)
        {
            cmdString.CommandText = "PREFIX cim: < http://iec.ch/TC57/2013/CIM-schema-cim16#> " +
                                    "PREFIX rdf: < http://www.w3.org/1999/02/22-rdf-syntax-ns#> " +
                                    "SELECT ?lineName ?regionName " +
                                    "FROM  <" + baseUri + graphName + "> " +
                                    "WHERE { " +
                                            "?line rdf:type cim:Line . " +
                                            "?line cim:IdentifiedObject.name ?lineName . " +
                                            "?line cim:Line.Region ?region . " +
                                            "?region cim:IdentifiedObject.name ?regionName . " +
                                     "}";

            cmdString.QueryProcessor = processorTDB2;
            SparqlQuery query = sparqlQueryParser.ParseFromString(cmdString);
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

        [Benchmark]
        [Arguments("http://iec.ch/TC57/2013/CIM-schema-cim16#", "CGMES_v2.4.15_RealGridTestConfiguration_EQ_V2")]
        public SparqlResultSet SparqlComplexQuerylargeRDF_InMemory(string baseUri, string graphName)
        {
            cmdString.CommandText = "PREFIX cim: < http://iec.ch/TC57/2013/CIM-schema-cim16#> " +
                                    "PREFIX rdf: < http://www.w3.org/1999/02/22-rdf-syntax-ns#> " +
                                    "SELECT ?transformerName ?ratedPower ?regionName " +
                                    "FROM  <" + baseUri + graphName + "> " +
                                    "WHERE { " +
                                            "?powerTransformer rdf:type cim:PowerTransformer . " +
                                            "?powerTransformer cim:IdentifiedObject.name ?transformerName . " +
                                            "?powerTransformer cim:PowerTransformer.ratedS ?ratedPower . " +
                                            "?powerTransformer cim:Equipment.EquipmentContainer ?container . " +
                                            "?container cim:Line.Region ?region . " +
                                            "?region cim:IdentifiedObject.name ?regionName . " +
                                     "}";

            cmdString.QueryProcessor = processorInMemory;
            SparqlQuery query = sparqlQueryParser.ParseFromString(cmdString);
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

        [Benchmark]
        [Arguments("http://iec.ch/TC57/2013/CIM-schema-cim16#", "CGMES_v2.4.15_RealGridTestConfiguration_EQ_V2")]
        public SparqlResultSet SparqlComplexQuerylargeRDF_TDB2(string baseUri, string graphName)
        {
            cmdString.CommandText = "PREFIX cim: < http://iec.ch/TC57/2013/CIM-schema-cim16#> " +
                                    "PREFIX rdf: < http://www.w3.org/1999/02/22-rdf-syntax-ns#> " +
                                    "SELECT ?transformerName ?ratedPower ?regionName " +
                                    "FROM  <" + baseUri + graphName + "> " +
                                    "WHERE { " +
                                            "?powerTransformer rdf:type cim:PowerTransformer . " +
                                            "?powerTransformer cim:IdentifiedObject.name ?transformerName . " +
                                            "?powerTransformer cim:PowerTransformer.ratedS ?ratedPower . " +
                                            "?powerTransformer cim:Equipment.EquipmentContainer ?container . " +
                                            "?container cim:Line.Region ?region . " +
                                            "?region cim:IdentifiedObject.name ?regionName . " +
                                     "}";

            cmdString.QueryProcessor = processorTDB2;
            SparqlQuery query = sparqlQueryParser.ParseFromString(cmdString);
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
        */
        [Benchmark]
        [Arguments("http://iec.ch/TC57/2013/CIM-schema-cim15#", "modelLabs_PowerTransformer_Example.xml")]
        //[Arguments("http://iec.ch/TC57/2013/CIM-schema-cim16#", "CGMES_v2.4.15_RealGridTestConfiguration_SSH_V2")]
        //[Arguments("http://iec.ch/TC57/2013/CIM-schema-cim16#", "CGMES_v2.4.15_RealGridTestConfiguration_EQ_V2")]
        public void DeleteGraph_InMemory(string baseUri, string graphName)
        {
            inMemoryConnector.DeleteGraph(baseUri + graphName);
        }

        [Benchmark]
        [Arguments("http://iec.ch/TC57/2013/CIM-schema-cim15#", "modelLabs_PowerTransformer_Example.xml")]
        //[Arguments("http://iec.ch/TC57/2013/CIM-schema-cim16#", "CGMES_v2.4.15_RealGridTestConfiguration_SSH_V2")]
        //[Arguments("http://iec.ch/TC57/2013/CIM-schema-cim16#", "CGMES_v2.4.15_RealGridTestConfiguration_EQ_V2")]
        public void DeleteGraph_TDB2(string baseUri, string graphName)
        {
            tdb2Connector.DeleteGraph(baseUri + graphName);
        }
        
        [GlobalCleanup]
        public void Cleanup()
        {
            tdb2Connector.Dispose();
            inMemoryConnector.Dispose();
        }
    }
}
