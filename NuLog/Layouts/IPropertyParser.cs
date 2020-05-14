/* © 2020 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

namespace NuLog.Layouts {

    /// <summary>
    /// Responsible for taking a path, and an object, and traversing the object to find the
    /// identified value.
    /// </summary>
    public interface IPropertyParser {

        /// <summary>
        /// Traverses/iterates the object, using the given path, to return the property at the
        /// identified location. If no property is found with the given path, null is returned. If
        /// the element in the chain is a dictionary with a string key, the path element is treated
        /// as a key to an item in the dictionary.
        /// </summary>
        /// <param name="zobject">The object to traverse, looking for the path.</param>
        /// <param name="path">   The path to the desired property.</param>
        /// <returns>The property at the given path within the object, or null if not found.</returns>
        object GetProperty(object zobject, string[] path);
    }
}