using GKFIMIDMExtensionsTest.FIMIDMDummies;
using Microsoft.MetadirectoryServices;
using Mms_ManagementAgent_GKFIMIDMExtensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKFIMIDMExtensionsTest
{
    [TestFixture]
    public class GKFIMIDMExtensionTest
    {
        //[Test]
        //public void InitializationTest()
        //{
        //    IMASynchronization maExtension = new GKGenericMAExtensionObject();
        //    maExtension.Initialize();
        //}

        [Test]
        public void TestConstantMatch()
        {
            IMASynchronization maExtension = new GKGenericMAExtensionObject();
            
            ValueCollection vc = new ValueCollectionDummy();
            maExtension.MapAttributesForJoin("ConstantMatch:abcdef", new CSEntryDummy(), ref vc);

            Assert.AreEqual(1, vc.Count);
            Assert.AreEqual(AttributeType.String, vc[0].DataType);
            Assert.AreEqual("abcdef", vc[0].ToString());
        }

        [Test]
        public void ProxyAddressesFinderTest()
        {
            IMASynchronization maSync = createMAExtensionFromConfigFile();

            CSEntryDummy cvEntry = new CSEntryDummy();
            AttribDummy proxyAddresses = new AttribDummy("proxyAddresses", null);
            proxyAddresses.IsMultivalued_Set = true;
            proxyAddresses.Values = new ValueCollectionDummy();
            proxyAddresses.Values.Add("smtp:secondary@email.address");
            proxyAddresses.Values.Add("SMTP:primary@email.address");
            proxyAddresses.Values.Add("smtp:tertiary@email.address");
            cvEntry.Attributes.Add("proxyAddresses", proxyAddresses);
            cvEntry.ObjectType_Set = "person";

            MVEntryDummy mvTargetEntry = new MVEntryDummy();
            mvTargetEntry.Attributes.Add("email", new AttribDummy("email", null));
            maSync.MapAttributesForImport("primaryEmailSelector:email", cvEntry, mvTargetEntry);

            Assert.AreEqual("primary@email.address", mvTargetEntry["email"].Value);
        }

        private IMASynchronization createMAExtensionFromConfigFile()
        {
            GKGenericMAExtensionObject maExtension = new GKGenericMAExtensionObject();
            maExtension.configureFromXMLFile(@"..\..\..\GKFIMIDMExtensions\GKFIMIDMExtensions.xml");

            return maExtension;
        }

    }
}
