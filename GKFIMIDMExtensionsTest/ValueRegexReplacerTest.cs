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
    public class ValueRegexReplacerTest
    {
        public const string CONFIGURATION_XML =
            "<value-converter name=\"RecipientTypeDetailsSource2Target\" type=\"ValueRegexReplacer\">" +
            "    <value-mapping source-value=\"^1$\" target-value=\"128\" />" +
            "    <value-mapping source-value=\"^128$\" />" +
            "    <value-mapping source-value=\"START([a-z0-9 ]+)END\" target-value=\"$1\" />" +
            "</value-converter>";

        private static ValueRegexReplacer createValueRegexReplacerFromConfiguration()
        {
            XmlDocument xmlVrrConfig = new XmlDocument();
            xmlVrrConfig.LoadXml(CONFIGURATION_XML);
            ValueRegexReplacer vrrPrototype = new ValueRegexReplacer(xmlVrrConfig.FirstChild);
            return vrrPrototype;
        }

        [Test]
        public void constructValueRegexReplacer()
        {
            ValueRegexReplacer vrrPrototype = createValueRegexReplacerFromConfiguration();

            Assert.AreEqual(3, vrrPrototype.MappingList.Count);
        }

        [Test]
        public void mapSomeStrings()
        {
            ValueRegexReplacer vrrPrototype = createValueRegexReplacerFromConfiguration();

            Assert.AreEqual("2", vrrPrototype.mapAttribute("2"));
            Assert.AreEqual("128", vrrPrototype.mapAttribute("1"));
            Assert.AreEqual(null, vrrPrototype.mapAttribute("128"));
            Assert.AreEqual("713", vrrPrototype.mapAttribute("713"));
            Assert.AreEqual("10", vrrPrototype.mapAttribute("10"));
        }

        [Test]
        public void mapWithGroups()
        {
            ValueRegexReplacer vrrPrototype = createValueRegexReplacerFromConfiguration();

            Assert.AreEqual("START", vrrPrototype.mapAttribute("START"));
            Assert.AreEqual("STARTEND", vrrPrototype.mapAttribute("STARTEND"));
            Assert.AreEqual("test", vrrPrototype.mapAttribute("STARTtestEND"));
            Assert.AreEqual("123  middle  456", vrrPrototype.mapAttribute("123 START middle END 456"));
            Assert.AreEqual(" content1  content2  content3 ", vrrPrototype.mapAttribute("START content1 END content2 START content3 END"));
        }
    }
}
