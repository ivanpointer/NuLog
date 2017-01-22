/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using NuLog.Layouts;
using NuLog.Targets;
using System.Collections.Generic;

namespace NuLog.Activators.Layouts
{
    /// <summary>
    /// The activator for the standard layout.
    /// </summary>
    public class StandardLayoutActivator : LogFactoryActivatorBase<ILayout, IEnumerable<LayoutParameter>>
    {
        private readonly IPropertyParser propertyParser;

        public StandardLayoutActivator(IPropertyParser propertyParser)
        {
            this.propertyParser = propertyParser;
        }

        public override ILayout BuildNew(IEnumerable<LayoutParameter> config)
        {
            return new StandardLayout(config, propertyParser);
        }
    }
}