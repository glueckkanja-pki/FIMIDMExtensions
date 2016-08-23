using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Mms_ManagementAgent_GKFIMIDMExtensions.ObjectFiltering
{
    public class ObjectTypeFilter : IObjectFilter
    {
        public string ApplicableObjectType { get; set; }

        public ObjectTypeFilter(XmlElement xmlConfiguration)
        {
            ApplicableObjectType = xmlConfiguration.InnerText;
        }

        public bool doesApply(IFIMDirectoryObject fimObject)
        {
            return ApplicableObjectType == fimObject.objectType;
        }

        public override string ToString()
        {
            return "ObjectType=" + ApplicableObjectType;
        }
    }
}
