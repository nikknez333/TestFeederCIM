using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBenchmark.Model
{
    public class RDFTriple
    {
        public int RDFTripleId { get; set; }
        public int RDFGraphId { get; set; }
        public string Subject { get; set; }
        public string Predicate { get; set; }
        public string Object { get; set; }
        public string GraphName { get; set; }
    }
}
