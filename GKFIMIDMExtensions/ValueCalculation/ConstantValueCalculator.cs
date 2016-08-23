using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Mms_ManagementAgent_GKFIMIDMExtensions
{
    /// <summary>
    /// Always returns the same configured value
    /// </summary>
    class ConstantValueCalculator : IValueCalculator
    {
        public string ConstantValue { get; set; }

        public ConstantValueCalculator(XmlElement configurationElement)
            : this(configurationElement.InnerText)
        {
        }

        public ConstantValueCalculator (string value2Return)
        {
            ConstantValue = value2Return;
        }

        public string calculateValue(IFIMDirectoryObject entry)
        {
            return ConstantValue;
        }
    }
}
