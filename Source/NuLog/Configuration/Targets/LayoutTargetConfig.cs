/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/5/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using Newtonsoft.Json.Linq;
using NuLog.Configuration.Layouts;

namespace NuLog.Configuration.Targets
{
    /// <summary>
    /// The configuration that represents a layout target config
    /// </summary>
    public class LayoutTargetConfig : TargetConfig
    {
        /// <summary>
        /// The layout configuration for this target config
        /// </summary>
        public LayoutConfig LayoutConfig { get; set; }

        /// <summary>
        /// Creates a standard layout config
        /// </summary>
        /// <param name="synchronous">The synchronous flag, used to override this target to always log synchronously</param>
        public LayoutTargetConfig(bool synchronous = false)
            : base(synchronous)
        {
            LayoutConfig = new LayoutConfig();
        }

        /// <summary>
        /// Creates a layout config using the provided JSON token
        /// </summary>
        /// <param name="jToken">The JSON token to use to build this layout config</param>
        /// <param name="synchronous">The synchronous flag, used to override this target to always log synchronously</param>
        public LayoutTargetConfig(JToken jToken, bool synchronous = false)
            : base(jToken, synchronous)
        {
            LayoutConfig = new LayoutConfig();

            if (jToken != null)
                LayoutConfig = new LayoutConfig(jToken);
        }
    }
}