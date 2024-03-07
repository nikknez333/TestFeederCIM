using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;

namespace CIMProfileLoader.Formatter
{
    public class PropertyInfo
    {
        public string Name { get; set; }
        public INode Value { get; set; }
    }
}
