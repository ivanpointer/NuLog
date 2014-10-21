/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/18/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace NuLog.Samples
{
    /// <summary>
    /// An implementation of Dictionary<string, string> which provides a constructor for parsing
    /// command line arguments (string[] args) into the dictionary.
    /// 
    /// Arguments can be in the format /ArgumentName=ArgumentValue or /ArgumentName ArgumentValue
    /// 
    /// Arguments start with a forward slash, and their values can be set after an equal sign, or after
    /// a space.  If multiple argument values are listed after an argument name, they are concatenated
    /// together with spaces.  For example:
    /// 
    /// /myArgument val1 val2 val3
    /// 
    /// Will result in "myArgument" with a value of "val1 val2 val3".  It is important to note that all
    /// white space between the values will be stripped down/converted to a single space.
    /// </summary>
    public class Arguments : Dictionary<string, string>
    {
        #region Constants

        private const string ArgumentName = "ArgumentName";
        private const string ArgumentValue = "ArgumentValue";
        private const string ArgPattern = "/(?<" + ArgumentName + ">.*)=(?<" + ArgumentValue + ">.*)";
        private static readonly Regex ArgRegex = new Regex(ArgPattern);
        private const string ShortArgPattern = "/(?<" + ArgumentName + ">.*)";
        private static readonly Regex ShortArgRegex = new Regex(ShortArgPattern);

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs an empty dictionary of arguments
        /// </summary>
        public Arguments() : base() { }

        /// <summary>
        /// Constructs a dictionary of arguments from the array of args
        /// </summary>
        /// <param name="args">The string array of args.  This is intended to be the arguments passed on the command line.</param>
        public Arguments(string[] args) : base()
        {
            Match match;
            string cleanedArgument = null;
            string argumentName = null;
            string argumentValue = null;

            // Iterate over each of the arguments checking them for the expected format
            foreach (string arg in args)
            {
                cleanedArgument = arg.Trim();

                // First check for a long style argument; an argument where the argument name
                //  and argument value are separated by an equal sign.

                match = ArgRegex.Match(cleanedArgument);
                if (match.Success)
                {
                    argumentName = match.Groups[ArgumentName].Value;
                    argumentValue = match.Groups[ArgumentValue].Value;
                }
                else
                {
                    // If no long style argument was found, check for a short style
                    //  argument, which is an argument and its value separated by a
                    //  space (resulting in the argument name and value being in
                    //  different entries in the string arg array)

                    match = ShortArgRegex.Match(cleanedArgument);
                    if (match.Success)
                    {
                        argumentName = match.Groups[ArgumentName].Value;
                        argumentValue = null;
                    }
                    else
                    {
                        argumentValue = String.IsNullOrEmpty(argumentValue)
                            ? cleanedArgument
                            : argumentValue + " " + cleanedArgument;
                    }
                }

                // Make sure that an argument name has been specified yet
                if (String.IsNullOrEmpty(argumentName) == false)
                {
                    this[argumentName] = argumentValue;
                    Console.WriteLine(String.Format("[{0}] = [{1}]", argumentName, argumentValue));
                }
                else
                {
                    // TODO: Use NuLog for this
                    Trace.WriteLine(String.Format("Failed to parse argument value \"{0}\", no argument name found", argumentValue));
                }
                
            }
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Searches this set of arguments for a flag and determines whether or not the flag is set based on any associated value
        /// </summary>
        /// <param name="key">The key of the argument to evaluate as a flag</param>
        /// <returns>A bool indicating the status of the flag</returns>
        public bool Flag(string key)
        {
            if (ContainsKey(key))
            {
                // If we have the key, and it has a value, try to parse the boolean value of it
                string value = this[key];
                if (String.IsNullOrEmpty(value) == false)
                    return Convert.ToBoolean(value);

                // If we have the key, but no value for it, we assume the flag is true
                return true;
            }

            // If we don't have the key, we assume the flag is false
            return false;
        }

        #endregion

    }
}
