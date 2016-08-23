using Microsoft.MetadirectoryServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKFIMIDMExtensionsTest.FIMIDMDummies
{
    class ValueCollectionDummy : ValueCollection
    {
        private LinkedList<Value> values = new LinkedList<Value>();

        public override void Add(ValueCollection val)
        {
            foreach (Value specificVal in val)
                Add(specificVal);
        }

        public override void Add(Value val)
        {
            values.AddLast(val);
        }

        public override void Add(byte[] val)
        {
            throw new NotImplementedException();
            //values.AddLast(new ValueDummy(val));
        }

        public override void Add(long val)
        {
            values.AddLast(new ValueDummy(val));
        }

        public override void Add(string val)
        {
            values.AddLast(new ValueDummy(val));
        }

        public override void Clear()
        {
            values.Clear();
        }

        public override bool Contains(Value val)
        {
            return values.Contains(val);
        }

        public override bool Contains(byte[] val)
        {
            throw new NotImplementedException();
        }

        public override bool Contains(long val)
        {
            foreach (Value currentVal in values)
                if (currentVal.Equals(val))
                    return true;

            return false;
        }

        public override bool Contains(string val)
        {
            foreach (Value currentVal in values)
                if (currentVal.Equals(val))
                    return true;

            return false;
        }

        public override int Count
        {
            get { return values.Count; }
        }

        private class ValueCollectionEnumeratorDummy : ValueCollectionEnumerator
        {
            private IEnumerator<Value> backendEnumerator { get; set;  }

            public ValueCollectionEnumeratorDummy(IEnumerator<Value> backendEnumerator)
            {
                this.backendEnumerator = backendEnumerator;
            }

            public override Value Current
            {
                get { return backendEnumerator.Current; }
            }

            public override bool MoveNext()
            {
                return backendEnumerator.MoveNext();
            }

            public override void Reset()
            {
                backendEnumerator.Reset();
            }
        }

        public override ValueCollectionEnumerator GetEnumerator()
        {
            return new ValueCollectionEnumeratorDummy(values.GetEnumerator());
        }

        public override void Remove(Value val)
        {
            values.Remove(val);
        }

        public override void Remove(byte[] val)
        {
            throw new NotImplementedException();
        }

        public override void Remove(long val)
        {
            Remove(new ValueDummy(val));
        }

        public override void Remove(string val)
        {
            Remove(new ValueDummy(val));
        }

        public override void Set(bool val)
        {
            throw new NotImplementedException();
        }

        public override long[] ToIntegerArray()
        {
            return values.Cast<long>().ToArray();
        }

        public override string[] ToStringArray()
        {
            return values.Cast<string>().ToArray();
        }

        public override Value this[int index]
        {
            get { return values.ElementAt(index); }
        }
    }
}
