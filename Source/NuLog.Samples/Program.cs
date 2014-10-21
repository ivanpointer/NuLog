using NuLog.Samples.Samples.S1_1_HelloWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Samples
{
    class Program
    {
        #region Structs

        private struct SampleSelection
        {
            public SampleBase Sample;
            public bool DoExit;
        }

        #endregion

        #region Constants

        public const string Section1 = "  1. The Basics";
        public const string SampleIndent = "    ";

        #endregion

        private static readonly IList<SampleBase> Samples = new List<SampleBase>()
        {
            new HelloWorldSample(Section1, SampleIndent + "1.1 Hello World")
        };

        static void Main(string[] args)
        {
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

        private static void PrintSamplesMenu(bool clear = true)
        {
            if(clear)
                Console.Clear();

            Console.WriteLine(String.Empty);

            string section = null;
            foreach (var sample in Samples)
            {
                if (String.Equals(section, sample.SectionName) == false)
                {
                    section = sample.SectionName;
                    Console.WriteLine(section);
                }
                Console.WriteLine(sample.SampleName);
            }

            Console.WriteLine(String.Empty);
        }

        private static SampleSelection GetNextSampleFromUser()
        {
            Console.WriteLine("Which sample would you like to execute?");
            PrintSamplesMenu();

            Console.Write("Enter a sample number, or [Enter] to exit: ");
            var userSelection = Console.ReadLine();

            SampleBase selectedSample = null;
            bool doExit = false;
            if (String.IsNullOrEmpty(userSelection) == false)
            {
                userSelection = userSelection.Trim();
                selectedSample = Samples.FirstOrDefault(_ => _.SampleName.Trim().StartsWith(userSelection));

                if (selectedSample == null)
                    Console.WriteLine(String.Format("Sample for \"{0}\" not found", userSelection));
            }
            else
            {
                doExit = true;
            }

            return new SampleSelection
            {
                Sample = selectedSample,
                DoExit = doExit
            };
        }
    }
}

