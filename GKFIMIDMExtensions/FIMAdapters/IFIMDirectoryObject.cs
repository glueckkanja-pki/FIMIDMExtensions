using Microsoft.MetadirectoryServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mms_ManagementAgent_GKFIMIDMExtensions
{
    /// <summary>
    /// Represents either a CSEntry or a MVEntry, both of which have attributes
    /// </summary>
    public interface IFIMDirectoryObject
    {
        Attrib this[string attributeName] { get; }

        string objectType { get;  }
    }
}
