/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/8/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NuLog.MetaData
{
    public class MetaDataParser
    {
        #region Members and Constructors

        private IDictionary<Type, PropertyInfo[]> TypeCache { get; set; }
        
        // Singleton
        private static readonly Lazy<MetaDataParser> _instance = new Lazy<MetaDataParser>(() => { return new MetaDataParser(); });
        private static MetaDataParser Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        // Private constructor for singleton
        private MetaDataParser()
        {
            TypeCache = new Dictionary<Type, PropertyInfo[]>();
        }

        #endregion

        #region Meta Data Parsing (Property Finding)

        /// <summary>
        /// Finds and returns a property within the given log event by the property name.  The meta data is
        /// checked first, then the other members of the log event.  See the documentation at
        /// https://github.com/ivanpointer/NuLog for more information on how the property names are parsed.
        /// </summary>
        /// <param name="logEvent">The log event to search</param>
        /// <param name="propertyName">The name of the property to search for</param>
        /// <returns>The property by the name found in the log event, or null if none found</returns>
        public static object GetProperty(LogEvent logEvent, string propertyName)
        {
            object property = null;
            if (String.IsNullOrEmpty(propertyName) == false)
            {
                var nameList = propertyName.Split('.');
                return GetProperty(logEvent, nameList);
            }

            return property;
        }

        /// <summary>
        /// Finds and returns a property within the given log event by the property name list.  The meta data
        /// is searched first, then the other members of the log event.  See the documentation at
        /// https://github.com/ivanpointer/NuLog for more information on how the property names are parsed.
        /// </summary>
        /// <param name="nameList">The name of the property to search for, in list format</param>
        /// <returns>The property by the name found in the log event, or null if none found</returns>
        public static object GetProperty(LogEvent logEvent, ICollection<string> nameList)
        {
            object property = null;

            if (nameList != null && nameList.Count > 0)
            {
                // Search the meta data first
                property = GetPropertyRecurse(logEvent.MetaData, nameList);

                // Search the rest of the log event second
                property = property == null
                    ? GetPropertyRecurse(logEvent, nameList)
                    : property;
            }

            return property;
        }

        // The internal recursive function for searching for an object by name.
        //  This is where the "heavy lifting" of searching for a propert within the log event
        //   is performed.
        private static object GetPropertyRecurse(object obj, ICollection<string> nameList, int depth = 0)
        {
            // Exit condition
            if (obj != null && depth < nameList.Count)
            {
                // Determine if the object is a dictionary, otherwise treat it as just an object
                if (typeof(IDictionary<string, object>).IsAssignableFrom(obj.GetType()) == false)
                {
                    // Try to get the element in the dictionary with the next name
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
                    // Try to get the property of the obect with the next name
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
                // Either we have hit a dead-end (null)
                //  Or we have reached the end of the name chain (depth) and we have our value
                return obj;
            }
        }

        #endregion

        #region Helpers

        // A property for retreiving the PropertyInfo of an object type
        //  Uses caching because the work of getting th properties of an object type
        //   is expensive.
        private PropertyInfo[] GetPropertyInfo(Type objectType)
        {
            // Check the cache to see if we already have property info for the type\
            //  Otherwise, get and cache the property info for the type

            lock (TypeCache)
            {
                if (TypeCache.ContainsKey(objectType))
                {
                    return TypeCache[objectType];
                }
                else
                {
                    var propertyInfo = objectType.GetProperties();
                    TypeCache[objectType] = propertyInfo;
                    return propertyInfo;
                }
            }
        }

        #endregion
    }
}
