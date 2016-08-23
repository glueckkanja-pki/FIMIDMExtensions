using Mms_ManagementAgent_GKFIMIDMExtensions;
using Mms_ManagementAgent_GKFIMIDMExtensions.ValueConversion;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GKFIMIDMExtensionsTest
{
    [TestFixture]
    public class ValueUniquerTest
    {
        public const string CONFIGURATION_XML =
            "<value-converter name=\"sAMAccountNameSuffixScrambler\" type=\"unique\" max-length=\"20\" randomization-length=\"6\" />";
        public const string CONFIGURATION_XML_4_SCRAMBLING_ONLY =
            "<value-converter name=\"Scrambler\" type=\"unique\" max-length=\"4\" randomization-length=\"4\" />";

        public ValueUniquer createUniquerFromXML()
        {
            string configurationXml = CONFIGURATION_XML;
            return createUniquerFromXML(configurationXml);
        }

        private static ValueUniquer createUniquerFromXML(string configurationXml)
        {
            XmlDocument xmlVrrConfig = new XmlDocument();
            xmlVrrConfig.LoadXml(configurationXml);
            return new ValueUniquer((XmlElement)xmlVrrConfig.FirstChild);
        }

        [Test]
        public void constructValueUnifier()
        {
            ValueUniquer vu = createUniquerFromXML();

            Assert.AreEqual(6, vu.RandomizationLength);
            Assert.AreEqual(20, vu.MaximumAttributeValueLength);
        }

        [Test]
        public void mapSomeShortStrings()
        {
            ValueUniquer vu = createUniquerFromXML();

            Assert.AreEqual("test", vu.mapAttribute("test"));
            Assert.AreEqual("test123", vu.mapAttribute("test123"));
            Assert.AreEqual("12345678901234567890", vu.mapAttribute("12345678901234567890"));
        }

        [Test]
        public void mapSomeLongStrings()
        {
            const string longValue = "123456789012345678901";  // 21 characters, so longValue > maxLength

            ValueUniquer vu = createUniquerFromXML();

            string longShortenedValue = vu.mapAttribute(longValue);

            Assert.AreNotEqual(longValue, longShortenedValue);
            Assert.AreEqual(longShortenedValue, vu.mapAttribute(longValue));
            Assert.AreEqual(20, longShortenedValue.Length);
            Assert.AreNotEqual(longShortenedValue, vu.mapAttribute(longValue + "x"));
            Assert.AreNotEqual(longShortenedValue, vu.mapAttribute("12345678901234567890x"));
            Assert.IsTrue(longShortenedValue.StartsWith("12345678901234"));   // 14 characters = 20 - 6 = maxLength - RandomizationLength
        }

        /// <summary>
        /// In this test, the expected hash value is hard-coded, so consistent hash values among changing .NET versions are tested
        /// </summary>
        [Test]
        public void testHashConsistency()
        {
            const string longValue = "123456789012345678abcXY";

            ValueUniquer vu = createUniquerFromXML();

            string longSuffixedValue = vu.mapAttribute(longValue);

            Assert.AreEqual("12345678901234VyCKta", longSuffixedValue);
        }

        [Test]
        public void testScramblingOnly()
        {
            ValueUniquer vu = createUniquerFromXML(CONFIGURATION_XML_4_SCRAMBLING_ONLY);

            Assert.AreEqual(4, vu.mapAttribute("1234567890").Length);
            Assert.AreEqual(4, vu.mapAttribute("123456").Length);
            Assert.AreEqual("1234", vu.mapAttribute("1234"));
            Assert.AreEqual("12", vu.mapAttribute("12"));
        }
    }
}
