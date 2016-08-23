using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mms_ManagementAgent_GKFIMIDMExtensions.MultiValueFiltering
{
    /// <summary>
    /// Used to select or transform a set of values in a multi-valued attribute to some other format
    /// </summary>
    public interface IMultiValueFilter
    {
        IEnumerable<string> filterValues(IEnumerable<string> sourceValues);
    }
}
