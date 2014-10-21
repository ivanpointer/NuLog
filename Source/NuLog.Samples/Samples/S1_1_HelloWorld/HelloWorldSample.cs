using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NuLog;

namespace NuLog.Samples.Samples.S1_1_HelloWorld
{
    class HelloWorldSample : SampleBase
    {
        private static readonly LoggerBase _logger = LoggerFactory.GetLogger();

        #region Implementation Details

        public HelloWorldSample(string section, string sampleName) : base(section, sampleName) { }

        public override void ExecuteSample(Arguments args)
        {
            _logger.Log("Hello, World!");
        }

        #endregion
    }
}

