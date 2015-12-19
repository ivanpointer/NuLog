﻿/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/20/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

namespace NuLog.Samples.Samples.S1_1_HelloWorld
{
    /// <summary>
    /// A sample class illustrating how simple it is to implement the NuLog framework:
    ///   1. Implement the NuLog NuGet package and mark the NuLog.json file for export
    ///   2. Implement the "using" statement and get an instance of a logger from the factory
    ///   3. Use the logger
    /// The narration for this sample is found at:
    /// https://github.com/ivanpointer/NuLog/wiki/1.1-Hello-World
    /// </summary>
    internal class HelloWorldSample : SampleBase
    {
        // The logger
        private static readonly LoggerBase _logger = LoggerFactory.GetLogger();

        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public HelloWorldSample(string section, string sampleName) : base(section, sampleName) { }

        #endregion Sample Wiring

        // Logging example
        public override void ExecuteSample()
        {
            // Initialize here because the samples are constructed only once
            //  We want to be running on the configuration for this sample
            LoggerFactory.Initialize("Samples/S1_1_HelloWorld/NuLog.json");
            LoggerBase logger = LoggerFactory.GetLogger();

            _logger.Log("Log Later!");
            _logger.LogNow("Log Now!");
        }
    }
}