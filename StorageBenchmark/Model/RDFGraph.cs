using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBenchmark.Model
{
    public class RDFGraph
    {
        public int RDFGraphId { get; set; }
        public string GraphName { get; set; }
        public List<RDFTriple> Triples { get; set; }
    }
}
