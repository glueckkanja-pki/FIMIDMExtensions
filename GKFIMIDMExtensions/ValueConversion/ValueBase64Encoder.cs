using Microsoft.MetadirectoryServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Mms_ManagementAgent_GKFIMIDMExtensions.ValueConversion
{
    public class ValueBase64Encoder : ValueMapper
    {
        public Encoding StringEncoding { get; set; }

        public ValueBase64Encoder(XmlElement configurationElement)
        {
            StringEncoding = Encoding.UTF8;
        }

        public override string mapAttribute(string sourceValue)
        {
            return Convert.ToBase64String(StringEncoding.GetBytes(sourceValue));
        }
    }
}
