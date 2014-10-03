using NuLog.Configuration.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Targets.Layouts
{
    public class LayoutFactory
    {
        public static ILayout BuildLayout(LayoutConfig layoutConfig)
        {
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
