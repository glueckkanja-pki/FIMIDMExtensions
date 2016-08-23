using log4net;
using Microsoft.MetadirectoryServices;
using Mms_ManagementAgent_GKFIMIDMExtensions.MultiValueFiltering;
using Mms_ManagementAgent_GKFIMIDMExtensions.ValueCalculation;
using Mms_ManagementAgent_GKFIMIDMExtensions.ValueConversion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Mms_ManagementAgent_GKFIMIDMExtensions
{
    public abstract class GKAbstractFIMExtension
    {
        protected IDictionary<string, IValueCalculator> dictConfiguredValueCalculators;

        private static ILog log
        {
            get
            {
                return LogManager.GetLogger("GK.FIM.IDMExtensions.GKAbstractFIMExtension");
            }
        }

        protected abstract string ConfigurationFileName { get; }
        protected abstract string LoggingConfigurationFileName { get;  }

        public virtual void Initialize()
        {
            // Initialize log4net
            string sPath2Log4Netconfig = Path.Combine(Utils.WorkingDirectory, LoggingConfigurationFileName);
            if (string.IsNullOrEmpty(sPath2Log4Netconfig) || !File.Exists(sPath2Log4Netconfig))
                sPath2Log4Netconfig = Path.Combine(Utils.ExtensionsDirectory, LoggingConfigurationFileName);

            if (!string.IsNullOrEmpty(sPath2Log4Netconfig))
            {
                log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(sPath2Log4Netconfig));
                log.Info("Initialization of GKAbstractFIMExtension");
            }

            string path2ConfigFile = Path.Combine(Utils.WorkingDirectory, ConfigurationFileName);
            if (!File.Exists(path2ConfigFile))
            {
                path2ConfigFile = Path.Combine(Utils.ExtensionsDirectory, ConfigurationFileName);  // Fallback to global configuration
                log.Warn("No MA/MV-specific config file in " + path2ConfigFile + ", fallback to global configuration " + path2ConfigFile);
            }

            if (!File.Exists(path2ConfigFile))
            {
                log.Warn("No configuration file exists. Some features may not be available.");
                return;
            }

            try
            {
                configureFromXMLFile(path2ConfigFile);
            }
            catch (Exception ex)
            {
                log.Fatal(ex);
                throw;
            }
        }

        public void configureFromXMLFile(string path2ConfigFile)
        {
            try
            {
                XmlDocument xmlConfig = new XmlDocument();
                xmlConfig.Load(path2ConfigFile);

                configureFromXML((XmlElement)xmlConfig.SelectSingleNode("/*"));
            }
            catch (Exception ex)
            {
                throw new FormatException("Problem parsing XML configuration file " + path2ConfigFile, ex);
            }
        }

        public virtual void configureFromXML(XmlElement xmlConfig)
        {
            ValueMapper.configureConvertersFromXML((XmlElement)xmlConfig.SelectSingleNode("converters"));
            MultiValueNoFilter.configureMultiValueFiltersFromXML((XmlElement)xmlConfig.SelectSingleNode("multivalue-filters"));

            dictConfiguredValueCalculators = new Dictionary<string, IValueCalculator>(3);
            foreach (XmlElement vcElement in xmlConfig.SelectNodes("calculators/value-calculator").OfType<XmlElement>())
            {
                IValueCalculator vCalc = ValueCalculatorConcatenator.createValueCalculatorFromConfiguration(vcElement);
                dictConfiguredValueCalculators.Add(vcElement.Attributes["name"].Value, vCalc);
            }
        }

        //public virtual void ConfigureFromXML(XmlNode configurationXml)
        //{

        //}
    }
}
