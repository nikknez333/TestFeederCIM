using CIMProfileLoader.Parser.Handler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Parsing;

namespace CIMProfileLoader.Parser
{
    public class RDFXMLParser
    {
        public static IRdfHandler DoParse(IRdfHandler handler, string filename, out bool success, out TimeSpan durationOfParsing)
        {
            success = true;
            durationOfParsing = new TimeSpan(0);

            DateTime startTime = DateTime.Now;
            DateTime stopTime;

            RDFXMLParserHandler handlerRDFXML = (RDFXMLParserHandler)handler;

            try
            {
                RdfXmlParser rdfParser = new RdfXmlParser();
                rdfParser.Load(handlerRDFXML, filename);
            }
            catch(RdfParseException parseEx)
            {
                success = false;
                throw new RdfParseException(parseEx.Message);
            }
            catch(RdfException rdfEx)
            {
                success = false;
                throw new RdfException(rdfEx.Message);
            }

            stopTime = DateTime.Now;

            durationOfParsing = stopTime - startTime;

            return handlerRDFXML;
        }
    }
}
