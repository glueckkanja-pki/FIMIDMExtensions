using Microsoft.MetadirectoryServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mms_ManagementAgent_GKFIMIDMExtensions.FIMAdapters
{
    /// <summary>
    /// Wraps an MVEntry to expose the standardizes IFIMDirectoryObject interface
    /// </summary>
    public class MVEntryAdapter : IFIMDirectoryObject
    {
        public MVEntry WrappedMVEntry { get; protected set;}

        public MVEntryAdapter(MVEntry wrappedMVentry)
        {
            this.WrappedMVEntry = wrappedMVentry;
        }

        public Attrib this[string attributeName]
        {
            get { return WrappedMVEntry[attributeName]; }
        }

        public string objectType
        {
            get { return WrappedMVEntry.ObjectType; }
        }
    }
}
