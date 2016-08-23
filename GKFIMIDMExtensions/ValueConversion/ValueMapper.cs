using Microsoft.MetadirectoryServices;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;

namespace Mms_ManagementAgent_GKFIMIDMExtensions.ValueConversion
{
    public abstract class ValueMapper
    {
        public static IDictionary<string, ValueMapper> dictConfiguredValueConverters { get; private set; }

        public static void configureConvertersFromXML(XmlElement xmlConverters)
        {
            dictConfiguredValueConverters = new Dictionary<string, ValueMapper>(3);

            if (null != xmlConverters)
                foreach (XmlElement nodeConverter in xmlConverters.SelectNodes("value-converter").OfType<XmlElement>())
                {
                    ValueMapper mapper = createValueMapperFromConfiguration(nodeConverter);
                    dictConfiguredValueConverters.Add(nodeConverter.Attributes["name"].Value, mapper);
                }
        }

        /// <summary>
        /// Transforms an attribute value to another value
        /// </summary>
        public abstract string mapAttribute(string sourceValue);

        public static ValueMapper createValueMapperFromConfiguration(XmlElement configurationElement)
        {
            string mapperType = configurationElement.Attributes["type"].Value;
            switch (mapperType)
            {
                case "regexReplacer":
                    return new ValueRegexReplacer(configurationElement);
                case "unique":
                    return new ValueUniquer(configurationElement);
                case "base64":
                    return new ValueBase64Encoder(configurationElement);
                case "escapedn":
                    return new ValueDNEscaper(configurationElement);
                default:
                    throw new NotImplementedException("Value converters do not support type " + mapperType);
            }
        }
    }
}
