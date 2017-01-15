/* © 2017 Ivan Pointer
MIT License: https://github.com/ivanpointer/NuLog/blob/master/LICENSE
Source on GitHub: https://github.com/ivanpointer/NuLog */

using System.Collections.Generic;

namespace NuLog.Samples.Samples.S1_4_MetaData
{
    /// <summary>
    /// A simple sample illustrating the most basic implementation of meta data.  The narration
    ///   of this sample can be found at:
    ///   https://github.com/ivanpointer/NuLog/wiki/1.4-Meta-Data
    /// </summary>
    public class MetaDataSample : SampleBase
    {
        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public MetaDataSample(string section, string sample) : base(section, sample) { }

        #endregion Sample Wiring

        // Logging Example
        public override void ExecuteSample()
        {
            // Initialize here because the samples are constructed only once
            //  We want to be running on the configuration for this sample
            using (var factory = new LoggerFactory("Samples/S1_4_MetaData/NuLog.json"))
            {
                var logger = factory.Logger();

                // Setup the meta data
                var metaData = new Dictionary<string, object>();
                metaData["Hello"] = "World!";

                // Log, sending the meta data
                logger.Log("This is my message", metaData);
            }
        }
    }
}