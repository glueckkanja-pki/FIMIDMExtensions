using Microsoft.MetadirectoryServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKFIMIDMExtensionsTest.FIMIDMDummies
{
    class MVEntryDummy : MVEntry
    {
        public override ConnectedMACollection ConnectedMAs
        {
            get { throw new NotImplementedException(); }
        }

        public override AttributeNameEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override Guid ObjectID
        {
            get { throw new NotImplementedException(); }
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

        public override string ToString()
        {
            return "MVEntry";
        }

        private IDictionary<string, Attrib> _attributes = new Dictionary<string, Attrib>(20);
        public IDictionary<string, Attrib> Attributes
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
