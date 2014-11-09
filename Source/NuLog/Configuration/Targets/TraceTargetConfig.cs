/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 11/08/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using NuLog.Configuration.Layouts;
using NuLog.Targets;

namespace NuLog.Configuration.Targets
{
    /// <summary>
    /// A bit of a "helper" class, making creating a configuration for the trace target a bit easier
    /// at runtime
    /// </summary>
    public class TraceTargetConfig : LayoutTargetConfig
    {
        public const string DefaultName = "trace";
        public static readonly string TraceTargetType = typeof(TraceTarget).FullName;

        /// <summary>
        /// Default constructor
        /// </summary>
        public TraceTargetConfig() : base()
        {
            Name = DefaultName;
            Type = TraceTargetType;
        }

        /// <summary>
        /// Constructor allowing to specify the name of the target
        /// </summary>
        /// <param name="name">The name of the target</param>
        public TraceTargetConfig(string name) : base()
        {
            Name = name;
            Type = TraceTargetType;
        }

        /// <summary>
        /// Constructor allowing to specify the name of the target, and a layout format for the standard layout
        /// </summary>
        /// <param name="name">The name of the target</param>
        /// <param name="layout">The layout format for the standard layout</param>
        public TraceTargetConfig(string name, string layout) : base()
        {
            Name = name;
            LayoutConfig = new LayoutConfig(layout);
            Type = TraceTargetType;
        }

        /// <summary>
        /// Constructor allowing to specify the name and a pre-built layout config
        /// </summary>
        /// <param name="name">The name of the target</param>
        /// <param name="layoutConfig">The layout config</param>
        public TraceTargetConfig(string name, LayoutConfig layoutConfig) : base()
        {
            Name = name;
            LayoutConfig = layoutConfig;
            Type = TraceTargetType;
        }

        /// <summary>
        /// Constructor allowing to specify a pre-built layout config
        /// </summary>
        /// <param name="layoutConfig">The layout config</param>
        public TraceTargetConfig(LayoutConfig layoutConfig) : base()
        {
            Name = DefaultName;
            LayoutConfig = layoutConfig;
            Type = TraceTargetType;
        }
    }
}
