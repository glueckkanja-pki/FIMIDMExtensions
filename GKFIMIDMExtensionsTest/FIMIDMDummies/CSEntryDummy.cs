using Microsoft.MetadirectoryServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKFIMIDMExtensionsTest.FIMIDMDummies
{
    class CSEntryDummy : CSEntry
    {
        public override void CommitNewConnector()
        {
            throw new NotImplementedException();
        }

        public override DateTime ConnectionChangeTime
        {
            get { throw new NotImplementedException(); }
        }

        public override RuleType ConnectionRule
        {
            get { throw new NotImplementedException(); }
        }

        public override ConnectionState ConnectionState
        {
            get { throw new NotImplementedException(); }
        }

        public override ReferenceValue DN
        {
            get; set;
        }

        public override void Deprovision()
        {
            throw new NotImplementedException();
        }

        public override AttributeNameEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        private ManagementAgent _MA;
        public override ManagementAgent MA
        {
            get { return _MA; }
        }
        public ManagementAgent MA_Set
        {
            set { _MA = value; }
        }

        public override ValueCollection ObjectClass
        {
            get;
            set;
        }

        private string _ObjectType;
        public override string ObjectType
        {
            get { return _ObjectType; }
        }
        public string ObjectType_Set
        {
            set { _ObjectType = value; }
        }

        public override string RDN
        {
            get; set;
        }

        public override string ToString()
        {
            return "CSEntry: RDN=" + RDN;
        }

        private IDictionary<string,Attrib> _attributes = new Dictionary<string, Attrib>(20);
        public IDictionary<string,Attrib> Attributes
        {
            get
            {
                return _attributes;
            }
        }

        public override Attrib this[string attributeName]
        {
            get { return _attributes[attributeName]; }
        }

    }
}
