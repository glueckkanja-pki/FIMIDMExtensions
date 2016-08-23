using Microsoft.MetadirectoryServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKFIMIDMExtensionsTest.FIMIDMDummies
{
    public class AttribDummy : Attrib
    {
        public override byte[] BinaryValue
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool BooleanValue
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override AttributeType DataType
        {
            get { throw new NotImplementedException(); }
        }

        public override void Delete()
        {
            throw new NotImplementedException();
        }

        public override long IntegerValue
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        private bool _IsMultivalued = false;
        public override bool IsMultivalued
        {
            get { return _IsMultivalued; }
        }
        public bool IsMultivalued_Set
        {
            set { _IsMultivalued = value; }
        }


        public override bool IsPresent
        {
            get { return true; }
        }

        public override ManagementAgent LastContributingMA
        {
            get { throw new NotImplementedException(); }
        }

        public override DateTime LastContributionTime
        {
            get { throw new NotImplementedException(); }
        }

        private string _Name;
        public override string Name
        {
            get { return _Name; }
        }
        public string Name_Set
        {
            set { _Name = value; }
        }

        public override ReferenceValue ReferenceValue
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override string StringValue
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override string Value
        {
            get;
            set;
        }

        public override ValueCollection Values
        {
            get;
            set;
        }

        public AttribDummy(string name, string value)
        {
            Name_Set = name;
            Value = value;
        }
    }
}
