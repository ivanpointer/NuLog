/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/20/2014
 * License: MIT (http://opensource.org/licenses/MIT)
 * GitHub: https://github.com/ivanpointer/NuLog
 */

using NuLog;

namespace NuLog.Samples.Samples.S1_1_HelloWorld
{
    /// <summary>
    /// A sample class illustrating how simple it is to implement the NuLog framework:
    ///   1. Implement the NuLog NuGet package and mark the NuLog.json file for export
    ///   2. Implement the "using" statement and get an instance of a logger from the factory
    ///   3. Use the logger
    /// </summary>
    class HelloWorldSample : SampleBase
    {
        // The logger
        private static readonly LoggerBase _logger = LoggerFactory.GetLogger();

        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public HelloWorldSample(string section, string sampleName) : base(section, sampleName) { }

        #endregion

        // Logging example
        public override void ExecuteSample(Arguments args)
        {
            _logger.Log("Log Later!");
            _logger.LogNow("Log Now!");
        }
        
    }
}

