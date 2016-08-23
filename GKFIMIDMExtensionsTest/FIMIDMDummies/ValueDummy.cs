using Microsoft.MetadirectoryServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKFIMIDMExtensionsTest.FIMIDMDummies
{
    class ValueDummy : Value
    {

        private AttributeType _DataType;
        public override AttributeType DataType
        {
            get { return _DataType; }
        }
        public AttributeType DataType_Set
        {
            set { _DataType = value; }
        }

        public object CurrentValue
        { get; set; }

        public ValueDummy(object newValue, AttributeType attrType)
        {
            CurrentValue = newValue;
            _DataType = attrType;
        }

        public ValueDummy(long newValue)
            : this(newValue,AttributeType.Integer)
        {
        }

        public ValueDummy(string newValue)
            : this(newValue, AttributeType.String)
        {
        }

        public ValueDummy(bool newValue)
            : this(newValue, AttributeType.Boolean)
        {
        }

        public override bool Equals(object obj)
        {
            return CurrentValue.Equals(obj);
        }

        public override int GetHashCode()
        {
            return CurrentValue.GetHashCode();
        }

        public override byte[] ToBinary()
        {
            throw new NotImplementedException();
        }

        public override bool ToBoolean()
        {
            return Convert.ToBoolean(CurrentValue);
        }

        public override long ToInteger()
        {
            return Convert.ToInt32(CurrentValue);
        }

        public override string ToString()
        {
            return CurrentValue.ToString();
        }
    }
}
