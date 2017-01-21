/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;
using System;
using System.Collections.Generic;

namespace NuLog.Factories
{
    /// <summary>
    /// The standard implementation of the log factory.
    /// </summary>
    public class StandardLogFactory : ILogFactory
    {
        public ICollection<ITarget> GetTargets(IEnumerable<TargetConfig> configTargets)
        {
            throw new NotImplementedException();
        }
    }
}