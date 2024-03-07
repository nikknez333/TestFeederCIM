using CIMProfileLoader.Parser;
using CIMProfileLoader.Parser.Handler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Ontology;
using VDS.RDF.Query.Inference;

namespace CIMProfileLoader.Core
{
    public delegate void MessageEventHandler(object sender, string message);

    public class RDFFileHandler
    {

        private IGraph RDFGraph;

        public event MessageEventHandler Message;
        
        public IGraph LoadRDF(string path)
        {
            this.RDFGraph = new Graph();
            RDFXMLParserHandler handler = new RDFXMLParserHandler();

            StringBuilder message = new StringBuilder();
            message.Append("\r\n\t------------------Parsing RDF------------------");
            message.Append("\r\n\nParsing file: \r\n\t").Append(path);
            OnMessage(message.ToString());

            if (!string.IsNullOrEmpty(path))
            {
                bool success;
                TimeSpan durationOfParsing = new TimeSpan(0);

                handler = (RDFXMLParserHandler)RDFXMLParser.DoParse(handler, path, out success, out durationOfParsing);

                StringBuilder msg = new StringBuilder("\r\nRDF file loaded:\r\n\t Duration of RDF file loading: " + durationOfParsing);

                if(success)
                {
                    if (handler.DocumentIdentifiedLikeRDFS)
                        RDFGraph = handler.CimProfileGraph;
                    else
                        RDFGraph = handler.CimDataGraph;

                    msg.Append("\r\n\t loading RDF File was successful");
                }
                else
                {
                    msg.Append("\r\n\t loading RDF File was unsuccessful");
                }
                OnMessage(msg.ToString());
            }
            else
            {
                OnMessage("Parsing impossible - no file or incorrect path");
                return null;
            }
            OnMessage("\r\n\t--------------Done parsing profile--------------");

            if (handler.DocumentIdentifiedLikeRDFS == true)
                OnMessage("\r\n\t Document identified as RDFS CIM Profile");
            else
                OnMessage("\r\n\t Document identified as CIM/XML");

            return RDFGraph;
        }

        public bool ApplyReasonerToData(OntologyGraph schema, Graph data)
        {
            try
            {
                StaticRdfsReasoner rdfsReasoner = new StaticRdfsReasoner();
                rdfsReasoner.Initialise(schema);
                rdfsReasoner.Apply(data);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        #region EVENTS

        protected virtual void OnMessage(string message)
        {
            Message?.Invoke(this, message);    
        }
         

        #endregion
    }
}
