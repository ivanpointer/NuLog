/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;

namespace NuLog.Targets
{
    /// <summary>
    /// Defines the expected behavior of a layout target.
    /// </summary>
    public interface ILayoutTarget : ITarget
    {
        /// <summary>
        /// Configures the layout for this target, based on the given target config.
        /// </summary>
        void Configure(TargetConfig config, ILayoutFactory layoutFactory);
    }
}