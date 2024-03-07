using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using CIMProfileLoader.Core;
using StorageBenchmark.Access;
using StorageBenchmark.Model;
using StorageBenchmark.Orderer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;

namespace StorageBenchmark
{
    [MemoryDiagnoser]
    [HideColumns("RDFTripleStartId", "RDFTripleEndId","InvocationCount", "UnrollFactor", "Job", "graphId", "baseUri", "graphName", "subjectName")]
    public class mySQLBenchmark
    {
        private Dictionary<string, IGraph> testGraphs;
        private RDFFileHandler handler;

        private List<RDFTriple> updatedTriples;

        private mySqlDBContext dbContext;

        private string basePath = string.Empty;

        [GlobalSetup]
        public void Setup()
        {
            dbContext = new mySqlDBContext();

            handler = new RDFFileHandler();

            basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            testGraphs = new Dictionary<string, IGraph>
            {
                {"small", handler.LoadRDF(Path.Combine(basePath, "TestFiles", "modelLabs_PowerTransformer_Example.xml")) },
                {"medium", handler.LoadRDF(Path.Combine(basePath, "TestFiles", "CGMES_v2.4.15_RealGridTestConfiguration_SSH_V2.xml")) },
                {"large", handler.LoadRDF(Path.Combine(basePath, "TestFiles", "CGMES_v2.4.15_RealGridTestConfiguration_EQ_V2.xml")) }
            };

            updatedTriples = new List<RDFTriple> { new RDFTriple { RDFTripleId = 1, Subject = "UpdatedSubject1", Predicate = "UpdatedPredicate1", Object = "UpdatedObject1" },
                                                   new RDFTriple { RDFTripleId = 2, Subject = "UpdatedSubject2", Predicate = "UpdatedPredicate2", Object = "UpdatedObject2" }
            };
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
        
        [IterationSetup(Target = nameof(AddRDFToDB))]
        public void IterationSetup()
        {

            if (dbContext.RDFTriples.Any())
            {
                var entitiesTriples = dbContext.RDFTriples.ToList();

                dbContext.RDFTriples.RemoveRange(entitiesTriples);
                dbContext.SaveChanges();

                dbContext.Database.ExecuteSqlCommand("ALTER TABLE rdftriples AUTO_INCREMENT = 1; ");
            }
            if (dbContext.RDFGraphs.Any())
            {
                var entities = dbContext.RDFGraphs.ToList();
                
                dbContext.RDFGraphs.RemoveRange(entities);
                dbContext.SaveChanges();

                dbContext.Database.ExecuteSqlCommand("ALTER TABLE rdfgraphs AUTO_INCREMENT = 1; ");
            }
        }
        
        [Benchmark]
        [Arguments("http://iec.ch/TC57/2013/CIM-schema-cim15#", "modelLabs_PowerTransformer_Example.xml")]
        //[Arguments("http://iec.ch/TC57/2013/CIM-schema-cim16#", "CGMES_v2.4.15_RealGridTestConfiguration_SSH_V2")]
        public void AddRDFToDB(string baseUri, string graphName)
        { 
            IGraph rdfGraph = GetGraph(graphName);

            rdfGraph.BaseUri = new Uri(baseUri + graphName);

            var graphDB = new RDFGraph { GraphName = rdfGraph.BaseUri?.ToString() ?? "DefaultGraph" };

            dbContext.RDFGraphs.Add(graphDB);
            dbContext.SaveChanges();

            var triples = rdfGraph.Triples.Select(t => new RDFTriple
            {
                RDFGraphId = graphDB.RDFGraphId,
                Subject = t.Subject.ToString(),
                Predicate = t.Predicate.ToString(),
                Object = t.Object.ToString(),
                GraphName = t.GraphUri?.ToString() ?? graphDB.GraphName
            }).ToList();

            graphDB.Triples = triples;

            dbContext.RDFTriples.AddRange(triples);
            dbContext.SaveChanges();
        
        }
        
        [Benchmark]
        [Arguments(1)]
        public (string graphName, List<RDFTriple> triples) ReadTriplesForGraph(int graphId)
        {
            var result = dbContext.RDFGraphs
                .Where(g => g.RDFGraphId == graphId)
                .Select(g => new
                {
                    g.GraphName,
                    Triples = g.Triples
                })
                .FirstOrDefault();

            if (result != null)
            {
                return (result.GraphName, result.Triples.ToList());
            }

            return (null, null);
        }
        
        [Benchmark]
        [Arguments(1)]
        public void UpdateRDFInDB(int graphId)
        {
            var graphDB = dbContext.RDFGraphs
                        .Where(g => g.RDFGraphId == graphId)
                        .Select(g => new
                        {
                            g.GraphName,
                            Triples = g.Triples
                        })
                        .FirstOrDefault();

            if(graphDB != null)
            {
                foreach(var updatedTriple in updatedTriples)
                {
                    var existingTriple = graphDB.Triples.FirstOrDefault(t => t.RDFTripleId == updatedTriple.RDFTripleId);

                    if(existingTriple != null)
                    {
                        existingTriple.Subject = updatedTriple.Subject;
                        existingTriple.Predicate = updatedTriple.Predicate;
                        existingTriple.Object = updatedTriple.Object;
                    }
                }

                dbContext.SaveChanges();
            }
        }
        
        [Benchmark]
        [Arguments(1)]
        public void DeleteTriplesFromGraph(int graphId)
        {
            var triplesToRemove = dbContext.RDFTriples
                                 .Where(t => t.RDFGraphId == graphId)
                                 .ToList();

            dbContext.RDFTriples.RemoveRange(triplesToRemove);

            dbContext.SaveChanges();
        }
        
        [Benchmark]
        [Arguments(2)]
        public void DeleteGraphAndTriples(int graphId)
        {
            var graphDB = dbContext.RDFGraphs
                          .FirstOrDefault(g => g.RDFGraphId == graphId);

            if (graphDB != null)
            {
                var triplesToRemove = dbContext.RDFTriples
                                    .Where(t => t.RDFGraphId == graphId)
                                    .ToList();

                dbContext.RDFTriples.RemoveRange(triplesToRemove);

                dbContext.RDFGraphs.Remove(graphDB);

                dbContext.SaveChanges();
            }
        }
        
        [Benchmark]
        [Arguments(1,50)]
        public List<RDFTriple> SQLQueryMoreResults(int RDFTripleStartId, int RDFTripleEndId)
        {
            var resultTriples = dbContext.RDFTriples
                                .Where(t => t.RDFGraphId >= RDFTripleStartId && t.RDFGraphId <= RDFTripleEndId)
                                .ToList();

            return resultTriples;
        }

        [Benchmark]
        [Arguments("http://iec.ch/TC57/2013/CIM-schema-cim15#modelLabs_PowerTransformer_Example.xml", "http://iec.ch/TC57/2010/CIM-schema-cim15#_124903727_LOC_CS")]
        //[Arguments("http://iec.ch/TC57/2013/CIM-schema-cim16#CGMES_v2.4.15_RealGridTestConfiguration_SSH_V2", "http://iec.ch/TC57/2013/CIM-schema-cim16#_1425808154_ACLS_T1")]
        public List<RDFTriple> SQLQueryFewResults(string graphName, string subjectName)
        {
            var SQLQuery = dbContext.RDFTriples
                           .Join
                           (
                            dbContext.RDFGraphs,
                            triple => triple.RDFTripleId,
                            graph => graph.RDFGraphId,
                            (triple, graph) => new { Triple = triple, Graph = graph }
                )
                .Where(joined => joined.Graph.GraphName == graphName && joined.Triple.Subject == subjectName)
                .Select(joined => joined.Triple);

            var resultTriple = SQLQuery.ToList();
            return resultTriple;
        }
        
    }
}
