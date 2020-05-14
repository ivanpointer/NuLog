/* © 2020 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Configuration;

namespace NuLog.Targets {

    /// <summary>
    /// A base class for making building layout-based targets simpler.
    /// </summary>
    public abstract class LayoutTargetBase : TargetBase, ILayoutTarget {

        /// <summary>
        /// The layout assigned to this layout target.
        /// </summary>
        protected ILayout Layout { get; set; }

        public void Configure(TargetConfig config, ILayoutFactory layoutFactory) {
            var format = config != null && config.Properties != null && config.Properties.ContainsKey("layout")
                ? (string)config.Properties["layout"]
                : null;
            var layout = layoutFactory.MakeLayout(format);

            this.Layout = layout;
        }
    }
}