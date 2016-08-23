using Microsoft.MetadirectoryServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mms_ManagementAgent_GKFIMIDMExtensions.FIMAdapters
{
    public class CSEntryAdapter : IFIMDirectoryObject
    {
        public CSEntry WrappedEntry { get; set; }

        public CSEntryAdapter(CSEntry wrappedEntry)
        {
            this.WrappedEntry = wrappedEntry;
        }

        public Microsoft.MetadirectoryServices.Attrib this[string attributeName]
        {
            get { return WrappedEntry[attributeName]; }
        }

        public string objectType
        {
            get { return WrappedEntry.ObjectType; }
        }
    }
}
