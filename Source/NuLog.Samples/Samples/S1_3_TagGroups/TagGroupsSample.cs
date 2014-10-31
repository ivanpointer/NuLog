using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuLog.Samples.Samples.S1_3_TagGroups
{
    public class TagGroupsSample : SampleBase
    {
        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public TagGroupsSample(string section, string sample) : base(section, sample) { }

        #endregion

        public override void ExecuteSample(Arguments args)
        {
            // Initialize here because the samples are constructed only once
            //  We want to be running on the configuration for this sample
            LoggerFactory.Initialize("Samples/S1_3_TagGroups/NuLog.json");
            LoggerBase logger = LoggerFactory.GetLogger();

            logger.LogNow("I like oranges!", "orange");
            logger.LogNow("I really like peaches!", "peach");
            logger.LogNow("I like tomatoes!", "tomato");
            logger.LogNow("I like fruit!", "fruit");
            logger.LogNow("I don't like mushrooms!", "mushroom");

            logger.LogNow("Debug message!", "debug");
            logger.LogNow("Info message!", "info");
            logger.LogNow("Fatal message!", "fatal");
        }
    }
}
