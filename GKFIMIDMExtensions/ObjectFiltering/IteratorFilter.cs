using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Mms_ManagementAgent_GKFIMIDMExtensions.ObjectFiltering
{
    /// <summary>
    /// Aggregates multiple object filters (IObjectFilter) for one result. Filters are either combined with logical ANDs (all filters must apply)
    /// or with logical ORs (at least one filter must apply) to calculate the result. 
    /// </summary>
    public class IteratorFilter : IObjectFilter
    {
        public enum AggregationType { And, Or };
                
        public ICollection<IObjectFilter> filterCollection { get; set; }

        public AggregationType iteratorAggregation { get; set; }

        public IteratorFilter(XmlElement xmlConfiguration)
        {
            string attributeType = xmlConfiguration.Attributes["type"].Value;
            iteratorAggregation = (AggregationType)Enum.Parse(typeof(AggregationType), attributeType, true);

            filterCollection = new LinkedList<IObjectFilter>(
                xmlConfiguration
                    .ChildNodes
                    .OfType<XmlElement>()
                    .Select<XmlElement, IObjectFilter>(createFilterFromXml)
                );
        }

        public bool doesApply(IFIMDirectoryObject fimObject)
        {
            switch (iteratorAggregation)
            {
                case AggregationType.And:
                    return filterCollection.All(filter => filter.doesApply(fimObject));
                case AggregationType.Or:
                    return filterCollection.Any(filter => filter.doesApply(fimObject));
                default:
                    throw new NotImplementedException("AggregationType \"" + iteratorAggregation + "\" is not implemented.");
            }
        }

        public override string ToString()
        {
            return filterCollection
                .Select<IObjectFilter, string>(filter => filter.ToString())
                .Aggregate((filter1, filter2) => filter1 + " " + iteratorAggregation + " " + filter2);
        }

        public static IObjectFilter createFilterFromXml(XmlElement filterConfiguration)
        {
            string attributeType = filterConfiguration.Attributes["type"].Value;
            switch (attributeType)
            {
                case "attribute":
                    return new AttributeValueFilter(filterConfiguration);
                case "objecttype":
                    return new ObjectTypeFilter(filterConfiguration);
                case "and":
                case "or":
                    return new IteratorFilter(filterConfiguration);
                default:
                    throw new NotImplementedException("Filtering does not support type " + attributeType);
            }
        }
    }
}
