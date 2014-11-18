/*
 * Author: Ivan Andrew Pointer (ivan@pointerplace.us)
 * Date: 10/30/2014
 * License: MIT (https://raw.githubusercontent.com/ivanpointer/NuLog/master/LICENSE)
 * Project Home: http://www.nulog.info
 * GitHub: https://github.com/ivanpointer/NuLog
 */

namespace NuLog.Samples.Samples.S1_3_TagGroups
{
    /// <summary>
    /// A sample showing how tag groups work.  The narration of this sample can be found at:
    /// https://github.com/ivanpointer/NuLog/wiki/1.3-Tag-Groups
    /// </summary>
    public class TagGroupsSample : SampleBase
    {
        #region Sample Wiring

        // Wiring for the sample program (menu wiring)
        public TagGroupsSample(string section, string sample) : base(section, sample) { }

        #endregion

        // Execute our sample
        public override void ExecuteSample()
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
