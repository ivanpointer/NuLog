using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.MetaData
{
    public class MetaDataParser
    {
        private IDictionary<Type, PropertyInfo[]> TypeCache { get; set; }

        #region Singleton Stuff
        private static readonly Lazy<MetaDataParser> _instance = new Lazy<MetaDataParser>(() => { return new MetaDataParser(); });
        private static MetaDataParser Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private MetaDataParser()
        {
            TypeCache = new Dictionary<Type, PropertyInfo[]>();
        }
        #endregion

        public static object GetProperty(LogEvent logEvent, string propertyName)
        {
            object property = null;
            if (String.IsNullOrEmpty(propertyName) == false)
            {
                var nameList = propertyName.Split(',');
                return GetProperty(logEvent, nameList);
            }

            return property;
        }

        public static object GetProperty(LogEvent logEvent, ICollection<string> nameList)
        {
            object property = null;

            if (nameList != null && nameList.Count > 0)
            {
                property = GetPropertyRecurse(logEvent.MetaData, nameList);
                property = property == null
                    ? GetPropertyRecurse(logEvent, nameList)
                    : property;
            }

            return property;
        }


        private static object GetPropertyRecurse(object obj, ICollection<string> nameList, int depth = 0)
        {
            if (obj != null && depth < nameList.Count)
            {
                if (typeof(IDictionary<string, object>).IsAssignableFrom(obj.GetType()) == false)
                {
                    var propertyList = Instance.GetPropertyInfo(obj.GetType());
                    var propertyInfo = propertyList.Where(_ => _.Name == nameList.ElementAt(depth)).FirstOrDefault();
                    if (propertyInfo != null)
                    {
                        return GetPropertyRecurse(propertyInfo.GetValue(obj, null), nameList, depth + 1);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    var dictionary = (IDictionary<string, object>)obj;
                    var key = nameList.ElementAt(depth);
                    if (dictionary.ContainsKey(key))
                    {
                        return GetPropertyRecurse(dictionary[key], nameList, depth + 1);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                return obj;
            }
        }

        #region Helpers
        private PropertyInfo[] GetPropertyInfo(Type objectType)
        {
            if (!TypeCache.ContainsKey(objectType))
            {
                var propertyInfo = objectType.GetProperties();
                TypeCache[objectType] = propertyInfo;
                return propertyInfo;
            }
            else
            {
                return TypeCache[objectType];
            }
        }
        #endregion
    }
}
