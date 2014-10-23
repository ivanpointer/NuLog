using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Samples.Samples.S1_2_TagBasics
{
    public class TagBasicsSample : SampleBase
    {
        public TagBasicsSample(string section, string sample) : base(section, sample) { }

        public override void ExecuteSample(Arguments args)
        {
            LoggerFactory.Initialize("Samples/S1_2_TagBasics/NuLog.json");
            LoggerBase logger = LoggerFactory.GetLogger();

            logger.LogNow("I will go to trace");
            logger.LogNow("I will go to both console and trace", "mytag");
        }
    }
}
