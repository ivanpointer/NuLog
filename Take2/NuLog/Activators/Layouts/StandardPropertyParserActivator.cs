/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Layouts;

namespace NuLog.Activators.Layouts
{
    /// <summary>
    /// The activator for the standard property parser.
    /// </summary>
    public class StandardPropertyParserActivator : LogFactoryActivatorBase<IPropertyParser>
    {
        public override IPropertyParser BuildNew()
        {
            return new StandardPropertyParser();
        }
    }
}