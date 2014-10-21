using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Samples
{
    /// <summary>
    /// Defines the base behavior for a sample.  This is defined so that the samples can be self-contained
    /// and executed in a menu.
    /// </summary>
    public abstract class SampleBase
    {
        /// <summary>
        /// Creates a sample instance in the given section with the given sample name
        /// </summary>
        /// <param name="section">The section this sample is in</param>
        /// <param name="sampleName">The name of this sample</param>
        public SampleBase(string section, string sampleName)
        {
            SectionName = section;
            SampleName = sampleName;
        }

        /// <summary>
        /// The name of the section that this sample belongs to
        /// </summary>
        public string SectionName { get; set; }

        /// <summary>
        /// The sample number and display name of the sample for the menu
        /// </summary>
        public string SampleName { get; set; }

        /// <summary>
        /// Executes the sample
        /// </summary>
        /// <param name="args">Optional arguments for the sample, the intent is that these are the arguments passed in on the command line</param>
        public abstract void ExecuteSample(Arguments args);

        /// <summary>
        /// Searches the given arguments for the argument by name argumentName.
        /// If the argument is not found in args, and request is true, this will
        /// request from the user the value of the argument, defaulting to defaultValue.
        /// If the argumenti s not found in args, and request is false, this will
        /// return the defaultValue
        /// </summary>
        /// <typeparam name="T">The type to cast the return value to</typeparam>
        /// <param name="args">The arguments to search for the argument</param>
        /// <param name="argumentName">The name of the argument</param>
        /// <param name="defaultValue">The default value for the argument</param>
        /// <param name="displayText">The text to display to the end user when requesting the value</param>
        /// <param name="request">A flag signaling whether or not to request the value from the end user if it is not probvided</param>
        /// <returns>A tuple containing a bool, indicating if a value was retrieved, and the value of the argument, casted to the type of this call</returns>
        protected Tuple<bool, T> GetArgument<T>(Arguments args, string argumentName, T defaultValue = default(T), string displayText = null, bool request = false)
        {
            Type tType = typeof(T);

            if (args.ContainsKey(argumentName))
            {
                try
                {
                    return new Tuple<bool, T>(true, (T)Convert.ChangeType(args[argumentName], tType));
                }
                catch
                {
                    Trace.WriteLine(String.Format("Failed to convert \"{0}\" to type \"{1}\"", args[argumentName], tType.FullName));
                }
            }
            else if (request)
            {
                string question = String.Format("{0}{1}: "
                    , String.IsNullOrEmpty(displayText)
                        ? argumentName
                        : displayText
                    , String.IsNullOrEmpty(Convert.ToString(defaultValue))
                        ? String.Empty
                        : Convert.ToString(defaultValue));

                Console.Write(question);

                string answer = Console.ReadLine();

                if(String.IsNullOrEmpty(answer) == false)
                {
                    try
                    {
                        return new Tuple<bool, T>(true, (T)Convert.ChangeType(answer, tType));
                    }
                    catch
                    {
                        Trace.WriteLine(String.Format("Failed to convert \"{0}\" to type \"{1}\"", answer, tType.FullName));
                    }
                }
            }

            return new Tuple<bool, T>(false, defaultValue);
        }
    }
}
