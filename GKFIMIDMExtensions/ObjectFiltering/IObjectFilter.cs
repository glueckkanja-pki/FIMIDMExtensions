using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mms_ManagementAgent_GKFIMIDMExtensions.ObjectFiltering
{
    /// <summary>
    /// A Filter that may or may not apply to any given IFIMDirectoryObject (CSEntry/MVEntry)
    /// </summary>
    public interface IObjectFilter
    {
        /// <summary>
        /// Tests the fimObject for a filter criterion. If it applies, the method returns true and if not, the method returns false.
        /// </summary>
        bool doesApply(IFIMDirectoryObject fimObject);
    }
}
