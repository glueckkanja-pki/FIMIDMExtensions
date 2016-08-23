using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Mms_ManagementAgent_GKFIMIDMExtensions.ValueCalculation
{
    class ValueCalculatorConcatenator : IValueCalculator
    {
        public ICollection<IValueCalculator> CalculatorCollection { get; set; }

        public ValueCalculatorConcatenator ()
        {
            CalculatorCollection = new LinkedList<IValueCalculator>();
        }
        
        public ValueCalculatorConcatenator (IEnumerable<IValueCalculator> startValues)
        {
            CalculatorCollection = new LinkedList<IValueCalculator>(startValues);
        }

        public ValueCalculatorConcatenator (XmlElement configurationElement)
        {
            CalculatorCollection = new LinkedList<IValueCalculator>(
                configurationElement
                    .ChildNodes
                    .OfType<XmlElement>()
                    .Select<XmlElement, IValueCalculator>(createValueCalculatorFromConfiguration)
            );
        }

        public string calculateValue(IFIMDirectoryObject entry)
        {
            return CalculatorCollection
                .Select<IValueCalculator, string>(calculator => calculator.calculateValue(entry))   // calls all calculators
                .Aggregate((s1, s2) => s1 + s2);                                                    // concatenates all strings
        }

        public static IValueCalculator createValueCalculatorFromConfiguration(XmlElement configurationElement)
        {
            string attributeType = configurationElement.Attributes["type"].Value;
            switch (attributeType)
            {
                case "attribute":
                    return new AttributeValueCalculator(configurationElement);
                case "constant":
                    return new ConstantValueCalculator(configurationElement);
                case "concatenator":
                    return new ValueCalculatorConcatenator(configurationElement);
                case "convert":
                    return new ConversionValueCalculator(configurationElement);
                default:
                    throw new NotImplementedException("Value calculations do not support type " + attributeType);
            }
        }
    }
}
