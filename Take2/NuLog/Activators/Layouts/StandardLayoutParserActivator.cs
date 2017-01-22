/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Layouts;

namespace NuLog.Activators.Layouts
{
    /// <summary>
    /// The activator for the standard layout parser.
    /// </summary>
    public class StandardLayoutParserActivator : LogFactoryActivatorBase<ILayoutParser>
    {
        public override ILayoutParser BuildNew()
        {
            return new StandardLayoutParser();
        }
    }
}