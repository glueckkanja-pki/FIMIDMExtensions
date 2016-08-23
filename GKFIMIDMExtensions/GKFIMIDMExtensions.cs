
using System;
using Microsoft.MetadirectoryServices;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using log4net;
using System.Linq;
using Mms_ManagementAgent_GKFIMIDMExtensions.ValueCalculation;
using Mms_ManagementAgent_GKFIMIDMExtensions.FIMAdapters;

namespace Mms_ManagementAgent_GKFIMIDMExtensions
{
    /// <summary>
    /// Summary description for MAExtensionObject.
	/// </summary>
	public class GKGenericMAExtensionObject : GKAbstractFIMExtension, IMASynchronization
	{
        protected override string ConfigurationFileName
        {
            get { return "GKFIMIDMExtensions.xml"; }
        }

        protected override string LoggingConfigurationFileName
        {
            get { return "gkfimextensions.log4net.config"; }
        }

        private IEnumerable<string> lstObjectTypes2Delete;

        private static ILog log
        {
            get
            {
                return LogManager.GetLogger("GK.FIM.IDMExtensions.GKGenericMAExtensionObject");
            }
        }

        public override void configureFromXML(XmlElement xmlConfig)
        {
            base.configureFromXML(xmlConfig);

            LinkedList<string> lstObjectTypes2Delete = new LinkedList<string>();
            foreach (XmlNode nodeOT in xmlConfig.SelectNodes("deprovisioning/delete-objecttype/text()"))
                lstObjectTypes2Delete.AddLast(nodeOT.Value);
            this.lstObjectTypes2Delete = lstObjectTypes2Delete;
        }

        void IMASynchronization.Terminate ()
        {
        }

        bool IMASynchronization.ShouldProjectToMV (CSEntry csentry, out string MVObjectType)
        {
			throw new EntryPointNotImplementedException();
		}

        DeprovisionAction IMASynchronization.Deprovision (CSEntry csentry)
        {
            log.Debug("=>Deprovision");
            if (null == lstObjectTypes2Delete)
                throw new EntryPointNotImplementedException();

            if (contains(lstObjectTypes2Delete, csentry.ObjectType))
                return DeprovisionAction.Delete;
            else
                return DeprovisionAction.Disconnect;
        }	

        bool IMASynchronization.FilterForDisconnection (CSEntry csentry)
        {
            throw new EntryPointNotImplementedException();
		}

		void IMASynchronization.MapAttributesForJoin (string FlowRuleName, CSEntry csentry, ref ValueCollection values)
        {
            log.Debug("=>MapAttributesForJoin");

            if (FlowRuleName.StartsWith("ConstantMatch:", StringComparison.InvariantCultureIgnoreCase))
            {
                // Matches something like ConstantMatch:FHHNet
                string attributeValue = FlowRuleName.Substring("ConstantMatch:".Length);
                values.Add(attributeValue);
            }
            else
			    throw new EntryPointNotImplementedException();
        }

        bool IMASynchronization.ResolveJoinSearch (string joinCriteriaName, CSEntry csentry, MVEntry[] rgmventry, out int imventry, ref string MVObjectType)
        {
            throw new EntryPointNotImplementedException();
		}

        /// <summary>
        /// Depending on the FlowRuleName, writes special properties. The following FlowRules are available:
        ///     tombstone - Writes a comment if the object is deleted in the source directory
        ///     uni-suffix[Suffix;lenHashSuffix;lenMaximum]:SourceAttribute->TargetAttribute - copies the value of the SourceAttribute to the value in the
        ///         TargetAttribute, appending the Suffix. However, the length of lenMaximum is never exceeded. In case the original value + suffix is 
        ///         longer than lenMaximum, the result of original value + suffix is hashed. lenHashSuffix characters of the Base64-encoded hash are
        ///         appended to the capped original value, such that the result has the length lenMaximum
        ///     configured:XYZ:SourceAttribute->TargetAttribute - XYZ must be configured as value-converter in the configuration file. XYZ is the value
        ///         of the name attribute.
        /// </summary>
        /// <param name="FlowRuleName">One of the allowed flow rules, possible with parameters</param>
        /// <param name="csentry">The source object to be read from</param>
        /// <param name="mventry">The target object to be written to</param>
        /// <example>uni-suffix[XY;6;19]:sAMAccountName->accountName-domainSuffixed</example>
        void IMASynchronization.MapAttributesForImport( string FlowRuleName, CSEntry csentry, MVEntry mventry)
        {
            log.Debug("=>MapAttributesForImport");
            try
            {
                string targetAttribute;
                IValueCalculator vChosenCalculator;
                parseFlowRuleName(FlowRuleName, out vChosenCalculator, out targetAttribute);
                mventry[targetAttribute].Value = vChosenCalculator.calculateValue(new CSEntryAdapter(csentry));
            }
            catch (Exception ex)
            {
                log.Error("Error in MapAttributesForImport", ex);
                throw;
            }
            log.Debug("<=MapAttributesForImport");
        }

        private void parseFlowRuleName(string FlowRuleName, out IValueCalculator vChosenCalculator, out string targetAttribute)
        {
            string[] flowRuleComponents = FlowRuleName.Split(':');
            if (flowRuleComponents.Length != 2)
                throw new ArgumentException("The flow rule name XYZ:TargetAttribute must contain exactly one colon, which is not the case for " + FlowRuleName);
            string valueCalculatorName = flowRuleComponents[0];
            targetAttribute = flowRuleComponents[1];

            if (!dictConfiguredValueCalculators.ContainsKey(valueCalculatorName))
                throw new InvalidOperationException("The value-calculator " + valueCalculatorName + " was chosen in Flow Rule Name " + FlowRuleName + ", " +
                    "but no such value-calculator was configured. The name attribute of a value-calculator and the name of the Flow Rule must match (case sensitive)");

            vChosenCalculator = dictConfiguredValueCalculators[valueCalculatorName];
        }

        void IMASynchronization.MapAttributesForExport (string FlowRuleName, MVEntry mventry, CSEntry csentry)
        {
            log.Debug("=>MapAttributesForExport");
            try
            {
                string targetAttribute;
                IValueCalculator vChosenCalculator;
                parseFlowRuleName(FlowRuleName, out vChosenCalculator, out targetAttribute);
                csentry[targetAttribute].Value = vChosenCalculator.calculateValue(new MVEntryAdapter(mventry));
            }
            catch (Exception ex)
            {
                log.Error("Error in MapAttributesForExport", ex);
                throw;
            }
            log.Debug("<=MapAttributesForExport");
        }

#region Helper Methods
        static public bool contains(IEnumerable<string> list, string entry)
        {
            foreach (string currentEntry in list)
                if (entry == currentEntry)
                    return true;
            return false;
        }
#endregion Helper Methods

    }
}
