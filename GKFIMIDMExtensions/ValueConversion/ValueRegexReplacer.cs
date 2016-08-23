using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Mms_ManagementAgent_GKFIMIDMExtensions.ValueConversion
{
    /// <summary>
    /// Replaces values identified via RegEx patterns with constant replacements. All others are passed through unchanged.
    /// </summary>
    public class ValueRegexReplacer : ValueMapper
    {
        public ICollection<Tuple<Regex,string>> MappingList { get; set; }

        /// <summary>
        /// Creates a new ValueRegexReplacer based on an XML configuration
        /// </summary>
        /// <example>
        ///     <value-converter name="RecipientTypeDetailsSource2Target">
        ///         <value-mapping source-value="^1$" target-value="128" />
        ///         <value-mapping source-value="^128$" />
        ///     </value-converter>
        /// </example>
        public ValueRegexReplacer(XmlNode configurationNode)
        {
            MappingList = new LinkedList<Tuple<Regex, string>>();
            foreach (XmlNode valueMapping in configurationNode.SelectNodes("value-mapping"))
            {
                Regex pattern = new Regex(valueMapping.Attributes["source-value"].Value, RegexOptions.Compiled);
                string replacement = valueMapping.Attributes["target-value"] == null ? null : valueMapping.Attributes["target-value"].Value;
                MappingList.Add(new Tuple<Regex,string>(pattern, replacement));
            }
        }
        
        public override string mapAttribute(string sourceValue)
        {
            foreach (Tuple<Regex, string> replacement in MappingList)
                if (replacement.Item1.IsMatch(sourceValue))
                    if (null == replacement.Item2)
                        return null;
                    else
                        return replacement.Item1.Replace(sourceValue, replacement.Item2);

            return sourceValue; // default behavior
        }
    }
}
