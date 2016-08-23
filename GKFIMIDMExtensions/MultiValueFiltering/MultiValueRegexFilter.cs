using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Mms_ManagementAgent_GKFIMIDMExtensions.MultiValueFiltering
{
    /// <summary>
    /// Returns only those values in an IEnumerable that match a given Regular Expression
    /// </summary>
    class MultiValueRegexFilter : IMultiValueFilter
    {
        public Regex FilterValue { get; set; }

        public MultiValueRegexFilter(XmlElement xmlConfiguration)
            : this(xmlConfiguration.InnerText)
        {
        }

        public MultiValueRegexFilter(string pattern)
        {
            FilterValue = new Regex(pattern, RegexOptions.Compiled);
        }

        public IEnumerable<string> filterValues(IEnumerable<string> sourceValues)
        {
            return sourceValues.Where(value => FilterValue.IsMatch(value));
        }
    }
}
