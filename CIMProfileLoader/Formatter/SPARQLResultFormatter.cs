using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Query;
using VDS.RDF.Writing.Formatting;

namespace CIMProfileLoader.Formatter
{
    public class SPARQLResultFormatter : IResultFormatter
    {
        private Dictionary<INode, List<PropertyInfo>> groupedProperties = new Dictionary<INode, List<PropertyInfo>>();
        private StringBuilder output = new StringBuilder();
        private List<string> tripleResult = new List<string>();
        public string Format(SparqlResult result)
        {
            tripleResult = result.Variables.ToList();
            
            string subjectVar = tripleResult[0];
            string predicateVar = tripleResult[1];
            string objectVar = tripleResult[2];

            INode _subject = result[subjectVar];
            INode _predicate = result[predicateVar];
            INode _object = result[objectVar];

            string subjectName = _subject?.ToString().Split('#').LastOrDefault();
            string predicateName = _predicate?.ToString().Split('#').LastOrDefault();
            string objectName = _object?.ToString().Split('#').LastOrDefault();

            if (subjectName != null && predicateName != null)
            {
                if (!groupedProperties.TryGetValue(_subject, out var properties))
                {
                    properties = new List<PropertyInfo>();
                    groupedProperties[_subject] = properties;
                }

                properties.Add(new PropertyInfo { Name = predicateName, Value = _object });
            }

            return "ok";
        }

        public string PrintFormattedData()
        {
            foreach (var kvp in groupedProperties)
            {
                INode subject = kvp.Key;
                string sName = subject?.ToString().Split('#').LastOrDefault();

                output.AppendLine($"#{sName}");
                output.AppendLine("\thas Properties:");
                foreach (PropertyInfo propertyInfo in kvp.Value)
                {
                    output.AppendLine($"\t\t{propertyInfo.Name}");
                    if (propertyInfo.Value is IUriNode uriNode)
                    {
                        string typeName = uriNode.ToString().Split('#').LastOrDefault();
                        output.AppendLine($"\t\t\tdataType = {typeName}");
                    }
                    else
                    {
                        output.AppendLine($"\t\t\tdataType = {propertyInfo.Value}");
                    }

                    output.AppendLine();
                }
            }

            return output.ToString();
        }

        public string PrintFormattedInstanceData()
        {
            foreach (var kvp in groupedProperties)
            {
                INode subject = kvp.Key;
                string sName = subject?.ToString().Split('#').LastOrDefault();

                output.AppendLine($"#{sName}");
                output.AppendLine("\thas Properties:");
                foreach (PropertyInfo propertyInfo in kvp.Value)
                {
                    output.AppendLine($"\t\t{propertyInfo.Name}");
                    if (propertyInfo.Value is IUriNode uriNode)
                    {
                        string typeName = uriNode.ToString().Split('#').LastOrDefault();
                        output.AppendLine($"\t\t\t{typeName}");
                    }
                    else
                    {
                        output.AppendLine($"\t\t\t{propertyInfo.Value}");
                    }

                    output.AppendLine();
                }
            }

            return output.ToString();
        }

        public string FormatBooleanResult(bool result)
        {
            throw new NotImplementedException();
        }
    }
}
