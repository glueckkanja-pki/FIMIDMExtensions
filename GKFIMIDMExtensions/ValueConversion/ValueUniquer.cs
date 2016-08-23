using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Mms_ManagementAgent_GKFIMIDMExtensions.ValueConversion
{
    /// <summary>
    /// Appends a configurable suffix to a value. This makes the value unique even if identical values are imported from different directories, 
    /// assuming that the suffix is different for each directory. A hashing techinque ensures that the length never exceeds a configurable
    /// maximum length, for example 19 characters for sAMAccountName
    /// </summary>
    public class ValueUniquer : ValueMapper
    {
        public uint MaximumAttributeValueLength { get; protected set; }
        public uint RandomizationLength { get; protected set; }
        
        /// <summary>
        /// The maximum length allowed for RandomizationLength. Currently, it is the Base64-encoded length of the hash used, which is one third more than the length in bytes.
        /// </summary>
        public const uint MAX_RANDOMIZATION_LENGTH = 11;

        /// <summary>
        /// A collection of configuration string to ValueUnifier mappings. This allows to skip the construction of new ValueUnifier objects if the configuration string is identical.
        /// </summary>
        protected static IDictionary<string, ValueUniquer> dictUnifiers = new Dictionary<string, ValueUniquer>(3);

        public ValueUniquer(XmlElement configurationElement)
        {
            MaximumAttributeValueLength = Convert.ToUInt32(configurationElement.Attributes["max-length"].Value);
            RandomizationLength = Convert.ToUInt32(configurationElement.Attributes["randomization-length"].Value);
        }

        ///// <summary>
        ///// Singleton/Factory Method to receive one ValueUnifier for each configuration string. If a ValueUnifier for the configuration string already exists, the existing
        ///// one is returned.
        ///// </summary>
        //public static ValueUnifier getValueUnifier4ConfigurationString(string configurationString)
        //{
        //    if (!dictUnifiers.ContainsKey(configurationString))
        //        dictUnifiers[configurationString] = new ValueUnifier(configurationString);

        //    return dictUnifiers[configurationString];
        //}

        ///// <summary>
        ///// [Suffix;lenHashSuffix;lenMaximum]:SourceAttribute->TargetAttribute - The length of lenMaximum is never exceeded. In case the original value + suffix is 
        /////         longer than lenMaximum, the result of original value + suffix is hashed. lenHashSuffix characters of the Base64-encoded hash are
        /////         appended to the capped original value, such that the result has the length lenMaximum
        ///// </summary>
        //protected ValueUnifier(string configurationString)
        //{
        //    // parse FlowRuleName for parameters
        //    string[] commandAndAttributes = configurationString.Split(':');
        //    if (commandAndAttributes.Length != 2)
        //        throw new ArgumentException("The flow rule \"uni-suffix\" has an invalid format. The flow rule name must contain exactly one colon (':') that separates the parameterized name from the attributes to map");
        //    string paramlist = commandAndAttributes[0];
        //    Name = "uni-suffix" + paramlist;
        //    if (paramlist.Length < "[x;x;x]".Length || paramlist[0] != '[' || paramlist[paramlist.Length - 1] != ']')
        //        throw new ArgumentException("The flow rule \"uni-suffix\" has an invalid format. A parameter list must succeed the name \"uni-suffix\". Square brackets ([]) must enclose the parameter list and the three parameters must be separated with semicolons.");
        //    string[] randomizationParameters = paramlist.Substring(1, paramlist.Length - 2)  // trim []
        //            .Split(';');
        //    if (randomizationParameters.Length != 3)
        //        throw new ArgumentException("The parameter list must contain exactly three parameters. uni-suffix" + paramlist + " does not have exactly two semicolons.");
        //    Suffix = randomizationParameters[0];
        //    RandomizationLength = uint.Parse(randomizationParameters[1]);
        //    if (RandomizationLength > MAX_RANDOMIZATION_LENGTH)
        //        throw new ArgumentOutOfRangeException("RandomizationLength", RandomizationLength, "RandomizationLength must not exceed MAX_RANDOMIZATION_LENGTH, which is " + MAX_RANDOMIZATION_LENGTH);
        //    MaximumAttributeValueLength = uint.Parse(randomizationParameters[2]);
        //    if (MaximumAttributeValueLength < RandomizationLength || MaximumAttributeValueLength <= 0)
        //        throw new ArgumentOutOfRangeException("MaximumAttributeValueLength", MaximumAttributeValueLength, "The maximum value must be at least as long as the randomized part (length "
        //            + RandomizationLength + ") and must be greater than zero.");

        //    parseConfiguration(commandAndAttributes[1]);
        //}

        public override string mapAttribute (string sourceValue)
        {
            if (sourceValue.Length <= MaximumAttributeValueLength)
                return sourceValue;
            else // the attribute value is too long to append the suffix... this means we will use the hash
            {
                string b64Hash = computeHashCode(sourceValue, RandomizationLength);
                return sourceValue.Substring(0, (int)(MaximumAttributeValueLength - RandomizationLength)) + b64Hash;
            }
        }

        /// <summary>
        /// Computes a secure hash code of a text (currently with SHA256) and returns hashLength characters of its Base64 representation
        /// </summary>
        protected static string computeHashCode(string text2Hash, uint hashLength)
        {
            HashAlgorithm hasher = SHA256Managed.Create();  // SHA256Managed is not thread-save, therefore it must be constructed on every computation
            return Convert.ToBase64String(hasher.ComputeHash(System.Text.Encoding.UTF8.GetBytes(text2Hash)))
                .Substring(0, (int)hashLength);
        }
    }
}
