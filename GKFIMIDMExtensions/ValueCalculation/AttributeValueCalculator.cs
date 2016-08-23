using Microsoft.MetadirectoryServices;
using Mms_ManagementAgent_GKFIMIDMExtensions.MultiValueFiltering;
using System.Xml;
using System.Linq;

namespace Mms_ManagementAgent_GKFIMIDMExtensions
{
    /// <summary>
    /// Evaluates the value of an attribute.
    /// </summary>
    class AttributeValueCalculator : IValueCalculator
    {
        public string AttributeName { get; set; }

        public IMultiValueFilter filter4MultiValues { get; set; }

//        public bool EscapeDN { get; set; }

        public AttributeValueCalculator(XmlElement configurationElement)
            : this(configurationElement.InnerText)
        {
//            if (configurationElement.Attributes["escape"] != null && Convert.ToBoolean(configurationElement.Attributes["escape"].Value))
//                EscapeDN = true;
            if (null != configurationElement.Attributes["select-multivalue"])
                filter4MultiValues = MultiValueNoFilter.dictConfiguredMultiValueFilters[configurationElement.Attributes["select-multivalue"].Value];
        }

        public AttributeValueCalculator (string nameOfTheAttribute)
        {
            AttributeName = nameOfTheAttribute;
            filter4MultiValues = MultiValueNoFilter.instance;
//            EscapeDN = false;
        }

        public string calculateValue(IFIMDirectoryObject entry)
        {
            Attrib targetAttribute = entry[AttributeName];
            //if (EscapeDN)
            //    return QuoteRDN(entry[AttributeName].Value);
            //else
            if (targetAttribute.IsMultivalued)
                return filter4MultiValues.filterValues(targetAttribute.Values.OfType<Value>().Select(val => val.ToString())).First();
            else
                return targetAttribute.Value;
        }
    }
}
