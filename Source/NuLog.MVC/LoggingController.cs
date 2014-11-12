/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/12/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using NuLog.MetaData;
using System.Collections.Generic;
using System.Web.Mvc;

namespace NuLog.MVC
{
    /// <summary>
    /// An implementation of the MVC controller which includes a logger
    ///  and is a meta data provider.  This automatically includes request
    ///  information in log events so that logging can be done on individual
    ///  request information through the meta data.
    /// </summary>
    public abstract class LoggingController : Controller, IMetaDataProvider
    {
        /// <summary>
        /// Key for the HTTP request in the meta data
        /// </summary>
        public const string HttpRequestMeta = "HttpRequest";
        /// <summary>
        /// Key for the HTTP session in the meta data
        /// </summary>
        public const string HttpSessionMeta = "HttpSession";

        /// <summary>
        /// The logger for this controller
        /// </summary>
        protected LoggerBase Logger { get; private set; }

        /// <summary>
        /// Default constructor, initializes the logger
        /// </summary>
        public LoggingController() : base()
        {
            Logger = LoggerFactory.GetLogger(this);
        }

        /// <summary>
        /// Provides HTTP request and session information as meta data for a log event
        /// </summary>
        /// <returns>HTTP request and session information as meta data</returns>
        public virtual IDictionary<string, object> ProvideMetaData()
        {
            var metaData = new Dictionary<string, object>();

            metaData[HttpRequestMeta] = (object) Request;
            metaData[HttpSessionMeta] = (object) Session;

            return metaData;
        }
    }
}
