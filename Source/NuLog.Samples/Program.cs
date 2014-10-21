/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/20/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */
using NuLog.Samples.Samples.S1_1_HelloWorld;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NuLog.Samples
{
    /// <summary>
    /// A program providing an interface to the samples for the NuLog framework
    /// </summary>
    class Program
    {
        #region Structs

        // A simple struct to store the selection information from the end user
        //  Instead of using null on "SampleBase" to trigger an exit, we use
        //  this struct because the first solution would cause the application
        //  to exit in the event that the user mistypes the sample number they
        //  wish to execute.
        private struct SampleSelection
        {
            /// <summary>
            /// The sample selected by the user
            /// </summary>
            public SampleBase Sample;
            /// <summary>
            /// Whether or not the user has instead chosen to exit
            /// </summary>
            public bool DoExit;
        }

        #endregion

        #region Constants

        // Constants for printing and formatting the samples into their
        //  sections in the menu
        public const string Section1 = "  1. The Basics";
        public const string SampleIndent = "    ";

        #endregion

        #region Constructors and Members

        // The ordered list of samples in the menu
        private static readonly IList<SampleBase> SamplesMenu = new List<SampleBase>()
        {
            new HelloWorldSample(Section1, SampleIndent + "1.1 Hello World")
        };

        #endregion

        #region Main

        // Mainly main stuff ;)
        static void Main(string[] args)
        {
            // Parse out the arguments coming in from the command line
            var arguments = new Arguments(args);

            // Execute the samples the end user selects until the user indicates to exit
            SampleSelection selection;
            bool exit = false;
            while (!exit)
            {
                // Get the user's selection and execute it if we got one
                selection = GetNextSampleFromUser();
                if (selection.DoExit == false && selection.Sample != null)
                {
                    Console.Clear();
                    Console.WriteLine(String.Format("Executing sample \"{0}\":" + System.Environment.NewLine, selection.Sample.SampleName.Trim()));
                    selection.Sample.ExecuteSample(arguments);
                    Console.WriteLine(System.Environment.NewLine + "Press [Enter] to continue");
                    Console.ReadLine();
                }

                // Update the exit flag
                exit = selection.DoExit;
            }
        }

        #endregion

        #region Helpers

        // Prints out the samples menu, but first clears the console
        private static void PrintSamplesMenu(bool clear = true)
        {
            // Clear the console
            if(clear)
                Console.Clear();
            Console.WriteLine(String.Empty);

            // Print the samples, adding in the section headers for each of the sections
            string section = null;
            foreach (var sample in SamplesMenu)
            {
                if (String.Equals(section, sample.SectionName) == false)
                {
                    section = sample.SectionName;
                    Console.WriteLine(section);
                }
                Console.WriteLine(sample.SampleName);
            }

            // Some fluff
            Console.WriteLine(String.Empty);
        }

        // Prints out the samples menu and challenges the user for the sample they want to execute
        private static SampleSelection GetNextSampleFromUser()
        {
            // Print the menu menu
            Console.WriteLine("Which sample would you like to execute?");
            PrintSamplesMenu();

            // Challenge the user for a sample number
            Console.Write("Enter a sample number, or [Enter] to exit: ");
            var userSelection = Console.ReadLine();

            // Try to figure out which sample the user wanted by matching
            //  what the user entered to the beginning of the sample name,
            //  or exit if the user indicated they wish to exit by simply
            //  pressing [Enter] (null or empty string)
            SampleBase selectedSample = null;
            bool doExit = false;
            if (String.IsNullOrEmpty(userSelection) == false)
            {
                userSelection = userSelection.Trim();
                selectedSample = SamplesMenu.FirstOrDefault(_ => _.SampleName.Trim().StartsWith(userSelection));

                if (selectedSample == null)
                    Console.WriteLine(String.Format("Sample for \"{0}\" not found", userSelection));
            }
            else
            {
                doExit = true;
            }

            // Return the results of the user selection.
            //  "DoExit" will only be true if the end user explicitly
            //   indicated their intent to exit by hitting [Enter] without
            //   typing anything else.
            //  "Sample" will only be populated if we found a sample matching
            //   what the user entered
            return new SampleSelection
            {
                Sample = selectedSample,
                DoExit = doExit
            };
        }

        #endregion

    }
}

