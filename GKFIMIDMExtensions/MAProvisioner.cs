using Mms_ManagementAgent_GKFIMIDMExtensions.ObjectFiltering;
using Mms_ManagementAgent_GKFIMIDMExtensions.ValueCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Mms_ManagementAgent_GKFIMIDMExtensions
{
    public class MAProvisioner
    {
        /// <summary>
        /// The MA's name this MAProvisioner is configured for
        /// </summary>
        public string TargetMAName { get; set; }

        public string TargetObjectType { get; set; }

        public string TargetAnchorAttribute { get; set; }

        /// <summary>
        /// Which MVEntries shall be provisioned?
        /// </summary>
        public IObjectFilter SourceObjectFilter { get; set; }

        /// <summary>
        /// Which DN will the CS entry in the Target MA have?
        /// </summary>
        public IValueCalculator dnCalculator { get; protected set; }

        public MAProvisioner(XmlNode provisioningData, IDictionary<string,IValueCalculator> availableCalculators)
        {
            TargetMAName = provisioningData.Attributes["name"].Value;
            TargetObjectType = provisioningData.SelectSingleNode("target-object-type").InnerText;
            SourceObjectFilter = IteratorFilter.createFilterFromXml((XmlElement)provisioningData.SelectSingleNode("filter"));

            XmlNode nodeDn = provisioningData.SelectSingleNode("dn");
            dnCalculator = availableCalculators[nodeDn.Attributes["calculator"].Value];
            if (null != nodeDn.Attributes["attribute"])
                TargetAnchorAttribute = nodeDn.Attributes["attribute"].Value;
        }
    }
}
