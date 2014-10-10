/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/8/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */
using NuLog.Configuration.Layouts;
using NuLog.Configuration.Targets;
using NuLog.Dispatch;
using NuLog.Targets.Layouts;

namespace NuLog.Targets
{
    /// <summary>
    /// An extgension of the target base, this provides faculties for logging log events using a layout to format the log event into text.
    /// Because many targets will want formatted log events, this base is provided to centralize an unify this effort.
    /// </summary>
    public abstract class LayoutTargetBase : TargetBase
    {
        public const string LayoutTokenName = "layout";

        private const string DefaultTargetName = "trace";

        private static readonly object _configLock = new object();
        public ILayout Layout { get; protected set; }

        /// <summary>
        /// Initializes the target as normal, except with a layout built-into the target
        /// </summary>
        /// <param name="targetConfig">The target config to build this target from</param>
        /// <param name="dispatcher">The dispatcher this target is attached to</param>
        /// <param name="synchronous">An override to the syncrhonous setting in the target config</param>
        public override void Initialize(TargetConfig targetConfig, LogEventDispatcher dispatcher = null, bool? synchronous = null)
        {
            // Initialize the base target as normal
            //  Add logic for initializing the layout as well
            base.Initialize(targetConfig, dispatcher, synchronous);
            if (targetConfig != null)
            {
                lock (_configLock)
                {
                    // Check to see if the target configuration passed in is already a layout target configuration and initialize
                    //  depending on this

                    LayoutConfig layoutConfig = null;
                    if (typeof(LayoutTargetConfig).IsAssignableFrom(targetConfig.GetType()))
                    {
                        layoutConfig = ((LayoutTargetConfig)targetConfig).LayoutConfig;
                    }
                    else
                    {
                        var layoutConfigToken = targetConfig.Config[LayoutTokenName];
                        layoutConfig = new LayoutConfig(layoutConfigToken);
                    }

                    Layout = LayoutFactory.BuildLayout(layoutConfig);
                }
            }
            else
            {
                Name = DefaultTargetName;
                Layout = new StandardLayout();
            }
        }
    }
}
