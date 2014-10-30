/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/28/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */

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

        #endregion

        // Logging Example
        public override void ExecuteSample(Arguments args)
        {
            // Initialize here because the samples are constructed only once
            //  We want to be running on the configuration for this sample
            LoggerFactory.Initialize("Samples/S1_4_MetaData/NuLog.json");
            LoggerBase logger = LoggerFactory.GetLogger();

            // Setup the meta data
            var metaData = new Dictionary<string, object>();
            metaData["Hello"] = "World!";

            // Log, sending the meta data
            logger.Log("This is my message", metaData);
        }
    }
}
