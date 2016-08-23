using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Mms_ManagementAgent_GKFIMIDMExtensions.MultiValueFiltering
{
    /// <summary>
    /// Does not filter actually. Can be used as default. It's a singleton and also contains a Factory Method for IMultiValueFilters of all types. Also
    /// contains a static dictionary of configured IMultiValueFilters indexed by name.
    /// </summary>
    public class MultiValueNoFilter : IMultiValueFilter
    {
        public static IDictionary<string, IMultiValueFilter> dictConfiguredMultiValueFilters { get; private set; }

        public static void configureMultiValueFiltersFromXML(XmlElement xmlMultiValueFilters)
        {
            dictConfiguredMultiValueFilters = new Dictionary<string, IMultiValueFilter>(3);

            if (null != xmlMultiValueFilters)
                foreach (XmlElement nodeMultiValueFilter in xmlMultiValueFilters.SelectNodes("multivalue-filter").OfType<XmlElement>())
                {
                    IMultiValueFilter mvFilter = createFilterFromXml(nodeMultiValueFilter);
                    dictConfiguredMultiValueFilters.Add(nodeMultiValueFilter.Attributes["name"].Value, mvFilter);
                }
        }

        public IEnumerable<string> filterValues(IEnumerable<string> sourceValues)
        {
            return sourceValues;
        }

        /// <summary>
        /// No public constructor necessary, it's a singleton
        /// </summary>
        private MultiValueNoFilter()
        {
        }

        private static MultiValueNoFilter _instance = new MultiValueNoFilter();
        /// <summary>
        /// Singleton
        /// </summary>
        public static MultiValueNoFilter instance
        { 
            get
            {
                return _instance;
            }
        }

        public static IMultiValueFilter createFilterFromXml(XmlElement filterConfiguration)
        {
            string attributeType = filterConfiguration.Attributes["type"].Value;
            switch (attributeType)
            {
                case "nofilter":
                    return instance;
                case "regex":
                    return new MultiValueRegexFilter(filterConfiguration);
                default:
                    throw new NotImplementedException("Multivalue-Filtering does not support type " + attributeType);
            }
        }
    }
}
