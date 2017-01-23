/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Loggers;
using System.Collections.Generic;

namespace NuLog
{
    /// <summary>
    /// Defines the expected behavior of a logger factory. The logger factory is responsible for providing
    /// instances of the various parts of the NuLog system.
    /// </summary>
    public interface ILoggerFactory
    {
        /// <summary>
        /// Gets a logger.
        /// </summary>
        ILogger GetLogger(IMetaDataProvider metaDataProvider, IEnumerable<string> defaultTags);
    }
}