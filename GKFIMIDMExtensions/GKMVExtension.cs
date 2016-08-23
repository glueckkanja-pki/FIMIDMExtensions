using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.MetadirectoryServices;
using System.IO;
using System.Xml;
using log4net;
using Mms_ManagementAgent_GKFIMIDMExtensions.FIMAdapters;

namespace Mms_ManagementAgent_GKFIMIDMExtensions
{
    public class GKMVExtension : GKAbstractFIMExtension, IMVSynchronization
    {
        protected override string ConfigurationFileName
        {
            get { return "GKMVExtension.xml"; }
        }

        protected override string LoggingConfigurationFileName
        {
            get { return "gkfimextensions.log4net.config"; }
        }

        //public IDictionary<string, ICollection<MAProvisioner>> ConfiguredMAProvisioners4SourceObjectType = new Dictionary<string, ICollection<MAProvisioner>>(1);
        public ICollection<MAProvisioner> ConfiguredMAProvisioners = new LinkedList<MAProvisioner>();

        private static ILog log
        {
            get
            {
                return LogManager.GetLogger("GK.FIM.IDMExtensions.GKMVExtension");
            }
        }

        //public void addMAProvisioner4SourceObjectType(string sourceObjectType, MAProvisioner newProvisoner)
        //{
        //    if (!ConfiguredMAProvisioners4SourceObjectType.ContainsKey(sourceObjectType))
        //        ConfiguredMAProvisioners4SourceObjectType.Add(sourceObjectType, new LinkedList<MAProvisioner>());

        //    ConfiguredMAProvisioners4SourceObjectType[sourceObjectType].Add(newProvisoner);
        //}

        public override void configureFromXML(XmlElement xmlConfig)
        {
            base.configureFromXML(xmlConfig);

            foreach (XmlNode nodeOT in xmlConfig.SelectNodes("provisioning/target-ma"))
            {
                MAProvisioner maProv = new MAProvisioner(nodeOT, dictConfiguredValueCalculators);
                ConfiguredMAProvisioners.Add(maProv);
                //addMAProvisioner4SourceObjectType(maProv.SourceObjectType, maProv);
                log.Info("Configuration read: Provisioning objects with filter " + maProv.SourceObjectFilter + " as " + maProv.TargetObjectType + " to MA " + maProv.TargetMAName);
            }
        }

        public void Provision(MVEntry mventry)
        {
            log.Debug("Provision called");
            log.Debug("Provision of " + mventry.ObjectType + " [" + mventry.ObjectID.ToString() + "]. Connected MAs: " +
                mventry.ConnectedMAs
                    .OfType<ConnectedMA>()
                    .Select<ConnectedMA,string>(ma => ma.Name + "(#"+ ma.Connectors.Count + ")")
                    .Aggregate((ma1, ma2) => ma1 + ", " + ma2)
                );

            MVEntryAdapter adaptedMvEntry = new MVEntryAdapter(mventry);

            //if (ConfiguredMAProvisioners4SourceObjectType.ContainsKey(mventry.ObjectType))
            //{
            IEnumerable<MAProvisioner> maProvisioners2Apply =
                //ConfiguredMAProvisioners4SourceObjectType[mventry.ObjectType]        // these shall be provisoned
                ConfiguredMAProvisioners
                    .Where(maprov =>
                        mventry
                            .ConnectedMAs
                            .OfType<ConnectedMA>()                      // make them linqable
                            .Where(ma => ma.Connectors.Count > 0)       // already provisioned
                            .All(ma => ma.Name != maprov.TargetMAName)  // is the MA of the MAProvisioners not yet provisioned?
                        &&
                        maprov.SourceObjectFilter.doesApply(adaptedMvEntry) // filter mvEntries 
                    );

            if (maProvisioners2Apply.Any())
            {
                log.Info("Provisioning to the following MAs: " + maProvisioners2Apply.Select(maProv => maProv.TargetMAName).Aggregate((prov1, prov2) => prov1 + " " + prov2));
                foreach (MAProvisioner maProv in maProvisioners2Apply)
                {
                    ConnectedMA ma2Provision = mventry.ConnectedMAs[maProv.TargetMAName];

                    log.Info("Provisioning a " + mventry.ObjectType + " from Metaverse to MA " + ma2Provision.Name + " as type " + maProv.TargetObjectType);
                    CSEntry newConnector = ma2Provision.Connectors.StartNewConnector(maProv.TargetObjectType);
                    string dnValue = maProv.dnCalculator.calculateValue(adaptedMvEntry);
                    newConnector.DN = ma2Provision.CreateDN(dnValue);
                    if (null != maProv.TargetAnchorAttribute)
                        newConnector[maProv.TargetAnchorAttribute].Value = dnValue;
                    newConnector.CommitNewConnector();
                }
            }
            //}

            //foreach (ConnectedMA ma2Provision in 
            //            mventry
            //                .ConnectedMAs
            //                .OfType<ConnectedMA>()                      // make them linqable
            //                .Where(ma => ma.Connectors.Count == 0)      // not yet provisioned
            //                .Where(ma => ConfiguredMAProvisoners.Keys.Contains(ma.Name))) // Provision was configured

        }

        public bool ShouldDeleteFromMV(CSEntry csentry, MVEntry mventry)
        {
            throw new NotImplementedException();
        }

        public void Terminate()
        {
            log.Debug("Terminating.");
        }
    }
}
