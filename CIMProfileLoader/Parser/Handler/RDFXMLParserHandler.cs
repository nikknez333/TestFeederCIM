using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Ontology;
using VDS.RDF.Parsing;
using VDS.RDF.Parsing.Handlers;

namespace CIMProfileLoader.Parser.Handler
{
    public class RDFXMLParserHandler : BaseRdfHandler
    {
        private OntologyGraph cimProfileGraph = null;
        private Graph cimDataGraph = null;
        private bool documentIdentifiedLikeRDFS;
        

        public OntologyGraph CimProfileGraph 
        { 
            get
            {
                return cimProfileGraph;
            }
        }

        public Graph CimDataGraph 
        {
            get
            {
                return cimDataGraph;
            }
        }

        public bool DocumentIdentifiedLikeRDFS 
        { 
            get
            {
                return documentIdentifiedLikeRDFS;
            }
        }

        public override bool AcceptsAll => throw new NotImplementedException();

        protected override void StartRdfInternal()
        {
            try
            {
                if (cimProfileGraph == null)
                    cimProfileGraph = new OntologyGraph();

                if (cimDataGraph == null)
                    cimDataGraph = new Graph();

                documentIdentifiedLikeRDFS = false;
            }
            catch (RdfParseException ex)
            {
                throw new RdfParseException(ex.Message);
            }
        }

        protected override bool HandleNamespaceInternal(string prefix, Uri namespaceUri)
        {
            if (prefix == "rdfs" || prefix == "cims")
                documentIdentifiedLikeRDFS = true;

            cimDataGraph.NamespaceMap.AddNamespace(prefix, namespaceUri);

            if(documentIdentifiedLikeRDFS)
            {
                cimDataGraph.NamespaceMap.Clear();
            }

            return true;
        }

        protected override bool HandleBaseUriInternal(Uri baseUri)
        {
            return base.HandleBaseUriInternal(baseUri);
        }

        protected override void EndRdfInternal(bool ok)
        {
            base.EndRdfInternal(ok);
        }
        protected override bool HandleTripleInternal(Triple t)
        {
            if (documentIdentifiedLikeRDFS == true)
            {
                CimProfileGraph.Assert(t);
            }
            else
            {
                CimDataGraph.Assert(t);
            }

            return true;
        }
    }
}
