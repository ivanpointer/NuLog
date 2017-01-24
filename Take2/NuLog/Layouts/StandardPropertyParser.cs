/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;
using System.Collections.Generic;
using System.Reflection;

namespace NuLog.Layouts
{
    /// <summary>
    /// The standard implementation of a property parser.
    /// </summary>
    public class StandardPropertyParser : IPropertyParser
    {
        private static readonly Type DictionaryType = typeof(IDictionary<string, object>);

        private readonly IDictionary<Type, IDictionary<string, PropertyInfo>> typeCache;

        public StandardPropertyParser()
        {
            this.typeCache = new Dictionary<Type, IDictionary<string, PropertyInfo>>();
        }

        public object GetProperty(object zobject, string[] path)
        {
            // Check for a null path
            if(path == null)
            {
                return null;
            }

            // Recurse it
            return GetPropertyRecurse(zobject, path);
        }

        /// <summary>
        /// The internal recursive function for searching for an object by name. This is where the
        /// "heavy lifting" of searching for a property within the log event is performed.
        ///
        /// The complexity (cyclomatic) of this one is going to be high, which may not be avoidable,
        /// as this is no-kidding recursion.
        /// </summary>
        private object GetPropertyRecurse(object zobject, string[] propertyChain, int depth = 0)
        {
            // Exit condition
            if (zobject == null || depth >= propertyChain.Length)
            {
                // Either we have hit a dead-end (null) Or we have reached the end of the name chain
                // (depth) and we have our value
                return zobject;
            }
            else
            {
                var zobjectType = zobject.GetType();
                var chainItem = propertyChain[depth];

                // Determine if the object is a dictionary, otherwise treat it as just an object
                if (DictionaryType.IsAssignableFrom(zobjectType) == false)
                {
                    // Try to get the element in the dictionary with the next name
                    var propertyDict = GetPropertyInfo(zobjectType);
                    var propertyInfo = propertyDict.ContainsKey(chainItem)
                        ? propertyDict[chainItem]
                        : null;
                    if (propertyInfo != null)
                    {
                        return GetPropertyRecurse(propertyInfo.GetValue(zobject, null), propertyChain, depth + 1);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    // Try to get the property of the object with the next name
                    var dictionary = (IDictionary<string, object>)zobject;
                    if (dictionary.ContainsKey(chainItem))
                    {
                        return GetPropertyRecurse(dictionary[chainItem], propertyChain, depth + 1);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// A property for retrieving the PropertyInfo of an object type Uses caching because the
        /// work of getting th properties of an object type is expensive.
        /// </summary>
        private IDictionary<string, PropertyInfo> GetPropertyInfo(Type objectType)
        {
            // Check the cache to see if we already have property info for the type\ Otherwise, get
            // and cache the property info for the type

            lock (typeCache)
            {
                if (typeCache.ContainsKey(objectType))
                {
                    return typeCache[objectType];
                }
                else
                {
                    var propertyInfo = objectType.GetProperties();
                    var dict = new Dictionary<string, PropertyInfo>();
                    foreach (var property in propertyInfo)
                    {
                        dict[property.Name] = property;
                    }
                    typeCache[objectType] = dict;
                    return dict;
                }
            }
        }
    }
}