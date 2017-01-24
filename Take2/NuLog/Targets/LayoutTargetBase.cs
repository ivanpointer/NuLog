/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

namespace NuLog.Targets
{
    /// <summary>
    /// A base class for making building layout-based targets simpler.
    /// </summary>
    public abstract class LayoutTargetBase : TargetBase
    {
        /// <summary>
        /// The layout assigned to this layout target.
        /// </summary>
        protected ILayout Layout { get; set; }

        /// <summary>
        /// Sets the given layout as the layout for this layout target.
        /// </summary>
        public void SetLayout(ILayout layout)
        {
            Layout = layout;
        }
    }
}