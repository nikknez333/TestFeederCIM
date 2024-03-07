using BenchmarkDotNet.Running;
using MySql.Data.EntityFramework;
using StorageBenchmark.Access;
using StorageBenchmark.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;

namespace StorageBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            //Zakomentarisati preostale dve BenchmarkRunner.Run<T>(); metode kako bi se smanjilo vreme potrebno za testiranje ili dodati atribut [ShortRunJob] iznad TripleStoreBenchmark,mySQLBenchmark
            //i PostgreSQLBenchmark klasa
            //ShortRunJob, iako je brzi nacin testiranja moze dati nesto nepreciznije rezultate

            DbConfiguration.SetConfiguration(new MySqlEFConfiguration());
            StartFusekiStore();
            BenchmarkRunner.Run<TripleStoreBenchmark>();
            //BenchmarkRunner.Run<mySQLBenchmark>();
            //BenchmarkRunner.Run<PostgreSQLBenchmark>();
        }

        public static void StartFusekiStore()
        {
            Process jenaStorage = new Process();

            string directory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string fusekiServerBATDirectory = Path.Combine(directory, "apache-jena-fuseki-4.8.0", "apache-jena-fuseki-4.8.0");

            jenaStorage.StartInfo.WorkingDirectory = fusekiServerBATDirectory;
            jenaStorage.StartInfo.FileName = "cmd.exe";
            jenaStorage.StartInfo.UseShellExecute = false;

            string command = "fuseki-server";
            jenaStorage.StartInfo.Arguments = "/k " + command;

            jenaStorage.Start();

            File.WriteAllText("jenaProcessId.txt", jenaStorage.Id.ToString());
        }
    }
}
