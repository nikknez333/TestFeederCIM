using StorageBenchmark.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBenchmark.Access
{
    public class PostgreSQLDBContext : DbContext
    {
        public PostgreSQLDBContext() : base("PostgreSQLDBContext")
        {

        }

        public DbSet<RDFGraph> RDFGraphs { get; set; }
        public DbSet<RDFTriple> RDFTriples { get; set; }
    }
}
