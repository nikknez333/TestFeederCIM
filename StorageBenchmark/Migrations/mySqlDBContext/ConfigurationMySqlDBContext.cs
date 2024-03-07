namespace StorageBenchmark.Migrations.mySqlDBContext
{
    using CIMProfileLoader.Core;
    using StorageBenchmark.Model;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using VDS.RDF;

    internal sealed class ConfigurationMySqlDBContext : DbMigrationsConfiguration<StorageBenchmark.Access.mySqlDBContext>
    {
        private RDFFileHandler handler = new RDFFileHandler();
        private StorageBenchmark.Access.mySqlDBContext dbContext = new Access.mySqlDBContext();
        public ConfigurationMySqlDBContext()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\mySqlDBContext";
        }

        protected override void Seed(StorageBenchmark.Access.mySqlDBContext context)
        {
            string basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            IGraph rdfGraph = handler.LoadRDF(Path.Combine(basePath, "TestFiles", "modelLabs_PowerTransformer_Example.xml"));
            IGraph rdfGraphDuplicate = rdfGraph;

            rdfGraph.BaseUri = new Uri("http://iec.ch/TC57/2013/CIM-schema-cim15#" + "modelLabs_PowerTransformer_Example.xml");
            rdfGraphDuplicate.BaseUri = new Uri("http://iec.ch/TC57/2013/CIM-schema-cim15#" + "duplicate_modelLabs_PowerTransformer_Example.xml");

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

            var graphDBDuplicate = new RDFGraph { GraphName = rdfGraphDuplicate.BaseUri?.ToString() ?? "DefaultGraph" };

            dbContext.RDFGraphs.Add(graphDBDuplicate);
            dbContext.SaveChanges();

            var triplesDuplicate = rdfGraphDuplicate.Triples.Select(t => new RDFTriple
            {
                RDFGraphId = graphDBDuplicate.RDFGraphId,
                Subject = t.Subject.ToString(),
                Predicate = t.Predicate.ToString(),
                Object = t.Object.ToString(),
                GraphName = t.GraphUri?.ToString() ?? graphDBDuplicate.GraphName
            }).ToList();

            graphDBDuplicate.Triples = triplesDuplicate;

            dbContext.RDFTriples.AddRange(triplesDuplicate);
            dbContext.SaveChanges();
        }
    }
}
