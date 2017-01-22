/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NuLog.Layouts
{
    /// <summary>
    /// The standard implementation of a property parser.
    /// </summary>
    public class StandardPropertyParser : IPropertyParser
    {
        private readonly IDictionary<Type, PropertyInfo[]> typeCache;

        public StandardPropertyParser()
        {
            this.typeCache = new Dictionary<Type, PropertyInfo[]>();
        }

        public object GetProperty(object zobject, string path)
        {
            // Make sure the path has something
            if (string.IsNullOrWhiteSpace(path))
            {
                return null;
            }

            // Split out our property chain and pass off
            var propertyChain = path.Split('.');
            return GetProperty(zobject, propertyChain);
        }

        private object GetProperty(object zobject, ICollection<string> propertyChain)
        {
            object property = null;

            if (propertyChain != null && propertyChain.Count > 0)
            {
                // Recurse the object, looking for the property
                property = property == null
                    ? GetPropertyRecurse(zobject, propertyChain)
                    : property;
            }

            return property;
        }

        /// <summary>
        /// The internal recursive function for searching for an object by name. This is where the
        /// "heavy lifting" of searching for a property within the log event is performed.
        ///
        /// The complexity (cyclomatic) of this one is going to be high, which may not be avoidable,
        /// as this is no-kidding recursion.
        /// </summary>
        private object GetPropertyRecurse(object zobject, ICollection<string> propertyChain, int depth = 0)
        {
            // Exit condition
            if (zobject == null || depth >= propertyChain.Count)
            {
                // Either we have hit a dead-end (null) Or we have reached the end of the name chain
                // (depth) and we have our value
                return zobject;
            }
            else
            {
                // Determine if the object is a dictionary, otherwise treat it as just an object
                if (typeof(IDictionary<string, object>).IsAssignableFrom(zobject.GetType()) == false)
                {
                    // Try to get the element in the dictionary with the next name
                    var propertyList = GetPropertyInfo(zobject.GetType());
                    var propertyInfo = propertyList.Where(_ => _.Name == propertyChain.ElementAt(depth)).FirstOrDefault();
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
                    var key = propertyChain.ElementAt(depth);
                    if (dictionary.ContainsKey(key))
                    {
                        return GetPropertyRecurse(dictionary[key], propertyChain, depth + 1);
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
        private PropertyInfo[] GetPropertyInfo(Type objectType)
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
                    typeCache[objectType] = propertyInfo;
                    return propertyInfo;
                }
            }
        }
    }
}