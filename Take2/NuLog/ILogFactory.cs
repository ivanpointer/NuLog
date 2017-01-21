/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using System.Collections.Generic;

namespace NuLog
{
    /// <summary>
    /// Defines the expected behavior of a log factory. The log factory is responsible for providing
    /// instances of the various parts of the NuLog system.
    /// </summary>
    public interface ILogFactory
    {
        ICollection<ITarget> GetTargets(IEnumerable<TargetConfig> configTargets);
    }
}