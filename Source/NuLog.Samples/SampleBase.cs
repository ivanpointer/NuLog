/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/28/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using System;

namespace NuLog.Samples
{
    /// <summary>
    /// Defines the base behavior for a sample.  This is defined so that the samples can be self-contained
    /// and executed in a menu.
    /// </summary>
    public abstract class SampleBase
    {
        #region Constants

        private const string PauseMessage = "\r\nPress [Enter] to continue...";

        #endregion Constants

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
        public abstract void ExecuteSample();

        /// <summary>
        /// Pauses the sample, prompting the user to press [Enter] to continue
        /// </summary>
        protected void PauseSample()
        {
            Console.WriteLine(PauseMessage);
            Console.ReadLine();
            Console.Clear();
        }
    }
}