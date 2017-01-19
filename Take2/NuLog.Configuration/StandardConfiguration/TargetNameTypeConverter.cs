/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace NuLog.Configuration.StandardConfiguration
{
    /// <summary>
    /// Converts a list of target names (string) between string and string[].
    /// </summary>
    public class TargetNameTypeConverter : TypeConverter
    {
        private static readonly Type stringType = typeof(string);

        // Overrides the CanConvertFrom method of TypeConverter. The ITypeDescriptorContext interface
        // provides the context for the conversion. Typically, this interface is used at design time
        // to provide information about the design-time container.
        public override bool CanConvertFrom(ITypeDescriptorContext context,
           Type sourceType)
        {
            if (sourceType == stringType)
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        // Overrides the ConvertFrom method of TypeConverter.
        public override object ConvertFrom(ITypeDescriptorContext context,
           CultureInfo culture, object value)
        {
            if (value is string)
            {
                var targetsRaw = (string)(value ?? string.Empty);
                var targets = targetsRaw.Split(',').Select(s => s.Trim()).ToArray();
                return targets;
            }
            return base.ConvertFrom(context, culture, value);
        }

        // Overrides the ConvertTo method of TypeConverter.
        public override object ConvertTo(ITypeDescriptorContext context,
           CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == stringType)
            {
                return string.Join(",", (string[])value);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}