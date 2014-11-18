/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/7/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using System.Collections.Generic;

namespace NuLog.MetaData
{
    /// <summary>
    /// Defines the expected behavior of a meta data provider
    /// </summary>
    public interface IMetaDataProvider
    {
        /// <summary>
        /// Provides meta data
        /// </summary>
        /// <returns>Meta data</returns>
        IDictionary<string, object> ProvideMetaData();
    }
}
