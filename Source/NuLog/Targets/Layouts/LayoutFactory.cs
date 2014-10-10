/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/8/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */
using NuLog.Configuration.Layouts;
using System;
using System.Reflection;

namespace NuLog.Targets.Layouts
{
    /// <summary>
    /// Responsible for building layouts based on their config
    /// </summary>
    public class LayoutFactory
    {
        /// <summary>
        /// Builds a concrete instance of a layout using the given layout config
        /// </summary>
        /// <param name="layoutConfig"></param>
        /// <returns></returns>
        public static ILayout BuildLayout(LayoutConfig layoutConfig)
        {
            // Default to a "StandardLayout".  Check for a provided layout type and build it using reflection
            if (layoutConfig != null)
            {
                if (String.IsNullOrEmpty(layoutConfig.Type) || layoutConfig.Type == typeof(StandardLayout).FullName)
                {
                    return new StandardLayout(layoutConfig);
                }
                else
                {
                    Type layoutType = Type.GetType(layoutConfig.Type);
                    ConstructorInfo constructor = layoutType.GetConstructor(new Type[] { });
                    ILayout newLayout = (ILayout)constructor.Invoke(null);
                    newLayout.Initialize(layoutConfig);

                    return newLayout;
                }
            }
            else
            {
                return new StandardLayout();
            }
        }
    }
}
