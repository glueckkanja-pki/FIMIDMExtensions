using Microsoft.MetadirectoryServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mms_ManagementAgent_GKFIMIDMExtensions
{
    /// <summary>
    /// Calculates some value from the values of an MV object
    /// </summary>
    public interface IValueCalculator
    {
        string calculateValue(IFIMDirectoryObject entry);
    }
}
