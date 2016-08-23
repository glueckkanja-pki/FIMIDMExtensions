using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Mms_ManagementAgent_GKFIMIDMExtensions.ObjectFiltering
{
    public class AttributeValueFilter : IObjectFilter
    {
        public string AttributeName2Filter { get; set; }

        public Regex FilterValue { get; set; }

        public bool DefaultIfNotAvailable { get; set; }

        public AttributeValueFilter(XmlElement xmlConfiguration)
        {
            FilterValue = new Regex(xmlConfiguration.InnerText, RegexOptions.Compiled);
            AttributeName2Filter = xmlConfiguration.Attributes["attribute"].Value;
            DefaultIfNotAvailable = false;
        }

        public bool doesApply(IFIMDirectoryObject fimObject)
        {
            if (null == fimObject[AttributeName2Filter])
                return DefaultIfNotAvailable;

            return FilterValue.IsMatch(fimObject[AttributeName2Filter].Value);
        }

        public override string ToString()
        {
            return FilterValue.ToString() + " matches " + AttributeName2Filter;
        }
    }
}
