using MySql.Data.EntityFramework;
using StorageBenchmark.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBenchmark.Access
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class mySqlDBContext : DbContext
    {
        public mySqlDBContext() : base("mySqlDBContext")
        {

        }
        public DbSet<RDFGraph> RDFGraphs { get; set; }
        public DbSet<RDFTriple> RDFTriples { get; set; }
        
    }
}
