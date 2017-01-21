/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.Collections.Generic;

namespace NuLog
{
    /// <summary>
    /// Defines the expected behavior of a meta data provider - namely, providing meta data.
    /// </summary>
    public interface IMetaDataProvider
    {
        /// <summary>
        /// Provide meta data for a log event.
        /// </summary>
        IDictionary<string, object> ProvideMetaData();
    }
}