/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;
using System.ComponentModel;
using System.Globalization;

namespace NuLog.Configuration.StandardConfiguration
{
    /// <summary>
    /// A type name converter specific to NuLog.
    /// </summary>
    public class NuLogTypeNameConverter : TypeConverter
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
                var stringVal = (string)value;
                if (stringVal.StartsWith("NuLog.Targets.") && !stringVal.EndsWith(", NuLog.Targets"))
                {
                    stringVal = stringVal + ", NuLog.Targets";
                }
                return Type.GetType(stringVal);
            }
            return base.ConvertFrom(context, culture, value);
        }

        // Overrides the ConvertTo method of TypeConverter.
        public override object ConvertTo(ITypeDescriptorContext context,
           CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == stringType)
            {
                var type = (Type)value;
                return string.Format("{0}, {1}", type.FullName, type.Assembly.GetName().Name);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}