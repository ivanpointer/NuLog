/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/28/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using NuLog.Samples.CustomizeSamples.S1_2_ASimpleTarget;
using NuLog.Samples.CustomizeSamples.S1_3_MakingALayoutTarget;
using NuLog.Samples.CustomizeSamples.S2_1_AddingColor;
using NuLog.Samples.CustomizeSamples.S2_2_AddingConfiguration;
using NuLog.Samples.CustomizeSamples.S2_3_AddingMetaData;
using NuLog.Samples.CustomizeSamples.S2_4_ShuttingDownTheTarget;
using NuLog.Samples.CustomizeSamples.S2_5_AsynchronousLoggingInTheTarget;
using NuLog.Samples.CustomizeSamples.S3_1_ExtendingTheLogger;
using NuLog.Samples.CustomizeSamples.S4_2_RuntimeMetaDataProviders;
using NuLog.Samples.CustomizeSamples.S4_3_StaticMetaDataProviders;
using NuLog.Samples.CustomizeSamples.S5_1_ConfigurationExtenders;
using NuLog.Samples.CustomizeSamples.S6_1_CustomLayout;
using NuLog.Samples.CustomizeSamples.S7_1_CustomConfigurationBuilder;
using NuLog.Samples.CustomizeSamples.S8_1_CustomExtender;
using NuLog.Samples.Samples.S1_1_HelloWorld;
using NuLog.Samples.Samples.S1_2_TagsRules;
using NuLog.Samples.Samples.S1_3_TagGroups;
using NuLog.Samples.Samples.S1_4_MetaData;
using NuLog.Samples.Samples.S1_5_SynchronousLogging;
using NuLog.Samples.Samples.S1_8_RuntimeConfigurationOverview;
using NuLog.Samples.Samples.S3_1_TraceTarget;
using NuLog.Samples.Samples.S3_2_SimpleConsoleTarget;
using NuLog.Samples.Samples.S3_3_ConsoleTarget;
using NuLog.Samples.Samples.S3_4_TextFileTarget;
using NuLog.Samples.Samples.S3_5_EmailTarget;
using NuLog.Samples.Samples.S4_1_LegacyLoggingExtension;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NuLog.Samples
{
    /// <summary>
    /// A program providing an interface to the samples for the NuLog framework
    /// </summary>
    internal class Program
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

        #endregion Structs

        #region Constants and Menu

        // Constants for printing and formatting the samples into their
        //  sections in the menu
        public const string SampleIndent = "    ";

        public const string Section1 = "  1. The Basics";
        public const string Section3 = "  3. Standard Targets";
        public const string Section4 = "  4. Appendix A: Legacy Logging Extension";

        public const string CustomSection1 = "  C.1. Building a Simple Target";
        public const string CustomSection2 = "  C.2. Building an \"Advanced\" Target: Adding a Splash of Color";
        public const string CustomSection3 = "  C.3. Extending the Logger";
        public const string CustomSection4 = "  C.4. Meta Data Providers";
        public const string CustomSection5 = "  C.5. Configuration Extenders";
        public const string CustomSection6 = "  C.6. Creating a Custom Layout";
        public const string CustomSection7 = "  C.7. Creating a Custom Configuration Builder";
        public const string CustomSection8 = "  C.8. Creating Custom Extenders";

        // The ordered list of samples in the menu
        private static readonly IList<SampleBase> SamplesMenu = new List<SampleBase>()
        {
            new HelloWorldSample(Section1, "1.1 Hello World"),
            new TagsRulesSample(Section1, "1.2 Tags and Rules"),
            new TagGroupsSample(Section1, "1.3 Tag Groups"),
            new MetaDataSample(Section1, "1.4 Meta Data"),
            new SynchronousLoggingSample(Section1, "1.5 Synchronous Logging"),
            new RuntimeConfigurationSample(Section1, "1.8 Runtime Configuration"),
            new TraceTargetSample(Section3, "3.1 Trace Target"),
            new SimpleConsoleTargetSample(Section3, "3.2 Simple Console Target"),
            new ConsoleTargetSample(Section3, "3.3 Console Target"),
            new TextFileTargetSample(Section3, "3.4 Text File Target"),
            new EmailTargetSample(Section3, "3.5 Email Target"),
            new LegacyLoggingExtensionSample(Section4, "4.1 Using the Legacy Logging Extension")
        };

        private static readonly IList<SampleBase> CustomSamplesMenu = new List<SampleBase>()
        {
            new ASimpleTargetSample(CustomSection1, "C.1.2 A Simple Target"),
            new MakingALayoutTargetSample(CustomSection1, "C.1.3 Making a Layout Target"),
            new AddingColorSample(CustomSection2, "C.2.1 Adding Color"),
            new AddingConfigurationSample(CustomSection2, "C.2.2 Adding Configuration"),
            new AddingMetaDataSample(CustomSection2, "C.2.3 Adding Meta Data"),
            new ShuttingDownTheTargetSample(CustomSection2, "C.2.4 Shutting Down the Target"),
            new AsynchronousLoggingTargetSample(CustomSection2, "C.2.5 Asynchronous Logging in the Target"),
            new ExtendingTheLoggerSample(CustomSection3, "C.3.1 Extending The Logger"),
            new RuntimeMetaDataProvidersSample(CustomSection4, "C.4.2 Runtime Meta Data Providers"),
            new StaticMetaDataProvidersSample(CustomSection4, "C.4.3 Static Meta Data Providers"),
            new ConfigurationExtendersSample(CustomSection5, "C.5.1 Implementing a Configuration Extender"),
            new CustomLayoutSample(CustomSection6, "C.6.1 Creating a Custom Layout"),
            new CustomConfigBuilderSample(CustomSection7, "C.7.1 Creating a Custom Configuration Builder"),
            new CustomExtenderSample(CustomSection8, "C.8.1 Creating the Trace Listener Extender")
        };

        #endregion Constants and Menu

        #region Main

        // Mainly main stuff ;)
        private static void Main(string[] args)
        {
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
                    Console.WriteLine(string.Format("Executing sample \"{0}\":" + System.Environment.NewLine, selection.Sample.SampleName.Trim()));
                    selection.Sample.ExecuteSample();
                    Console.WriteLine(System.Environment.NewLine + "Press [Enter] to continue");
                    Console.ReadLine();
                }

                // Update the exit flag
                exit = selection.DoExit;
            }

            // Shutdown all logger factories
            LoggerFactoryRegistry.ShutdownAll();
        }

        #endregion Main

        #region Helpers

        // Prints out the samples menu, but first clears the console
        private static void PrintSamplesMenu(string menuHeader, IList<SampleBase> samples, bool clear = false)
        {
            // Clear the console
            if (clear)
                Console.Clear();
            Console.WriteLine(string.Empty);

            Console.WriteLine(string.Format("{0}:", menuHeader));

            // Print the samples, adding in the section headers for each of the sections
            string section = null;
            foreach (var sample in samples)
            {
                if (string.Equals(section, sample.SectionName) == false)
                {
                    section = sample.SectionName;
                    Console.WriteLine(section);
                }
                Console.WriteLine(SampleIndent + sample.SampleName);
            }

            // Some fluff
            Console.WriteLine(string.Empty);
        }

        // Prints out the samples menu and challenges the user for the sample they want to execute
        private static SampleSelection GetNextSampleFromUser()
        {
            // Print the menu menu
            Console.WriteLine("Which sample would you like to execute?");
            PrintSamplesMenu("Standard Implementation", SamplesMenu, true);
            PrintSamplesMenu("Customizing NuLog", CustomSamplesMenu);

            // Challenge the user for a sample number
            Console.Write("Enter a sample number, or [Enter] to exit: ");
            var userSelection = Console.ReadLine();

            // Try to figure out which sample the user wanted by matching
            //  what the user entered to the beginning of the sample name,
            //  or exit if the user indicated they wish to exit by simply
            //  pressing [Enter] (null or empty string)
            SampleBase selectedSample = null;
            bool doExit = false;
            if (string.IsNullOrEmpty(userSelection) == false)
            {
                userSelection = userSelection.Trim();
                selectedSample = SamplesMenu.FirstOrDefault(_ => _.SampleName.Trim().StartsWith(userSelection));

                if (selectedSample == null)
                    selectedSample = CustomSamplesMenu.FirstOrDefault(_ => _.SampleName.Trim().StartsWith(userSelection));

                if (selectedSample == null)
                    Console.WriteLine(string.Format("Sample for \"{0}\" not found", userSelection));
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

        #endregion Helpers
    }
}