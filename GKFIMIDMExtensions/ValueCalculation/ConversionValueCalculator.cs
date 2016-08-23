using Microsoft.MetadirectoryServices;
using Mms_ManagementAgent_GKFIMIDMExtensions.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Mms_ManagementAgent_GKFIMIDMExtensions.ValueCalculation
{
    class ConversionValueCalculator : IValueCalculator
    {
        public ValueMapper MappingFilter { get; set; }
        public IValueCalculator BackendCalculator { get; set; }

        public ConversionValueCalculator(ValueMapper mapper, IValueCalculator unmappedCalculator)
        {
            MappingFilter = mapper;
            BackendCalculator = unmappedCalculator;
        }

        public ConversionValueCalculator(XmlElement configurationElement)
        {
            MappingFilter = ValueMapper.dictConfiguredValueConverters[configurationElement.Attributes["converter"].Value];
            BackendCalculator = ValueCalculatorConcatenator.createValueCalculatorFromConfiguration((XmlElement)configurationElement.SelectSingleNode("value-calculator"));
        }

        public string calculateValue(IFIMDirectoryObject entry)
        {
            return MappingFilter.mapAttribute(
                BackendCalculator.calculateValue(entry)
                );
        }
    }
}
