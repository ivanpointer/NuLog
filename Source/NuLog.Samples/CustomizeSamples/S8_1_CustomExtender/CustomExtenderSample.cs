/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.Diagnostics;

namespace NuLog.Samples.CustomizeSamples.S8_1_CustomExtender
{
    /// <summary>
    /// An example illustrating an example of a useful extender.  The narration
    /// of this example can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/8.1-Creating-the-Trace-Listener-Extender
    /// </summary>
    public class CustomExtenderSample : SampleBase
    {
        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public CustomExtenderSample(string section, string sample) : base(section, sample) { }

        #endregion Sample Wiring

        // Logging example
        public override void ExecuteSample()
        {
            // Load the configuration
            var factory = new LoggerFactory("CustomizeSamples/S8_1_CustomExtender/NuLog.json");

            // Send a message to trace.
            //  This should be caught by our extender and written to
            //  console because of the rules in our configuration.
            Trace.WriteLine("Hello, World!");
            Debug.WriteLine("Hello, Debug!");
        }
    }
}