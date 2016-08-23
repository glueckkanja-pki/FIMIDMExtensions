using GKFIMIDMExtensionsTest.FIMIDMDummies;
using Microsoft.MetadirectoryServices;
using Mms_ManagementAgent_GKFIMIDMExtensions;
using Mms_ManagementAgent_GKFIMIDMExtensions.FIMAdapters;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKFIMIDMExtensionsTest
{
    [TestFixture]
    class GKMVExtensionTest
    {
        [Test]
        public void InitializationTest()
        {
            MAProvisioner maProv = createMAProvisionerFromConfigFile();
            Assert.AreEqual("XY AD MA", maProv.TargetMAName);
        }

        private static MAProvisioner createMAProvisionerFromConfigFile()
        {
            GKMVExtension mvExtension = new GKMVExtension();
            mvExtension.configureFromXMLFile(@"..\..\..\GKFIMIDMExtensions\GKMVExtension.xml");

            Assert.AreEqual(1, mvExtension.ConfiguredMAProvisioners.Count);
            MAProvisioner maProv = mvExtension.ConfiguredMAProvisioners.First();

            return maProv;
        }
        
        [Test]
        public void integratedTest()
        {
            MAProvisioner maProv = createMAProvisionerFromConfigFile();

            MVEntryDummy mvEntry = new MVEntryDummy();
            mvEntry.Attributes.Add("cn", new AttribDummy("cn", "Wurst, Hans"));
            mvEntry.Attributes.Add("location", new AttribDummy("location", "OU=Benutzer,OU=Dingens,DC=domain,DC=component"));

            string resultantDN = maProv.dnCalculator.calculateValue(new MVEntryAdapter(mvEntry));

            Assert.AreEqual("CN=Wurst\\, Hans,OU=Benutzer,OU=Dingens,DC=domain,DC=component",resultantDN);
        }

        [Test]
        public void filterTest()
        {
            MAProvisioner maProv = createMAProvisionerFromConfigFile();

            MVEntryDummy mvEntry = new MVEntryDummy();
            mvEntry.Attributes.Add("MandantID", new AttribDummy("MandantID", "10"));
            mvEntry.ObjectType_Set = "person";

            Assert.IsTrue(maProv.SourceObjectFilter.doesApply(new MVEntryAdapter(mvEntry)));

            MVEntryDummy mvGroup = new MVEntryDummy();
            mvGroup.Attributes.Add("MandantID", new AttribDummy("MandantID", "10"));
            mvGroup.ObjectType_Set = "group";

            Assert.IsFalse(maProv.SourceObjectFilter.doesApply(new MVEntryAdapter(mvGroup)));

            MVEntryDummy mvPersonMandantX = new MVEntryDummy();
            mvPersonMandantX.Attributes.Add("MandantID", new AttribDummy("MandantID", "20"));
            mvPersonMandantX.ObjectType_Set = "person";

            Assert.IsFalse(maProv.SourceObjectFilter.doesApply(new MVEntryAdapter(mvPersonMandantX)));

            MVEntryDummy mvPersonMandant12 = new MVEntryDummy();
            mvPersonMandant12.Attributes.Add("MandantID", new AttribDummy("MandantID", "12"));
            mvPersonMandant12.ObjectType_Set = "person";

            Assert.IsTrue(maProv.SourceObjectFilter.doesApply(new MVEntryAdapter(mvPersonMandant12)));
        }
    }
}
